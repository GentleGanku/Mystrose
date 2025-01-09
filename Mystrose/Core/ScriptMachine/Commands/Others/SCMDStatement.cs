namespace Mystrose.Core.ScriptMachine.Commands.Others;

public class SCMDStatement : ScriptCommand, IStatementCommand, IStackable
{

    #region Constructor
    public SCMDStatement() : base(ScriptCommandType.Statement, "SCMD03", "Statement", "A script command that executes a set of internal commands within its scope, when its prerequisites are met. It executes otherwise if the Reverse Check is set to True. Any kinds of commands, other than Trigger and Variable ones, are executable in this scope. Stacks up to 20 internal commands.")
    {
        Parameters = new()
        {
            ["Label Name"] = new ScriptParameter("Label", "The label name of the statement to be used."),
            ["Statement Type"] = new ScriptOptions("Variable / Self / Player / Monster / Skill / Aura / Map / Faction / Quest / Item / Drop", "The type of the statement to be checked."),
            ["Reverse Check"] = new ScriptParameter(false, "If True, the command will execute with the statement being False.")
        };
        SecondaryParameters = [];
        InternalCommands = [];
    }
    #endregion

    #region Properties
    [JsonIgnore]
    public string LabelName
    {
        get => Parameters["Label Name"].ToString()!;
    }

    [JsonIgnore]
    public int StackLimit
    {
        get => 20;
    }

    public List<ScriptCommand> InternalCommands
    {
        get;
        set;
    }

    [JsonIgnore]
    public ScriptStatementType? StatementType
    {
        get => Enum.TryParse(Parameters["Statement Type"].String.Replace(" ", ""), out ScriptStatementType type) ? type : null;
    }

    [JsonIgnore]
    public bool IsReverseChecked
    {
        get => Parameters["Reverse Check"].Boolean;
    }
    #endregion

    #region Methods: Interface
    public bool IsInputValid(ScriptCommand cmd)
    {
        return cmd.Type != ScriptCommandType.Trigger && cmd.Type != ScriptCommandType.Variable && InternalCommands.Count <= StackLimit;
    }
    #endregion

    #region Methods: Override
    public override ScriptCommand Clone()
    {
        return new SCMDStatement()
        {
            InternalCommands = ScriptRepository.CloneToCommandsList(InternalCommands),
            Parameters = ScriptRepository.CloneToParameters(Parameters),
            SecondaryParameters = ScriptRepository.CloneToSecondaryParameters(SecondaryParameters),
            EndResult = JsonSerializer.Deserialize<ScriptResultType>(JsonSerializer.Serialize(EndResult))
        };
    }

    public override Dictionary<string, ScriptParameter> PassSecondaryParameters(string key)
    {
        if (SecondaryParameters.TryGetValue(key, out Dictionary<string, ScriptParameter>? value))
        {
            return value;
        }

        IReadableModel targetModel = StatementType switch
        {
            ScriptStatementType.Variable => new RMScriptVariable(),
            ScriptStatementType.Self => new RMSelf(),
            ScriptStatementType.Player => new RMAvatar(),
            ScriptStatementType.Monster => new RMMonster(),
            ScriptStatementType.Skill => new RMActiveSkill(),
            ScriptStatementType.Aura => new RMAura(),
            ScriptStatementType.Map => new RMArea(),
            ScriptStatementType.Faction => new RMFaction(),
            ScriptStatementType.Quest => new RMQuest(),
            ScriptStatementType.Item => new RMInventoryItem(),
            ScriptStatementType.Drop => new RMItemDrop(),
            _ => throw new NotImplementedException()
        };

        SecondaryParameters.Clear();
        SecondaryParameters[key] = ScriptRepository.ConvertToConditionals(targetModel.KeyProperties);
        SecondaryParameters["Optional"] = ScriptRepository.ConvertToConditionals(targetModel).Where(k => !SecondaryParameters[key].ContainsKey(k.Key)).ToDictionary();

        return SecondaryParameters[key];
    }

    public override async Task Execute(ScriptEngine engine)
    {
        Dictionary<string, ScriptParameter> secondaryPrms = SecondaryParameters[Parameters["Statement Type"].String];
        object? target = StatementType switch
        {
            ScriptStatementType.Variable => new RMScriptVariable(engine.CurrentLoadout.Variables[secondaryPrms["Key"].String]),
            ScriptStatementType.Self => new RMSelf(engine.Master),
            ScriptStatementType.Player => new RMAvatar(engine.Area.Players.Find(p => p.Name.Equals(secondaryPrms["Name"].GetVar(engine).String, StringComparison.OrdinalIgnoreCase))),
            ScriptStatementType.Monster => new RMMonster(engine.Area.Monsters.Find(m => m.MonMapID == secondaryPrms["Monster Map ID"].GetVar(engine).Integer)),
            ScriptStatementType.Skill => new RMActiveSkill(engine.Skills.Find(s => s.Index == secondaryPrms["Index"].GetVar(engine).Integer)),
            ScriptStatementType.Aura => new RMAura(engine.World.Auras[JsonSerializer.Deserialize<EntityType>(secondaryPrms["Target Type"].GetVar(engine).String), secondaryPrms["Target ID"].GetVar(engine).String, secondaryPrms["Name"].GetVar(engine).String]),
            ScriptStatementType.Map => new RMArea(engine.Area),
            ScriptStatementType.Faction => new RMFaction(engine.World.Factions.Find(f => f.Name.Equals(secondaryPrms["Name"].GetVar(engine).String, StringComparison.OrdinalIgnoreCase))),
            ScriptStatementType.Quest => new RMQuest(engine.Quests.Find(q => q.ID == secondaryPrms["ID"].GetVar(engine).Integer)),
            ScriptStatementType.Item => new RMInventoryItem(engine.World.Inventories[JsonSerializer.Deserialize<InventoryType>(secondaryPrms["Inventory Type"].GetVar(engine).String)][secondaryPrms["Name"].GetVar(engine).String]),
            ScriptStatementType.Drop => new RMItemDrop(engine.World.Drops.Find(d => d.Name.Equals(secondaryPrms["Name"].GetVar(engine).String, StringComparison.OrdinalIgnoreCase)))
        };

        if (target is null)
        {
            EndResult = ScriptResultType.Failure;
            return;
        }

        JsonObject jsonTarget = JsonSerializer.Deserialize<JsonObject>(JsonSerializer.Serialize(target))!;
        bool isConditionTrue = false;

        foreach (KeyValuePair<string, ScriptParameter> parameter in secondaryPrms)
        {
            ScriptParameter value = new(jsonTarget[parameter.Key]!.ToString());
            ScriptConditional condition = (ScriptConditional)parameter.Value;
            isConditionTrue = condition.IsTrue(value.Object, condition.GetVar(engine));

            if (isConditionTrue != !IsReverseChecked)
            {
                break;
            }
        }

        if (IsReverseChecked == true)
        {
            EndResult = isConditionTrue == false ? ScriptResultType.Success : ScriptResultType.Failure;
        }
        else
        {
            EndResult = isConditionTrue == true ? ScriptResultType.Success : ScriptResultType.Failure;
        }

        if (EndResult == ScriptResultType.Success)
        {
            foreach (ScriptCommand cmd in InternalCommands)
            {
                try
                {
                    await cmd.Execute(engine);
                }
                catch (Exception e)
                {
                    EndResult = ScriptResultType.Error;
                    return;
                }
            }
        }
    }

    public override string ToString()
    {
        return $"If the {LabelName} statement is " + (IsReverseChecked ? "False" : "True") + ", then ...";
    }
    #endregion

}

