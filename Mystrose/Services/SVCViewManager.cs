namespace Mystrose.Services;

public class SVCViewManager
{

    #region Delegates & Handlers
    public delegate void ViewHandler(MystWindow view);
    public static event ViewHandler RenderedViewEvent;
    public static event ViewHandler UnrenderedViewEvent;
    #endregion

    #region Fields
    private static readonly Dictionary<string, MystWindow> _views = [];
    #endregion

    #region Methods: Render/Unrender
    public static Response<MystWindow?> Open(Type type)
    {
        if (_views.TryGetValue(type.Name, out MystWindow? existingView))
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

        SVCLogger.LogOnConsole($"Opening {type.Name} View...", "SVCViewManager", "Open");

        view.Show();

        return new(true,
            "View opened.",
            view);
    }

    public static Response<MystWindow?> Render(MystWindow view)
    {
        string viewType = view.GetType().Name;

        if (_views.TryGetValue(viewType, out MystWindow? existingView))
        {
            return new(false, 
                "View already rendered.", 
                existingView);
        }

        view.ContentRendered += (sender, e) =>
        {
            view.UpdateLayout();
            view.Activate();

            _views.Add(viewType, view);

            RenderedViewEvent?.Invoke(view);

            SVCLogger.LogOnConsole($"{viewType} View rendered.", "SVCViewManager", "Render");
        };

        return new(true, 
            "Rendering the view.", 
            view);
    }

    public static Response<MystWindow?> Unrender(MystWindow view)
    {
        string viewType = view.GetType().Name;

        if (!_views.TryGetValue(viewType, out MystWindow? existingView))
        {
            return new(false,
                "View not rendered.",
                null);
        }

        _views.Remove(viewType);

        UnrenderedViewEvent?.Invoke(view);

        SVCLogger.LogOnConsole($"{viewType} View unrendered.", "SVCViewManager", "Unrender");

        return new(true,
            "View unrendered.",
            view);
    }

    public static Response<MystWindow?> UnrenderAll()
    {
        if (_views.Count <= 0)
        {
            return new(false,
                "No views rendered.",
                null);
        }

        foreach (var view in _views.Reverse())
        {
            if (view.Key is nameof(VWMaster))
            {
                continue;
            }

            view.Value.Close();
        }

        return new(true,
            "All views unrendered.",
            null);
    }
    #endregion

}
