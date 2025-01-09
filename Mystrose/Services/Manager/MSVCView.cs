namespace Mystrose.Services.Manager;

public class MSVCView() : ManagerService<MystWindow>(nameof(MSVCView))
{

    #region Delegates & Handlers
    public delegate void ViewHandler(MystWindow view);
    public event ViewHandler RenderedViewEvent;
    public event ViewHandler UnrenderedViewEvent;
    #endregion

    #region (Static) Fields
    public static MSVCView Instance
    {
        get
        {
            if (_instance is null)
            {
                _instance = new MSVCView();
                _instance.Construct();
            }
            
            return _instance;
        }
    }
    #endregion

    #region (Private) Fields
    private static MSVCView? _instance;
    #endregion

    #region Methods: Builder
    public override void Construct()
    {
        try
        {
            Items.Clear();

            Log("View Manager constructed.", "Construct");
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
            Items.Clear();

            Log("View Manager deconstructed.", "Deconstruct");
        }
        catch (Exception ex)
        {
            Log(ex.ToString(), "Deconstruct");
        }
    }
    #endregion

    #region Methods: Render/Unrender
    public Response<MystWindow?> Open(Type type)
    {
        if (Items.TryGetValue(type.Name, out MystWindow? existingView))
        {
            if (existingView.WindowState is WindowState.Minimized)
            {
                existingView.WindowState = WindowState.Normal;
            }

            existingView.Focus();

            return new(false,
                "View already opened.",
                existingView);
        }

        MystWindow view = (MystWindow)Activator.CreateInstance(type)!;

        HSVCLogger.Instance.LogOnConsole($"Opening {type.Name} View...", "SVCViewManager", "Open");

        view.Show();

        return new(true,
            "View opened.",
            view);
    }

    public Response<MystWindow?> OpenForInstances(Type type)
    {
        if (MSVCGame.Instance.ActiveCollection.Count == 0)
        {
            return new(true,
                "No instances are currently running.",
                null);
        }

        if (Items.TryGetValue(type.Name, out MystWindow? existingView))
        {
            if (existingView!.WindowState is WindowState.Minimized)
            {
                existingView.WindowState = WindowState.Normal;
            }

            existingView.Focus();

            return new(false,
                "View already opened.",
                existingView);
        }

        MystWindow view = (MystWindow)Activator.CreateInstance(type)!;

        HSVCLogger.Instance.LogOnConsole($"Opening {type.Name} View...", "SVCViewManager", "Open");

        view.Show();

        return new(true,
            "View opened.",
            view);
    }

    public Response<MystWindow?> Render(MystWindow view)
    {
        string viewType = view.GetType().Name;

        if (Items.TryGetValue(viewType, out MystWindow? existingView))
        {
            return new(false, 
                "View already rendered.", 
                existingView);
        }

        view.ContentRendered += (sender, e) =>
        {
            view.UpdateLayout();
            view.Activate();

            Items.Add(viewType, view);

            RenderedViewEvent?.Invoke(view);

            HSVCLogger.Instance.LogOnConsole($"{viewType} View rendered.", "SVCViewManager", "Render");
        };

        return new(true, 
            "Rendering the view.", 
            view);
    }

    public Response<MystWindow?> Unrender(MystWindow view)
    {
        string viewType = view.GetType().Name;

        if (!Items.TryGetValue(viewType, out MystWindow? existingView))
        {
            return new(false,
                "View not rendered.",
                null);
        }

        Items.Remove(viewType);

        UnrenderedViewEvent?.Invoke(view);

        HSVCLogger.Instance.LogOnConsole($"{viewType} View unrendered.", "SVCViewManager", "Unrender");

        return new(true,
            "View unrendered.",
            view);
    }

    public Response<MystWindow?> UnrenderAll()
    {
        if (Items.Count <= 0)
        {
            return new(false,
                "No views rendered.",
                null);
        }

        foreach (var view in Items.Reverse())
        {
            if (view.Key is nameof(VWMaster))
            {
                continue;
            }

            view.Value!.Close();
        }

        return new(true,
            "All views unrendered.",
            null);
    }
    #endregion

}
