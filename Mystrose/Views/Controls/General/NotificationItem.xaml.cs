using UserControl = System.Windows.Controls.UserControl;

namespace Mystrose.Views.Controls.General;

/// <summary>
/// An User Control that represents a notification in the client.
/// </summary>
public partial class NotificationItem : UserControl
{

    #region Constructor
    public NotificationItem()
    {
        InitializeComponent();

        DataContext = this;
    }
    #endregion

    #region Private Variables
    private bool _isMarked;
    private string _source;
    private string _timestamp;
    private object _icon;
    private string _message;
    #endregion

    #region Properties
    /// <summary>
    /// The condition of whether the notification is marked as read or not.
    /// </summary>
    public bool IsMarked
    {
        get
        {
            return _isMarked;
        }
        set
        {
            _isMarked = value;
            UnreadIcon.Visibility = _isMarked ? Visibility.Hidden : Visibility.Visible;
        }
    }

    /// <summary>
    /// The source of the notification.
    /// </summary>
    public string Source
    {
        get
        {
            return _source;
        }
        set
        {
            _source = value;
            SourceBlock.Text = SourceText;
        }
    }

    /// <summary>
    /// The timestamp of the notification.
    /// </summary>
    public string Timestamp
    {
        get
        {
            return _timestamp;
        }
        set
        {
            _timestamp = value;
            SourceBlock.Text = SourceText;
        }
    }

    /// <summary>
    /// The icon of the notification.
    /// </summary>
    public object Icon
    {
        get
        {
            return _icon;
        }
        set
        {
            _icon = value;
        }
    }

    /// <summary>
    /// The message of the notification.
    /// </summary>
    public string Message
    {
        get
        {
            return _message;
        }
        set
        {
            _message = value;
            MessageBlock.Text = _message;
        }
    }

    /// <summary>
    /// The source text (source and timestamp) of the notification.
    /// </summary>
    public string SourceText
    {
        get
        {
            return Source != null ? (Timestamp + "  ●  " + Source) : Timestamp;
        }
    }
    #endregion

}
