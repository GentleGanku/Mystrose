namespace Mystrose.Core.ScriptMachine.Codelines.Others;

public class SCLFiller : ScriptCodeline
{

    #region Properties: Attributes
    public override string ID
    {
        get => "SCL.101";
    }

    public override ScriptCodelineType Type
    {
        get => ScriptCodelineType.Filler;
    }

    public override string Name
    {
        get => "Filler";
    }

    public override string Description
    {
        get => "Script codeline that displays a comment. It executes nothing.";
    }
    #endregion

    #region Methods: Override
    public override ScriptCodeline Clone()
    {
        return new SCLFiller()
        {
            Parameters = ScriptMachineParser.CloneToParameters(Parameters)
        };
    }

    public override void LoadRegulars()
    {
        Innates["Label"].Set("Filler-coded");

        Dictionary<string, ScriptParameter> regulars = new()
        {
            ["Comment Text"] = new ScriptParameter("Hello, World!", "Text to be displayed as comment")
        };
        Parameters = Parameters.Concat(regulars)
            .ToDictionary(
                kvp => kvp.Key, 
                kvp => kvp.Value);
    }

    public override Dictionary<string, ScriptParameter> LoadAdditionals()
    {
        return [];
    }

    public override async Task Execute(ScriptEngine engine)
    {
        if (Innates["Is Enabled"].Boolean is false)
        {
            return;
        }

        engine.StateCodelineToBe(ScriptCodelineStatusType.Succeed, this);
    }

    public override async Task Cancel(ScriptEngine engine)
    {
        return;
    }

    public override string ToString()
    {
        return Parameters["Comment Text"].String.Length > 0 ? ("// " + Parameters["Comment Text"]) : "";
    }
    #endregion

}

