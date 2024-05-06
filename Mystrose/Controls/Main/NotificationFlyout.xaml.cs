using Mystrose.Controls.General;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using Wpf.Ui.Controls;

namespace Mystrose.Controls.Main;

/// <summary>
/// Interaction logic for NotificationFlyout.xaml
/// </summary>
public partial class NotificationFlyout : UserControl
{
    #region Constructor
    public NotificationFlyout()
    {
        DataContext = this;
        NotificationLists = new Dictionary<int, List<NotificationItem>>();
        UnreadCount = 0;

        InitializeComponent();

        Loaded += OnLoaded;
    }
    #endregion

    #region Destructor
    ~NotificationFlyout()
    {
        Loaded -= OnLoaded;
    }
    #endregion

    #region Properties
    public Dictionary<int, List<NotificationItem>> NotificationLists
    {
        get;
        set;
    }

    public List<NotificationItem> SelectedList
    {
        get;
        set;
    }

    public int UnreadCount
    {
        get;
        set;
    }

    public Flyout Parent
    {
        get;
        set;
    }

    public SymbolIcon NoticeIcon
    {
        get;
        set;
    }

    public bool Initialized
    {
        get;
        set;
    }
    #endregion

    #region Methods - Setter
    private void InitializeProperties()
    {
        NoticeIcon.Visibility = Visibility.Collapsed;

        Parent.Closed += OnClosed;
        GroupBox.SelectionChanged += OnGroupChanged;

        SetGroup(0);

        Initialized = true;
    }
    #endregion

    #region Methods - Control
    public void Open()
    {   
        if (NoticeIcon.Visibility == Visibility.Visible)
        {
            NoticeIcon.Visibility = Visibility.Collapsed;
        }

        Parent.IsOpen = true;
    }

    public void Close()
    {
        Parent.IsOpen = false;
    }

    public void MarkAll()
    {
        foreach (NotificationItem notif in SelectedList)
        {
            if (!notif.IsMarked)
            {
                notif.IsMarked = true;
                UnreadCount--;
            }
        }
        SetUnreadText();
    }

    public void AddItem(int groupIndex, string source, object icon, string message)
    {
        if (NotificationLists.Count == groupIndex)
        {
            AddGroup(groupIndex);
        }

        switch (NotificationLists[groupIndex].Count)
        {
            case 0:
                if (groupIndex == GroupBox.SelectedIndex)
                {
                    EmptyNotifsTxt.Visibility = Visibility.Collapsed;
                    NotifsList.Visibility = Visibility.Visible;
                }
                break;
            case 30:
                NotificationLists[groupIndex].Remove(NotificationLists[groupIndex].Last());
                break;
        } 

        NotificationItem notification = new NotificationItem()
        {
            IsMarked = false,
            Source = source,
            Timestamp = DateTime.Now.ToString("hh:mm:ss tt"),
            Icon = icon,
            Message = message
        };

        NotificationLists[groupIndex].Add(notification);
        UnreadCount++;

        SetUnreadText();

        if (groupIndex == GroupBox.SelectedIndex)
        {
            NotifsList.Items.Insert(0, notification);
        }
        if (!Parent.IsOpen)
        {
            NoticeIcon.Visibility = Visibility.Visible;
        }
    }

    public void AddGroup(int groupIndex)
    {
        ComboBoxItem item = new ComboBoxItem()
        {
            Content = "No. " + (groupIndex + 1)
        };

        GroupBox.Items.Insert(groupIndex, item);
        NotificationLists.Add(groupIndex, new List<NotificationItem>());
    }

    public void SetGroup(int groupIndex)
    {
        if (SelectedList != null)
        {
            MarkAll();
        }

        GroupBox.SelectedIndex = groupIndex;
        SelectedList = NotificationLists[groupIndex];

        NotifsList.Items.Clear();
        foreach (NotificationItem item in NotificationLists[groupIndex])
        {
            NotifsList.Items.Insert(0, item);
        }

        if (NotificationLists[groupIndex].Count == 0)
        {
            EmptyNotifsTxt.Visibility = Visibility.Visible;
            NotifsList.Visibility = Visibility.Collapsed;
        }
        else
        {
            EmptyNotifsTxt.Visibility = Visibility.Collapsed;
            NotifsList.Visibility = Visibility.Visible;
        }

        if (NotificationLists[groupIndex].Where((item) => !item.IsMarked).Count() > 0 && !Parent.IsOpen)
        {
            NoticeIcon.Visibility = Visibility.Visible;
        }
    }

    public void SetUnreadText()
    {
        if (UnreadCount > 0)
        {
            UnreadBorder.Visibility = Visibility.Visible;
        }
        else
        {
            UnreadBorder.Visibility= Visibility.Collapsed;
        }
        UnreadTxt.Text = UnreadCount + " unread message(s)";
    }
    #endregion

    #region Methods - Event Handlers
    private void OnLoaded(object sender, RoutedEventArgs e)
    {
        if (!Initialized)
        {
            InitializeProperties();
        }
    }

    private void OnClosed(object sender, RoutedEventArgs e)
    {
        Close();
        MarkAll();
    }

    private void OnGroupChanged(object sender, RoutedEventArgs e)
    {
        SetGroup(GroupBox.SelectedIndex);
    }
    #endregion

}
