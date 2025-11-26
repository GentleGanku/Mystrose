namespace Mystrose.Core.ScriptMachine.Codelines.Action;

public class ACLMapMovement : SCLAction
{

    #region Properties: Attributes
    public override string ID
    {
        get => "SCL.008";
    }

    public override ScriptCodelineType Type
    {
        get => ScriptCodelineType.Action;
    }

    public override string Name
    {
        get => "Map Movement";
    }

    public override string Description
    {
        get => "Script codeline that executes movement on your character in the map.";
    }
    #endregion

    #region Methods: Override
    public override ScriptCodeline Clone()
    {
        return new ACLMapMovement()
        {
            Parameters = ScriptMachineParser.CloneToParameters(Parameters)
        };
    }

    public override void LoadRegulars()
    {
        Dictionary<string, ScriptParameter> regulars = new()
        {
            ["Movement Type"] = new ScriptOptions("Jump/Join/Walk", "Type of movement to execute")
        };
        Parameters = Parameters.Concat(regulars)
            .ToDictionary(
                kvp => kvp.Key,
                kvp => kvp.Value);
    }

    public override Dictionary<string, ScriptParameter> LoadAdditionals()
    {
        Dictionary<string, ScriptParameter> additionals = Regulars["Movement Type"].String switch
        {
            "Jump" => new()
            {
                ["Cell"] = new ScriptParameter("Enter", "Cell to jump into"),
                ["Pad"] = new ScriptParameter("Spawn", "Pad to jump into")
            },
            "Join" => new()
            {
                ["Map Name"] = new ScriptParameter("battleon", "Map to join in"),
                ["Room Number"] = new ScriptParameter(1, "Room number to join in"),
                ["Cell"] = new ScriptParameter("Enter", "Cell to reside into"),
                ["Pad"] = new ScriptParameter("Spawn", "Pad to reside into")
            },
            "Walk" => new()
            {
                ["X-Coordinate"] = new ScriptParameter(0.1, "X-coordinate to move onto"),
                ["Y-Coordinate"] = new ScriptParameter(0.1, "Y-coordinate to move onto")
            },
            _ => new()
        };
        Parameters = Parameters.Concat(additionals)
            .ToDictionary(
                kvp => ScriptMachineParser.ADDITIONAL_PREFIX + kvp.Key,
                kvp => kvp.Value);

        return Additionals;
    }

    public override async Task Execute(ScriptEngine engine)
    {
        if (!Validate(engine))
        {
            return;
        }

        engine.StateCodelineToBe(ScriptCodelineStatusType.Executing, this);

        switch (Parameters["Movement Type"].String)
        {
            case "Jump":
                string jumpCell = Regulars["Cell"].GetVariable(engine).String;
                string jumpPad = Regulars["Pad"].GetVariable(engine).String;

                engine.FlashAPI.CallGameFunction("world.moveToCell", jumpCell, jumpPad);
                break;
            case "Join":
                string joinMap = Regulars["Map Name"].GetVariable(engine).String;
                string joinRoom = Regulars["Room Number"].GetVariable(engine).String;
                string joinCell = Regulars["Cell"].GetVariable(engine).String;
                string joinPad = Regulars["Pad"].GetVariable(engine).String;

                engine.FlashAPI.CallGameFunction("world.gotoTown", joinMap + "-" + joinRoom, joinCell, joinPad);
                break;
            case "Walk":
                string x = Regulars["X-Coordinate"].GetVariable(engine).String;
                string y = Regulars["Y-Coordinate"].GetVariable(engine).String;

                engine.FlashAPI.SendToServer($"%xt%zm%mv%{engine.World.Area!.ID}%{x}%{y}%8%");
                break;
        }

        engine.StateCodelineToBe(ScriptCodelineStatusType.Succeed, this);
    }

    public override async Task Cancel(ScriptEngine engine)
    {
        // TODO: Implement cancellation logic if needed
        return;
    }

    public override string ToString()
    {
        return Parameters["Movement Type"].String switch
        {
            "Jump" => $"Jump to cell: {Regulars["Cell"]}, Pad: {Regulars["Pad"]}",
            "Join" => $"Join map: {Regulars["Map Name"]}-{Regulars["Room Number"]}, Cell: {Regulars["Cell"]}, Pad: {Regulars["Pad"]}",
            "Walk" => $"Walk to X: {Regulars["X-Coordinate"]}, Y: {Regulars["Y-Coordinate"]}",
            _ => "Invalid movement type"
        };
    }
    #endregion

}
