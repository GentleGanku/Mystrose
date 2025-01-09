using UserControl = System.Windows.Controls.UserControl;

namespace Mystrose.Views.Base.Controls;

public partial class AttributeItem : UserControl
{

    #region Constructor
    public AttributeItem()
    {
        InitializeComponent();
        DataContext = this;
    }

    public AttributeItem(string name, object value)
    {
        InitializeComponent();
        DataContext = this;

        AttributeName = name;
        AttributeValue = value.ToString()!;
    }
    #endregion

    #region (Private) Fields
    private string _attributeName;
    private string _attributeValue;
    #endregion

    #region Properties
    public string AttributeName
    {
        get => _attributeName;
        set
        {
            _attributeName = value;
            LBL_Name.Content = value;
        }
    }

    public string AttributeValue
    {
        get => _attributeValue;
        set
        {
            _attributeValue = value;
            TBX_Value.Text = value;
        }
    }
    #endregion

}
