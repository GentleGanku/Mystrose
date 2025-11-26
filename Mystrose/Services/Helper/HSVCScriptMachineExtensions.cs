namespace Mystrose.Services.Helper;

public class HSVCScriptMachineExtensions() : HelperService(nameof(HSVCScriptMachineExtensions))
{

    #region (Static) Fields
    public static HSVCScriptMachineExtensions Instance
    {
        get
        {
            if (_instance is null)
            {
                _instance = new HSVCScriptMachineExtensions();
                _instance.Construct();
            }

            return _instance;
        }
    }
    #endregion

    #region (Private) Fields
    private static HSVCScriptMachineExtensions? _instance;
    #endregion

    #region (Readonly) Fields
    private readonly Dictionary<string, ScriptParameterExtension> _innateParameters = new()
    {
        [ScriptMachineParser.INNATE_PREFIX + "Label"] = new(ScriptParameterInputType.Parameter, "", "Text label to be specifically displayed on the dashboard"),
        [ScriptMachineParser.INNATE_PREFIX + "Is Enabled"] = new(ScriptParameterInputType.Options, "True/False", "Indicator of whether the command can be executed by the dashboard"),
        [ScriptMachineParser.CONDITIONAL_PREFIX + "Reversal"] = new(ScriptParameterInputType.Options, "False/True", "Indicator of whether the conditional check should be reversed")
    };
    private readonly Dictionary<ScriptEntityModelType, Type> _readableModels = new()
    {
        [ScriptEntityModelType.ActiveSkill] = typeof(RMActiveSkill),
        [ScriptEntityModelType.Area] = typeof(RMArea),
        [ScriptEntityModelType.Aura] = typeof(RMAura),
        [ScriptEntityModelType.Avatar] = typeof(RMAvatar),
        [ScriptEntityModelType.Cell] = typeof(RMCell),
        [ScriptEntityModelType.CombatMessage] = typeof(RMCombatMessage),
        [ScriptEntityModelType.EventMessage] = typeof(RMEventMessage),
        [ScriptEntityModelType.Faction] = typeof(RMFaction),
        [ScriptEntityModelType.InventoryItem] = typeof(RMInventoryItem),
        [ScriptEntityModelType.ItemDrop] = typeof(RMItemDrop),
        [ScriptEntityModelType.Monster] = typeof(RMMonster),
        [ScriptEntityModelType.Quest] = typeof(RMQuest),
        [ScriptEntityModelType.Self] = typeof(RMSelf),
        [ScriptEntityModelType.ShopItem] = typeof(RMShopItem),
        [ScriptEntityModelType.ScriptVariable] = typeof(RMScriptVariable)
    };
    private readonly Dictionary<string, ScriptCodeline> _codelines = new()
    {
        ["SCL.001"] = new ACLIndexJump(),
        ["SCL.002"] = new ACLTargetSetter(),
        ["SCL.003"] = new ACLStanceSwitch(),
        ["SCL.004"] = new ACLVariableSetter(),
        ["SCL.005"] = new ACLWait(),
        ["SCL.006"] = new ACLSkillUse(),
        ["SCL.007"] = new ACLRest(),
        ["SCL.008"] = new ACLMapMovement(),

        ["SCL.101"] = new SCLFiller(),

        ["SCL.201"] = new SCLStack(),

        ["SCL.301"] = new SCLStatement(),

        ["SCL.401"] = new SCLTrigger(),

        ["SCL.501"] = new SCLVariable(),
    };
    #endregion

    #region Methods: Builder
    public override void Construct()
    {
        try
        {
            Log("Settings constructed successfully.", "Construct");
        }
        catch (Exception ex)
        {
            Log(ex.ToString(), "Construct");
        }
    }

    public override void Deconstruct()
    {
        try
        {
            Log("Settings deconstructed successfully.", "Deconstruct");
        }
        catch (Exception ex)
        {
            Log(ex.ToString(), "Deconstruct");
        }
    }
    #endregion

    #region Methods: Helper
    private ScriptParameter TranslateExtensionToParameter(ScriptParameterExtension prmExtension)
    {
        return prmExtension.InputType switch
        {
            ScriptParameterInputType.Parameter => new ScriptParameter(prmExtension.Value, prmExtension.Hint),
            ScriptParameterInputType.Conditional => new ScriptConditional(ScriptConditionType.Equal, prmExtension.Value, prmExtension.Hint),
            ScriptParameterInputType.Options => new ScriptOptions(prmExtension.Value, prmExtension.Hint),
            ScriptParameterInputType.KeyValuePair => new ScriptKeyValuePair(prmExtension.Value.Split('|')[0], prmExtension.Value.Split('|')[1], prmExtension.Hint),
            _ => new ScriptParameter(prmExtension.Value, prmExtension.Hint)
        };
    }
    #endregion

    #region Methods: Getter
    public Response<Dictionary<string, ScriptParameter>?> RetrieveInnateParameters()
    {
        if (_innateParameters.Count == 0)
        {
            return new(false,
                "No innate parameters found.",
                null);
        }

        Dictionary<string, ScriptParameter> innates = _innateParameters
            .Where(prm => prm.Key.StartsWith(ScriptMachineParser.INNATE_PREFIX))
            .ToDictionary(
                prm => prm.Key,
                prm => TranslateExtensionToParameter(prm.Value));

        return new(true,
            "Innate parameters retrieved successfully.",
            innates);
    }
    public Response<Dictionary<string, ScriptParameter>?> RetrieveConditionalParameters()
    {
        if (_innateParameters.Count == 0)
        {
            return new(false,
                "No innate parameters found.",
                null);
        }

        Dictionary<string, ScriptParameter> innates = _innateParameters
            .Where(prm => prm.Key.StartsWith(ScriptMachineParser.INNATE_PREFIX) ||
                prm.Key.StartsWith(ScriptMachineParser.CONDITIONAL_PREFIX))
            .ToDictionary(
                prm => prm.Key,
                prm => TranslateExtensionToParameter(prm.Value));

        return new(true,
            "Innate parameters retrieved successfully.",
            innates);
    }

    public Response<IReadableModel?> RetrieveReadableModel(ScriptEntityModelType modelType, object? gameModel = default, World? world = default)
    {
        if (_readableModels.Count == 0)
        {
            return new(false,
                "No readable models found.",
                null);
        }

        Type type = _readableModels[modelType];
        IReadableModel readableModel = (IReadableModel)Activator.CreateInstance(type, gameModel, world)!;

        return new(true,
            "Readable model retrieved successfully.",
            readableModel);
    }

    public Response<string[]?> RetrieveReadableModelNames()
    {
        if (_readableModels.Count == 0)
        {
            return new(false,
                "No readable models found.",
                null);
        }

        string[] modelNames = [.. _readableModels.Keys.Select(type => JSONParser.Serialize(type))];

        return new(true,
            "Readable model names retrieved successfully.",
            modelNames);
    }

    public Response<ScriptCodeline?> RetrieveScriptCodeline(string id)
    {
        if (_codelines.Count == 0)
        {
            return new(false,
                "No script codelines found.",
                null);
        }

        ScriptCodeline scriptCodeline = _codelines[id].Clone();

        return new(true,
            "Script codeline retrieved successfully.",
            scriptCodeline);
    }

    public Response<ScriptCodeline[]?> RetrieveScriptCodelinesList()
    {
        if (_codelines.Count == 0)
        {
            return new(false,
                "No script codelines found.",
                null);
        }

        ScriptCodeline[] codelinesList = [.. _codelines.Values];

        return new(true,
            "Script codelines list retrieved successfully.",
            codelinesList);
    }

    public Response<ScriptCodeline[]?> RetrieveScriptCodelinesList(ScriptCodelineType codelineType)
    {
        if (_codelines.Count == 0)
        {
            return new(false,
                "No script codelines found.",
                null);
        }

        ScriptCodeline[] codelinesList = [.. _codelines.Values
            .Where(cdln => cdln.Type.Equals(codelineType))];

        return new(true,
            "Script codelines list retrieved successfully.",
            codelinesList);
    }
    #endregion

}
