namespace Mystrose.Core.ScriptMachine.Engines;

public class ScriptEngine
{

    #region Constructors
    public ScriptEngine(GameHost host, ScriptEngineType type = ScriptEngineType.Regular)
    {
        Type = type;
        Host = host;

        IsRunning = false;
    }
    #endregion

    #region Delegates & Handlers
    public delegate void ScriptRunEvent(ScriptCommand cmd);
    public event ScriptRunEvent ScriptRunHandler;

    public delegate void ScriptResultEvent(ScriptResultType result, string msg = "");
    public event ScriptResultEvent ScriptResultHandler;

    private delegate void ScriptTriggerEvent(ScriptTriggerType type, Dictionary<string, ScriptParameter> objectOnEvent);
    private event ScriptTriggerEvent ScriptTriggerHandler;
    #endregion

    #region Private Fields
    private Task Task
    {
        get;
        set;
    }

    private CancellationTokenSource CTSource
    {
        get;
        set;
    }
    #endregion

    #region Fields
    public FlashPlayer Flash
    {
        get => Host.Flash;
    }

    public World World
    {
        get => Host.World;
    }

    public MainAvatar Master
    {
        get => Host.World.Avatar;
    }

    public Area Area
    {
        get => Host.World.Area;
    }

    public ActiveSkills Skills
    {
        get => Host.World.Skills;
    }

    public List<Quest> Quests
    {
        get => Host.World.Quests;
    }

    public InventoryManager Inventory
    {
        get => Host.World.Inventories[InventoryType.Base];
    }
    #endregion

    #region Properties: Engine
    public GameHost Host
    {
        get;
        private set;
    }

    public ScriptEngineType Type
    {
        get;
        private set;
    }

    public bool IsPaused
    {
        get;
        private set;
    }

    public bool IsRunning
    {
        //get => Task.Status == TaskStatus.Running;
        get;
        set;
    }
    #endregion

    #region Properties: Data
    public ScriptLoadout CurrentLoadout
    {
        get;
        set;
    }

    public ScriptStance CurrentStance
    {
        get;
        set;
    }

    public int CurrentIndex
    {
        get;
        set;
    }
    #endregion

    #region Methods: Script
    public void StartScript()
    {
        IsRunning = true;
        CurrentIndex = 0;

        CTSource = new CancellationTokenSource();

        SetCurrentVariables();
        ScriptTriggerHandler += OnTriggerCall;
        Task = Task.Factory.StartNew(OnScriptRun, CTSource.Token, TaskCreationOptions.LongRunning, TaskScheduler.Default);
    }

    public void StopScript()
    {
        IsRunning = false;
        CTSource.Cancel();
        ScriptTriggerHandler -= OnTriggerCall;
        ResetCurrentVariables();

        foreach (ScriptStance stance in CurrentLoadout.Stances)
        {
            stance.SetIndex(0);
        }
    }

    public void PauseScript()
    {
        IsPaused = true;

        CTSource.Cancel();
        ScriptTriggerHandler -= OnTriggerCall;
    }

    public void ResumeScript()
    {
        IsPaused = false;

        CTSource = new CancellationTokenSource();

        ScriptTriggerHandler += OnTriggerCall;
        Task = Task.Factory.StartNew(OnScriptRun, CTSource.Token, TaskCreationOptions.LongRunning, TaskScheduler.Default);
    }

    public async Task OnScriptRun()
    {
        string errorMsg = string.Empty;

        while (!CTSource.IsCancellationRequested)
        {
            ScriptCommand targetCommand = CurrentStance.Commands[CurrentIndex];

            ScriptRunHandler.Invoke(targetCommand);

            try
            {
                await targetCommand.Execute(this);
            }
            catch (Exception e)
            {
                errorMsg = e.Message;
                targetCommand.EndResult = ScriptResultType.Error;
                break;
            }

            switch (targetCommand.EndResult)
            {
                case ScriptResultType.Failure:
                    ScriptResultHandler.Invoke(targetCommand.EndResult, $"[Stance: {CurrentStance.Name}, Index: {CurrentIndex}] The command has been skipped due to it returning a False result.");
                    break;
                case ScriptResultType.Success:
                    ScriptResultHandler.Invoke(targetCommand.EndResult, $"[Stance: {CurrentStance.Name}, Index: {CurrentIndex}] Continuing the script as usual.");
                    break;

                case ScriptResultType.Cancel:
                    StopScript();

                    ScriptResultHandler.Invoke(targetCommand.EndResult, $"[Stance: {CurrentStance.Name}, Index: {CurrentIndex}] The script has been stopped.");
                    return;
                case ScriptResultType.Error:
                    StopScript();

                    ScriptResultHandler.Invoke(targetCommand.EndResult, $"[Stance: {CurrentStance.Name}, Index: {CurrentIndex}] An error has occurred while executing the script.\r\nError: {errorMsg}");
                    return;
            };

            CurrentIndex += 1;
            CurrentStance.SetIndex(CurrentIndex);
            if (CurrentIndex >= CurrentStance.Commands.Count)
            {
                CurrentIndex = 0;
                CurrentStance.SetIndex(0);
            }

            targetCommand.EndResult = ScriptResultType.Busy;
            await Task.Delay(100);
        }
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

    public void InvokeTrigger(ScriptTriggerType type, object objectOnEvent)
    {
        if (!IsRunning)
        {
            return;
        }

        Dictionary<string, ScriptParameter> parameters = ScriptRepository.ConvertToParameters(objectOnEvent);
        ScriptTriggerHandler.Invoke(type, parameters);
    }
    #endregion

    #region Methods: Variable
    public void SetCurrentVariables()
    {
        foreach (SCMDVariable varCmd in CurrentLoadout.PresetVariables)
        {
            varCmd.Execute(this);
        }
    }

    public void ResetCurrentVariables()
    {
        CurrentLoadout.Variables.Clear();
    }

    public bool GetVariableValidation(ScriptParameter var)
    {
        if (string.IsNullOrEmpty(var.String))
        {
            return false;
        }

        return SVCRegex.ScriptVariable().IsMatch(var.String);
    }

    public ScriptParameter GetVariableValue(ScriptParameter param)
    {
        if (GetVariableValidation(param))
        {
            string combinedValue = param.String;

            foreach (Match match in SVCRegex.ScriptVariable().Matches(param.String))
            {
                string varName = match.Value.Replace("{", "").Replace("}", "");

                if (!CurrentLoadout.Variables.ContainsKey(varName))
                {
                    continue;
                }

                combinedValue = combinedValue.Replace(match.Value, CurrentLoadout.Variables[varName]!.Value);
            }

            ScriptParameter varParam = new(combinedValue);
            return varParam;
        }

        return param;
    }
    #endregion

    #region Methods: Event
    private void OnTriggerCall(ScriptTriggerType type, Dictionary<string, ScriptParameter> objectOnEvent)
    {
        foreach (SCMDTrigger cmd in CurrentLoadout.Triggers.FindAll(t => t.IsEnabled && t.TriggerType == type))
        {
            if (!cmd.IsValid(this, objectOnEvent))
            {
                continue;
            }

            cmd.Execute(this);
        }
    }
    #endregion
    
    // Placeholder
    public void Destruct()
    {
        // Placeholder
    }

}
