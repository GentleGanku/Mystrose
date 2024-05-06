using Mystrose.Controls.Main;
using Mystrose.ScriptMachine.Objects;
using Mystrose.ScriptMachine.Enumerations;
using Mystrose.Global;
using System;
using System.Threading;
using System.Threading.Tasks;
using System.Collections.Generic;
using Mystrose.ScriptMachine.Inputs;
using Mystrose.Systems;
using Mystrose.GameModels.Master;
using Mystrose.GameModels.Base;
using Mystrose.GameModels.Environment;

namespace Mystrose.ScriptMachine;

public class ScriptEngine
{

    #region Constructors
    public ScriptEngine(GameHost host, ScriptEngineType type = ScriptEngineType.Regular)
    {
        Type = type;
        Host = host;

        Task = new(() => OnScriptRun());
    }
    #endregion

    #region Delegates & Handlers
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
        get => Host.World.Master;
    }

    public Area Area
    {
        get => Host.World.Area;
    }

    public SkillManager Skills
    {
        get => Host.World.Skills;
    }

    public List<Quest> Quests
    {
        get => Host.World.Quests;
    }

    public InventoryManager Inventory
    {
        get => Host.World.Inventory;
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
        get => Task.Status == TaskStatus.Running;
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
        CurrentIndex = 0;

        CTSource = new CancellationTokenSource();

        SetCurrentVariables();
        ScriptTriggerHandler += OnTriggerCall;
        Task = Task.Factory.StartNew(OnScriptRun, CTSource.Token, TaskCreationOptions.LongRunning, TaskScheduler.Default);
    }

    public void StopScript()
    {
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
        while (!CTSource.IsCancellationRequested)
        {
            ScriptCommand targetCommand = CurrentStance.Commands[CurrentIndex];

            try
            {
                await targetCommand.Execute(this);
            }
            catch (Exception e)
            {
                // TODO: Handle exception
                targetCommand.EndResult = ScriptResultType.Error;
                StopScript();
            }
            // TODO: Handle the command's Result

            CurrentIndex += targetCommand.EndResult == ScriptResultType.Success ? 1 : 2;
            CurrentStance.SetIndex(CurrentIndex);

            if (CurrentIndex >= CurrentStance.Commands.Count)
            {
                CurrentIndex = 0;
                CurrentStance.SetIndex(0);
            }

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

    public void InvokeTrigger(ScriptTriggerType type, Dictionary<string, ScriptParameter> objectOnEvent)
    {
        if (!IsRunning)
        {
            return;
        }

        ScriptTriggerHandler.Invoke(type, objectOnEvent);
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
        if (var.String == null)
        {
            return false;
        }

        return Regexes.ScriptVariable().IsMatch(var.String);
    }

    public ScriptParameter GetVariableValue(ScriptParameter var)
    {
        return GetVariableValidation(var) ? CurrentLoadout.Variables[Regexes.ScriptVariable().Replace(var.String, "")].KeyValuePair : var;
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

}
