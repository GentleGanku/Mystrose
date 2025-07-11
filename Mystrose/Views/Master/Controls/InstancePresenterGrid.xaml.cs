using UserControl = System.Windows.Controls.UserControl;

namespace Mystrose.Views.Master.Controls;

public partial class InstancePresenterGrid : UserControl
{

    #region Constructor
    public InstancePresenterGrid()
    {
        InitializeComponent();
    }
    #endregion

    #region Fields
    public MystWindow ParentWindow
    {
        get => (MystWindow)Window.GetWindow(this);
    }

    public List<InstanceButton> Instances
    {
        get => GRD_Instances.Children.OfType<InstanceButton>().ToList();
    }
    #endregion

    #region Properties
    private InstanceButton? SelectedInstance
    {
        get;
        set;
    }
    #endregion

    #region Methods: Utility
    private void SortButtons()
    {
        int count = 0;

        foreach (string codename in MSVCGame.Instance.ActiveCollection.Keys)
        {
            InstanceButton? instanceButton = Instances.Find(i => i.NameText.Equals(codename));

            if (instanceButton is null)
            {
                continue;
            }

            instanceButton.SetValue(Grid.ColumnProperty, count);
            count++;
        }
    }
    #endregion

    #region Methods: Actions
    public void AddInstance(string codename)
    {
        InstanceButton? instanceButton = Instances.Find(i => i.NameText.Equals(codename));
        if (instanceButton is not null)
        {
            HSVCLogger.Instance.LogOnTrace($"Instance '{codename}' already exists.");
            return;
        }

        Response<Action> response = ParentWindow.Invoke(() =>
        {
            InstanceButton newButton = new()
            {
                NameText = codename
            };
            newButton.SetValue(Grid.ColumnProperty, GRD_Instances.Children.Count);

            GRD_Instances.Children.Add(newButton);

            SortButtons();

            SelectInstance(codename);
        });
    }

    public void RemoveInstance(string codename)
    {
        InstanceButton? instanceButton = Instances.Find(i => i.NameText.Equals(codename));
        if (instanceButton is null)
        {
            HSVCLogger.Instance.LogOnTrace($"Instance '{codename}' does not exist.");
            return;
        }

        Response<Action> response = ParentWindow.Invoke(() =>
        {
            int indexToRemove = GRD_Instances.Children.IndexOf(instanceButton);
            GRD_Instances.Children.Remove(instanceButton);
            SortButtons();

            InstanceButton? selectedButton = Instances.Find(i => i.Button.Appearance is ControlAppearance.Primary);
            if (selectedButton is not null)
            {
                return;
            }

            if (GRD_Instances.Children.Count > 0)
            {
                if (indexToRemove == GRD_Instances.Children.Count)
                {
                    MSVCGame.Instance.Select(Instances[indexToRemove - 1].NameText);
                }
                else
                {
                    MSVCGame.Instance.Select(Instances[indexToRemove].NameText);
                }
            }
            else
            {
                MSVCGame.Instance.Deselect();
            }
        });
    }

    public void SelectInstance(string codename)
    {
        if (Instances.Count <= 0)
        {
            HSVCLogger.Instance.LogOnTrace($"No instances currently exist.");
            return;
        }

        Response<Action> response = ParentWindow.Invoke(() =>
        {
            if (SelectedInstance is not null)
            {
                SelectedInstance.Button.Appearance = ControlAppearance.Transparent;
            }

            if (string.IsNullOrEmpty(codename))
            {
                SelectedInstance = null;
                return;
            }

            InstanceButton? instanceButton = Instances.Find(i => i.NameText.Equals(codename));
            instanceButton!.Button.Appearance = ControlAppearance.Primary;

            SelectedInstance = instanceButton;
        });
    }
    #endregion

    #region Methods: Event Handlers
    private void AddIncomingInstance(string codename, HSTGame? game)
    {
        AddInstance(codename);
    }

    private void RemoveIncomingInstance(string codename, HSTGame? game)
    {
        RemoveInstance(codename);

        MSVCScript.Instance.Deactivate(codename);
        MSVCInterceptor.Instance.ActiveCollection[codename] = [];
        MSVCVisualizer.Instance.RefreshGameRecord(codename);
        MSVCWorld.Instance.Deactivate(codename);
    }

    private void SelectIncomingInstance(string codename, HSTGame? game)
    {
        SelectInstance(codename);
    }
    #endregion

    #region Handlers: Events
    private void OnLoaded(object sender, RoutedEventArgs e)
    {
        MSVCGame.Instance.ActivatedGameEvent += AddIncomingInstance;
        MSVCGame.Instance.DeactivatedGameEvent += RemoveIncomingInstance;
        MSVCGame.Instance.SelectedGameEvent += SelectIncomingInstance;
        
        MSVCGame.Instance.Construct();
    }

    private void OnUnloaded(object sender, RoutedEventArgs e)
    {
        MSVCGame.Instance.ActivatedGameEvent -= AddIncomingInstance;
        MSVCGame.Instance.DeactivatedGameEvent -= RemoveIncomingInstance;
        MSVCGame.Instance.SelectedGameEvent -= SelectIncomingInstance;

        Loaded -= OnLoaded;
        Unloaded -= OnUnloaded;
    }
    #endregion

}
