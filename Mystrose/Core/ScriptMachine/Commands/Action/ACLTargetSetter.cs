namespace Mystrose.Core.ScriptMachine.Codelines.Action;

public class ACLTargetSetter : SCLAction
{

    #region Properties: Attributes
    public override string ID
    {
        get => "SCL.002";
    }

    public override ScriptCodelineType Type
    {
        get => ScriptCodelineType.Action;
    }

    public override string Name
    {
        get => "Target Setter";
    }

    public override string Description
    {
        get => "Script codeline that sets the main target based on specific criterias.";
    }
    #endregion

    #region Methods: Override
    public override ScriptCodeline Clone()
    {
        return new ACLTargetSetter()
        {
            Parameters = ScriptMachineParser.CloneToParameters(Parameters)
        };
    }

    public override void LoadRegulars()
    {
        Dictionary<string, ScriptParameter> regulars = new()
        {
            ["Target Type"] = new ScriptOptions("Self/Player/Monster/Random/No Target", "Type of primary target to be set on")
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
            "Self" => [],
            "Player" => new()
            {
                ["Player Name"] = new ScriptParameter("", "Name of the player to be set as the target")
            },
            "Monster" => new()
            {
                ["Tag"] = new ScriptParameter("", "Either name of the monster (random) or the Monster Map ID of it (specific) to be set as the target")
            },
            "Random" => new()
            {
                ["Focus Type"] = new ScriptOptions("Player/Monster", "Type of target to be focused on")
            },
            "No Target" => [],
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

        bool isSuccess = true;

        switch (Parameters["Target Type"].String)
        {
            case "Self":
                engine.FlashAPI.CallGameFunctionOnFunc("world.setTarget", "world.getAvatarByUserName", engine.Player.Name);
                break;
            case "Player":
                engine.FlashAPI.CallGameFunctionOnFunc("world.setTarget", "world.getAvatarByUserName", Additionals["Player Name"].GetVariable(engine).String);
                break;
            case "Monster":
                if (Additionals["Tag"].ValueType == ScriptValueType.String)
                {
                    Random monRandom = new();
                    int monIndex = 0;

                    Monster[] monsters = [.. engine.Map.Monsters.FindAll(
                        (m) =>
                        {
                            MonsterFormat? monsterFormat = engine.Map.Format.MonsterFormats.Find(mf => mf.Name.Equals(Additionals["Tag"].GetVariable(engine).String, StringComparison.OrdinalIgnoreCase));

                            if (monsterFormat is null)
                            {
                                return false;
                            }

                            return monsterFormat.ID == m.ID && m.Cell == engine.Player.Cell;
                        })];

                    if (monsters.Length == 0)
                    {
                        isSuccess = false;
                        break;
                    }

                    monIndex = monRandom.Next(0, monsters.Length);
                    engine.FlashAPI.CallGameFunctionOnFunc("world.setTarget", "world.getMonster", monsters[monIndex].MonMapID);
                }
                else if (Additionals["Tag"].ValueType == ScriptValueType.Integer)
                {
                    engine.FlashAPI.CallGameFunctionOnFunc("world.setTarget", "world.getMonster", Additionals["Tag"].GetVariable(engine).Integer);
                }
                break;
            case "Random":
                Random allRandom = new();
                int allIndex = 0;

                if (Additionals["Focus Type"].String == "Player")
                {
                    Avatar[] allPlayers = [.. engine.Map.Players.FindAll(
                        (p) =>
                        {
                            return p.Name != engine.Player.Name && p.Cell == engine.Player.Cell;
                        })];

                    if (allPlayers.Length == 0)
                    {
                        isSuccess = false;
                        break;
                    }

                    allIndex = allRandom.Next(0, allPlayers.Length);
                    engine.FlashAPI.CallGameFunctionOnFunc("world.setTarget", "world.getAvatarByUserName", allPlayers[allIndex].Name);
                }
                else if (Additionals["Focus Type"].String == "Monster")
                {
                    Monster[] allMonsters = [.. engine.Map.Monsters.FindAll(
                        (m) =>
                        {
                            return m.Cell == engine.Player.Cell;
                        })];

                    if (allMonsters.Length == 0)
                    {
                        isSuccess = false;
                        break;
                    }

                    allIndex = allRandom.Next(0, allMonsters.Length);
                    engine.FlashAPI.CallGameFunctionOnFunc("world.setTarget", "world.getMonster", allMonsters[allIndex].MonMapID);
                }
                break;
            case "No Target":
                engine.FlashAPI.CallGameFunctionOnFunc("world.setTarget", "world.getMonster", "");
                break;
        }

        engine.StateCodelineToBe(
            isSuccess ? ScriptCodelineStatusType.Succeed : ScriptCodelineStatusType.Failed, 
            this);
    }

    public override async Task Cancel(ScriptEngine engine)
    {
        // TODO: Implement cancellation logic if needed
        return;
    }

    public override string ToString()
    {
        return Parameters["Target Type"].String switch
        {
            "Self" => "Set target to Self",
            "Player" => "Set target to Player '" + Additionals["Player Name"] + "'",
            "Monster" => "Set target to Monster '" + Additionals["Tag"] + "'",
            "Random" => $"Set target to Random ({Additionals["Focus Type"]})",
            "No Target" => "Set target to None"
        };
    }
    #endregion

}
