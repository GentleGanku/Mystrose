namespace Mystrose.Core.ScriptMachine.Engines;

public class ScriptEngine(HSTGame host)
{

    #region Delegates & Handlers
    public delegate void StatusHandler(ScriptEngineStatusType status, string context = "");
    public event StatusHandler StatusEvent;

    public delegate void CodelineHandler(ScriptCodeline cdln);
    public event CodelineHandler MainCodelineEvent;
    public event CodelineHandler SideCodelineEvent;

    public delegate bool TriggerHandler(ScriptEngine engine, ScriptEntityModelType modelType, Dictionary<string, ScriptParameter> parameters);
    public event TriggerHandler TriggerEvent;

    public delegate void ErrorHandler(Exception exception, string context = "");
    public event ErrorHandler ErrorEvent;
    #endregion

    #region Fields
    public ISVCFlashAPI FlashAPI => host.FlashAPI;
    public World World => MSVCWorld.Instance[host.Identifier.Codename].Item2!;
    public MainAvatar Player => World.Avatar;
    public Area Map => World.Area;
    public ActiveSkills Skills => World.Skills;
    public List<Quest> Quests => [.. World.Quests];
    public InventoryManager Inventory => World.Inventories[InventoryType.Base];
    public InventoryManager TempInventory => World.Inventories[InventoryType.Temp];
    public InventoryManager HouseInventory => World.Inventories[InventoryType.House];
    public InventoryManager BankInventory => World.Inventories[InventoryType.Bank];

    #endregion

    #region Properties: Tasking
    public virtual ScriptLoadout ActiveLoadout
    {
        get;
        protected set;
    } = new ScriptLoadout();

    public virtual ScriptEngineStatusType Status
    {
        get;
        protected set;
    } = ScriptEngineStatusType.Idle;

    public virtual Task RunnerTask
    {
        get;
        protected set;
    } = new Task(() => {});

    public virtual CancellationTokenSource CTSource
    {
        get;
        protected set;
    } = new CancellationTokenSource();

    public virtual ScriptCodeline[] OngoingCodelines
    {
        get;
        protected set;
    } = [];
    #endregion

    #region Methods: Tasking
    public virtual void EnlistLoadout(ScriptLoadout loadout)
    {
        ActiveLoadout = loadout;
        StateEngineToBe(ScriptEngineStatusType.Idle);
    }

    public virtual async Task OnRunningScript()
    {
        while (!CTSource.Token.IsCancellationRequested)
        {
            ScriptCodeline cdln = ActiveLoadout.ActiveStance.Commands[ActiveLoadout.ActiveStance.Index];
            cdln.Status = ScriptCodelineStatusType.Idle;

            try
            {
                await cdln.Execute(this);
            }
            catch (Exception ex)
            {
                StateEngineToBe(ScriptEngineStatusType.Crash, ex);
                break;
            }

            ActiveLoadout.ActiveStance.JumpIndex(1);
            if ((ActiveLoadout.ActiveStance.Index + 1) >= ActiveLoadout.ActiveStance.Commands.Count)
            {
                ActiveLoadout.ActiveStance.SetIndex(0);
            }

            await Task.Delay(ScriptMachineParser.EXECUTION_INTERVAL_TIME, CTSource.Token);
        }
    }

    public virtual void Start()
    {
        RefreshVariables();
        RefreshTriggers();

        CTSource = new CancellationTokenSource();
        RunnerTask = Task.Run(async () =>
        {
            try
            {
                await OnRunningScript();
            }
            catch (Exception ex)
            {
                StateEngineToBe(ScriptEngineStatusType.Crash, ex);
            }
        }, CTSource.Token);
    }

    public virtual void Resume()
    {
        RefreshTriggers();

        CTSource = new CancellationTokenSource();
        RunnerTask = Task.Run(async () =>
        {
            try
            {
                await OnRunningScript();
            }
            catch (Exception ex)
            {
                StateEngineToBe(ScriptEngineStatusType.Crash, ex);
            }
        }, CTSource.Token);
    }

    public virtual void Stop()
    {
        CTSource.Cancel();
        RefreshTriggers();
        CancelOngoing();

        RefreshStances();
    }

    public virtual void Pause()
    {
        CTSource.Cancel();
        RefreshTriggers();
        CancelOngoing();
    }
    #endregion

    #region Methods: Utility
    public async Task WaitForCondition(Func<bool> predicate, int maxLoop = -1)
    {
        int loopCount = 0;

        while (!predicate())
        {
            await Task.Delay(250);
            loopCount++;

            if (loopCount == maxLoop && maxLoop != -1)
            {
                break;
            }
        }
    }
    #endregion

    #region Methods: Codeline Calls
    public virtual void CancelOngoing()
    {
        foreach (var cdln in OngoingCodelines)
        {
            StateCodelineToBe(ScriptCodelineStatusType.Canceled, cdln);
        }
    }

    public virtual void RefreshStances()
    {
        foreach (var stn in ActiveLoadout.Stances)
        {
            stn.SetIndex(0);
        }
    }

    public virtual void RefreshTriggers()
    {
        foreach (var cdln in ActiveLoadout.Triggers)
        {
            if (Status is ScriptEngineStatusType.Running)
            {
                TriggerEvent += cdln.ValidateCondition;
            }
            else
            {
                TriggerEvent -= cdln.ValidateCondition;
            }
        }
    }

    public virtual void Trigger(ScriptEntityModelType modelType, Dictionary<string, ScriptParameter> parameters)
    {
        TriggerEvent?.Invoke(this, modelType, parameters);
    }

    public virtual async void RefreshVariables()
    {
        ActiveLoadout.Variables.Clear();

        if (Status is ScriptEngineStatusType.Running)
        {
            foreach (var cdln in ActiveLoadout.PresetVariables)
            {
                await cdln.Execute(this);
            }
        }
    }

    public virtual bool ValidateVariable(ScriptParameter parameter)
    {
        if (string.IsNullOrEmpty(parameter.String))
        {
            return false;
        }

        return ScriptRegexes.ScriptVariable().IsMatch(parameter.String);
    }

    public virtual ScriptParameter GetVariableValue(ScriptParameter parameter)
    {
        if (ValidateVariable(parameter))
        {
            string combinedValue = parameter.String;

            foreach (Match match in ScriptRegexes.ScriptVariable().Matches(parameter.String))
            {
                string varName = match.Value.Replace("{", "").Replace("}", "");

                if (!ActiveLoadout.Variables.ContainsKey(varName))
                {
                    continue;
                }

                combinedValue = combinedValue.Replace(match.Value, ActiveLoadout.Variables[varName]!.String);
            }

            return new(combinedValue);
        }

        return parameter;
    }
    #endregion

    #region Methods: State Management
    public virtual void StateEngineToBe(ScriptEngineStatusType targetStatus, Exception? exception = default)
    {
        ScriptEngineStatusType prevStatus = Status;
        Status = targetStatus;

        switch (targetStatus)
        {
            case ScriptEngineStatusType.Idle:
                ActiveLoadout.ActiveStance.SetIndex(0);

                StatusEvent?.Invoke(targetStatus, "Script engine is now idle.");
                break;

            case ScriptEngineStatusType.Running:
                if (prevStatus is ScriptEngineStatusType.Idle)
                {
                    Start();
                    StatusEvent?.Invoke(targetStatus, "Starting the script engine now...");
                }
                else if (prevStatus is ScriptEngineStatusType.Paused)
                {
                    Resume();
                    StatusEvent?.Invoke(targetStatus, "Resuming the script engine now...");
                }
                break;

            case ScriptEngineStatusType.Paused:
                Pause();
                StatusEvent?.Invoke(targetStatus, "Script engine has been paused.");
                break;
            case ScriptEngineStatusType.Stopped:
                Stop();
                StatusEvent?.Invoke(targetStatus, "Script engine has been successfully stopped.");
                break;

            case ScriptEngineStatusType.Crash:
                Stop();
                StatusEvent?.Invoke(targetStatus, "Script engine has crashed. See below for more information:\r\n" + exception!.Message);
                ErrorEvent?.Invoke(exception!, "Script engine has crashed. See below for more information:\r\n" + exception!.Message);
                break;
        }
    }

    public virtual void StateCodelineToBe(ScriptCodelineStatusType targetStatus, ScriptCodeline cdln)
    {
        ScriptCodelineStatusType prevStatus = cdln.Status;

        if (prevStatus is ScriptCodelineStatusType.Canceled)
        {
            return;
        }

        cdln.Status = targetStatus;

        switch (targetStatus)
        {
            case ScriptCodelineStatusType.Executing:
                OngoingCodelines = [.. OngoingCodelines, cdln];
                break;

            case ScriptCodelineStatusType.Canceled:
                cdln.Cancel(this);

                OngoingCodelines = [.. OngoingCodelines.Where(c => c != cdln)];
                break;

            case ScriptCodelineStatusType.Failed:
            case ScriptCodelineStatusType.Succeed:
                OngoingCodelines = [.. OngoingCodelines.Where(c => c != cdln)];
                break;
        }
        
        switch (prevStatus)
        {
            case ScriptCodelineStatusType.Idle:
                MainCodelineEvent?.Invoke(cdln);
                break;
            case ScriptCodelineStatusType.Standby:
                SideCodelineEvent?.Invoke(cdln);
                break;
        }
    }
    #endregion


}
