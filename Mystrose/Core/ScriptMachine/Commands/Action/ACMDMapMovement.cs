namespace Mystrose.Core.ScriptMachine.Commands.Action;

public class ACMDMapMovement : SCMDAction
{

    #region Constructor
    public ACMDMapMovement() : base(ScriptCommandType.Action, "ACMD08", "Map Movement", "A script command that executes character movement on a map.")
    {
        Parameters = new()
        {
            ["Movement Type"] = new ScriptOptions("Jump / Join / Walk", "The type of movement to execute")
        };
        SecondaryParameters = [];
    }
    #endregion

    #region Methods: Override
    public override ScriptCommand Clone()
    {
        return new ACMDMapMovement()
        {
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

        SecondaryParameters.Clear();
        SecondaryParameters[key] = key switch
        {
            "Jump" => new()
            {
                ["Cell"] = new ScriptParameter("Enter", "The cell to jump to"),
                ["Pad"] = new ScriptParameter("Spawn", "The pad to jump to")
            },
            "Join" => new()
            {
                ["Map Name"] = new ScriptParameter("battleon", "The map to join"),
                ["Room Number"] = new ScriptParameter(1, "The room number to join"),
                ["Cell"] = new ScriptParameter("Enter", "The cell to reside to"),
                ["Pad"] = new ScriptParameter("Spawn", "The pad to reside to")
            },
            "Walk" => new()
            {
                ["X-Coordinate"] = new ScriptParameter(0.1, "The X-coordinate to move to"),
                ["Y-Coordinate"] = new ScriptParameter(0.1, "The Y-coordinate to move to")
            }
        };

        return SecondaryParameters[key];
    }

    public override async Task Execute(ScriptEngine engine)
    {
        switch (Parameters["Movement Type"].String)
        {
            case "Jump":
                string jumpCell = SecondaryParameters["Jump"]["Cell"].GetVar(engine).String;
                string jumpPad = SecondaryParameters["Jump"]["Pad"].GetVar(engine).String;

                engine.Flash.CallGameFunction("world.moveToCell", jumpCell, jumpPad);
                break;
            case "Join":
                string joinMap = SecondaryParameters["Join"]["Map Name"].GetVar(engine).String;
                string joinRoom = SecondaryParameters["Join"]["Room Number"].GetVar(engine).ToString()!;
                string joinCell = SecondaryParameters["Join"]["Cell"].GetVar(engine).String;
                string joinPad = SecondaryParameters["Join"]["Pad"].GetVar(engine).String;

                engine.Flash.CallGameFunction("world.gotoTown", joinMap + "-" + joinRoom, joinCell, joinPad);
                break;
            case "Walk":
                string x = SecondaryParameters["Walk"]["X-Coordinate"].GetVar(engine).ToString()!;
                string y = SecondaryParameters["Walk"]["Y-Coordinate"].GetVar(engine).ToString()!;

                engine.Flash.SendPacket($"%xt%zm%mv%{engine.World.Area!.ID}%{x}%{y}%8%");
                break;
        }

        EndResult = ScriptResultType.Success;
    }

    public override string ToString()
    {
        return Parameters["Movement Type"].String switch
        {
            "Jump" => $"Jump to Cell: {SecondaryParameters["Jump"]["Cell"]}, Pad: {SecondaryParameters["Jump"]["Pad"]}",
            "Join" => $"Join Map: {SecondaryParameters["Join"]["Map Name"]}-{SecondaryParameters["Join"]["Room Number"]}, Cell: {SecondaryParameters["Join"]["Cell"]}, Pad: {SecondaryParameters["Join"]["Pad"]}",
            "Walk" => $"Walk to X: {SecondaryParameters["Walk"]["X-Coordinate"]}, Y: {SecondaryParameters["Walk"]["Y-Coordinate"]}",
            _ => "Invalid Movement Type"
        };
    }
    #endregion

}
