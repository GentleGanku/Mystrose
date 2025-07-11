using Clipboard = System.Windows.Clipboard;
using Button = Wpf.Ui.Controls.Button;

namespace Mystrose.Views.Interceptor;

public partial class VWPacketInterceptor : MystWindow, IClientSwitcher
{

    #region Constructor
    public VWPacketInterceptor() : base()
    {
        InitializeComponent();

        Loaded += OnLoaded;
        Unloaded += OnUnloaded;
        ContentRendered += OnContentRendered;
    }
    #endregion

    #region (Private) Fields
    private readonly string[] _packetTypes = ["All", "JSON", "XML", "XT", "ZM"];
    private int _searchIndex = -1;
    #endregion

    #region Fields
    public ClientSwitchButton SwitchButton
    {
        get => (ClientSwitchButton)TTLB_Main.AdditionalContent;
    }
    #endregion

    #region Properties
    private string CommandFilter
    {
        get;
        set;
    } = string.Empty;
    #endregion

    #region Methods: Setup
    private void Initialize()
    {
        CB_PacketTypes.Items.Clear();
        foreach (string packetType in _packetTypes)
        {
            CB_PacketTypes.Items.Add(packetType);
        }
        CB_PacketTypes.SelectedIndex = 0;

        TBX_FormattedPacket.Text = "No packet selected yet.";
    }

    private void Destruct()
    {
        CB_PacketTypes.Items.Clear();
        LV_Packets.Items.Clear();
        TBX_FormattedPacket.Text = string.Empty;
        TBX_PacketSender.Clear();
    }
    #endregion

    #region Methods: Utility
    private void RefreshPackets(int type)
    {
        Response<Action> response = Invoke(() =>
        {
            InterceptorMessage[] messages = MSVCInterceptor.Instance.ReadIntercepts(SwitchButton.SelectedCodename).Output;
            type -= 1;

            LV_Packets.Items.Clear();
            foreach (InterceptorMessage message in messages)
            {
                if (type >= 0 && message.Type != type)
                {
                    continue;
                }

                if (!string.IsNullOrEmpty(CommandFilter) && !message.Command.Equals(CommandFilter))
                {
                    continue;
                }

                LV_Packets.Items.Add(message);
            }
            LV_Packets.ScrollIntoView(LV_Packets.Items[LV_Packets.Items.Count - 1]);
        });
    }

    private void RefreshEnlistedPackets(string command)
    {
        if (string.IsNullOrEmpty(command))
        {
            CommandFilter = string.Empty;
            RefreshPackets(CB_PacketTypes.SelectedIndex);
            return;
        }

        Response<Action> response = Invoke(() =>
        {
            InterceptorMessage[] messages = LV_Packets.Items.Cast<InterceptorMessage>().ToArray();

            LV_Packets.Items.Clear();
            foreach (InterceptorMessage message in messages)
            {
                if (!message.Command.Contains(command, StringComparison.OrdinalIgnoreCase))
                {
                    continue;
                }

                LV_Packets.Items.Add(message);
            }
            LV_Packets.ScrollIntoView(LV_Packets.Items[LV_Packets.Items.Count - 1]);
        });

        CommandFilter = command;
    }

    private void SearchKeyword(string keyword)
    {
        if (string.IsNullOrEmpty(keyword))
        {
            return;
        }

        Response<Action> response = Invoke(() =>
        {
            if (!TBX_FormattedPacket.SelectedText.Equals(keyword, StringComparison.OrdinalIgnoreCase))
            {
                _searchIndex = -1;
            }

            int index = TBX_FormattedPacket.Text.IndexOf(keyword, _searchIndex + 1, StringComparison.OrdinalIgnoreCase);
            if (index >= 0)
            {
                _searchIndex = index;
            }
            else
            {
                _searchIndex = -1;
                index = TBX_FormattedPacket.Text.IndexOf(keyword, StringComparison.OrdinalIgnoreCase);
            }

            if (index >= 0)
            {
                TBX_FormattedPacket.Focus();
                TBX_FormattedPacket.Select(index, keyword.Length);
                TBX_FormattedPacket.ScrollToLine(TBX_FormattedPacket.GetLineIndexFromCharacterIndex(index));
            }
        });
    }
    
    private void RefreshFormattedPacket(string formattedPacket = "", int type = 4)
    {
        TBX_FormattedPacket.Text = string.IsNullOrEmpty(formattedPacket) ? "No packet selected yet." : formattedPacket;

        TBX_FormattedPacket.Foreground = type switch
        {
            0 => (SolidColorBrush)FindResource("FG_Message_Json"),
            1 => (SolidColorBrush)FindResource("FG_Message_Xml"),
            2 => (SolidColorBrush)FindResource("FG_Message_Xt"),
            3 => (SolidColorBrush)FindResource("FG_Message_Zm"),
            _ => (SolidColorBrush)FindResource("FG_Message_Neutral")
        };
    }

    private void RefreshOngoingPacket()
    {
        string packetString = TBX_PacketSender.Text;

        TBX_PacketSender.Foreground = packetString switch
        {
            _ when packetString.StartsWith('{') && packetString.EndsWith('}') => (SolidColorBrush)FindResource("FG_Message_Json"),
            _ when packetString.StartsWith('<') && packetString.EndsWith('>') => (SolidColorBrush)FindResource("FG_Message_Xml"),
            _ when packetString.StartsWith("%xt%zm") && packetString.EndsWith('%') => (SolidColorBrush)FindResource("FG_Message_Zm"),
            _ when packetString.StartsWith("%xt") && packetString.EndsWith('%') => (SolidColorBrush)FindResource("FG_Message_Xt"),
            _ => Brushes.Gray
        };
    }
    #endregion

    #region Methods: Actions
    private void CopySelectedPacket()
    {
        if (LV_Packets.SelectedIndex < 0)
        {
            NotifyInfo("Menu Action", "No packet selected.");
            return;
        }

        Response<Action> response = Invoke(() =>
        {
            InterceptorMessage message = (InterceptorMessage)LV_Packets.SelectedItem;
            string packetString = message.Message;

            Clipboard.SetText(packetString);
        });

        if (response.IsSuccess)
        {
            NotifySuccess("Menu Action", "Successfully copied selected packet.");
        }
        else
        {
            NotifyFailure("Menu Action", "Failed to copy selected packet.");
        }
    }

    private void CopyFormattedPacket()
    {
        if (string.IsNullOrEmpty(TBX_FormattedPacket.Text))
        {
            NotifyInfo("Menu Action", "No formatted packet found to copy.");
            return;
        }

        Response<Action> response = Invoke(() =>
        {
            Clipboard.SetText(TBX_FormattedPacket.Text);
        });

        if (response.IsSuccess)
        {
            NotifySuccess("Menu Action", "Successfully copied formatted packet.");
        }
        else
        {
            NotifyFailure("Menu Action", "Failed to copy formatted packet.");
        }
    }

    private void ClearPackets()
    {
        if (LV_Packets.Items.Count <= 0)
        {
            NotifyInfo("Menu Action", "No packets found to clear.");
            return;
        }

        Response<Action> response = Invoke(() =>
        {
            MSVCInterceptor.Instance.ClearIntercepts(SwitchButton.SelectedCodename);

            LV_Packets.Items.Clear();
            RefreshFormattedPacket();
        });

        if (response.IsSuccess)
        {
            NotifySuccess("Menu Action", "Successfully cleared packets.");
        }
        else
        {
            NotifyFailure("Menu Action", "Failed to clear packets.");
        }
    }

    private void FormatPacket()
    {
        if (LV_Packets.SelectedIndex < 0)
        {
            return;
        }

        Response<Action> response = Invoke(() =>
        {
            InterceptorMessage message = (InterceptorMessage)LV_Packets.SelectedItem;
            string packetString = message.Type switch
            {
                0 => MSVCInterceptor.Instance.FormatInJson(message).Output,
                1 => MSVCInterceptor.Instance.FormatInXml(message).Output,
                2 => MSVCInterceptor.Instance.FormatInXt(message).Output,
                3 => MSVCInterceptor.Instance.FormatInZm(message).Output,
                _ => string.Empty
            };

            RefreshFormattedPacket(packetString, message.Type);
        });
    }

    private void SendPacketToServer()
    {
        if (string.IsNullOrEmpty(TBX_PacketSender.Text))
        {
            NotifyInfo("Menu Action", "No packet found to send.");
            return;
        }

        Response<Action> response = Invoke(() =>
        {
            HSTGame game = MSVCGame.Instance[SwitchButton.SelectedCodename].Item2!;

            string packetString = TBX_PacketSender.Text;

            game.FlashAPI.SendToServer(packetString);
        });

        if (response.IsSuccess)
        {
            NotifySuccess("Menu Action", "Successfully sent packet to the server.");
        }
        else
        {
            NotifyFailure("Menu Action", "Failed to send packet.");
        }
    }

    private void SendPacketToClient()
    {
        if (string.IsNullOrEmpty(TBX_PacketSender.Text))
        {
            NotifyInfo("Menu Action", "No packet found to send.");
            return;
        }

        Response<Action> response = Invoke(() =>
        {
            HSTGame game = MSVCGame.Instance[SwitchButton.SelectedCodename].Item2!;

            string packetString = TBX_PacketSender.Text;

            game.FlashAPI.SendToClient(packetString);
        });

        if (response.IsSuccess)
        {
            NotifySuccess("Menu Action", "Successfully sent packet to the client.");
        }
        else
        {
            NotifyFailure("Menu Action", "Failed to send packet.");
        }
    }

    private void ClearOngoingPacket()
    {
        if (string.IsNullOrEmpty(TBX_PacketSender.Text))
        {
            NotifyInfo("Menu Action", "Ongoing packet is already empty.");
            return;
        }

        Response<Action> response = Invoke(() =>
        {
            TBX_PacketSender.Clear();
        });

        if (response.IsSuccess)
        {
            NotifySuccess("Menu Action", "Successfully cleared ongoing packet.");
        }
        else
        {
            NotifyFailure("Menu Action", "Failed to clear ongoing packet.");
        }
    }
    #endregion

    #region Methods: Service Handlers
    public void AppendPacket(string codename, InterceptorMessage message)
    {
        if (!codename.Equals(SwitchButton.SelectedCodename))
        {
            return;
        }

        Response<Action> response = Invoke(() =>
        {
            int type = CB_PacketTypes.SelectedIndex - 1;
            if (type >= 0 && message.Type != type)
            {
                return;
            }

            if (!string.IsNullOrEmpty(CommandFilter) && !message.Command.Equals(CommandFilter))
            {
                return;
            }

            LV_Packets.Items.Add(message);
            LV_Packets.ScrollIntoView(message);
        });
    }
    
    public void DeactivateInstance(string codename, object? args)
    {
        if (!codename.Equals(SwitchButton.SelectedCodename))
        {
            return;
        }

        Response<Action> response = Invoke(() =>
        {
            SwitchButton.RemoveInstance(codename);
        });
    }
    #endregion

    #region Events: Read/Write
    private void OnLoaded(object sender, RoutedEventArgs e)
    {
        MSVCInterceptor.Instance.InterceptEvent += AppendPacket;
        MSVCGame.Instance.DeactivatedGameEvent += DeactivateInstance;
        
        SwitchButton.Item.SelectionChanged += SwitchButton_SelectionChanged;

        MBTN_CopySelected.Button.Click += MenuButton_Click;
        MBTN_CopyFormatted.Button.Click += MenuButton_Click;
        MBTN_ClearCaptured.Button.Click += MenuButton_Click;

        MBTN_FindKeyword.Button.Click += MenuButton_Click;

        MBTN_SendToClient.Button.Click += MenuButton_Click;
        MBTN_SendToServer.Button.Click += MenuButton_Click;
        MBTN_ClearOngoing.Button.Click += MenuButton_Click;
    }

    private void OnUnloaded(object sender, RoutedEventArgs e)
    {
        MSVCInterceptor.Instance.InterceptEvent -= AppendPacket;
        MSVCGame.Instance.DeactivatedGameEvent -= DeactivateInstance;

        SwitchButton.Item.SelectionChanged -= SwitchButton_SelectionChanged;
        
        MBTN_CopySelected.Button.Click -= MenuButton_Click;
        MBTN_CopyFormatted.Button.Click -= MenuButton_Click;
        MBTN_ClearCaptured.Button.Click -= MenuButton_Click;

        MBTN_FindKeyword.Button.Click -= MenuButton_Click;

        MBTN_SendToClient.Button.Click -= MenuButton_Click;
        MBTN_SendToServer.Button.Click -= MenuButton_Click;
        MBTN_ClearOngoing.Button.Click -= MenuButton_Click;

        Destruct();
    }

    private void OnContentRendered(object? sender, EventArgs e)
    {
        Initialize();
    }
    #endregion

    #region Events: Interface
    public void SwitchButton_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        RefreshPackets(CB_PacketTypes.SelectedIndex);
    }

    private void PacketTypes_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        RefreshPackets(CB_PacketTypes.SelectedIndex);
    }

    private void Packets_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        FormatPacket();
    }

    private void InterceptToggle_CheckChanged(object sender, RoutedEventArgs e)
    {
        MSVCInterceptor.Instance.SetIntercepting((bool)TSBTN_Intercept.IsChecked!);
    }

    private void MenuButton_Click(object sender, RoutedEventArgs e)
    {
        MenuButton button = ((sender as Button)!.Parent as MenuButton)!;

        switch (button.Name)
        {
            case "MBTN_CopySelected":
                CopySelectedPacket();
                break;

            case "MBTN_CopyFormatted":
                CopyFormattedPacket();
                break;

            case "MBTN_ClearCaptured":
                ClearPackets();
                break;

            case "MBTN_FindKeyword":
                SearchKeyword(TBX_FindKeyword.Text);
                break;

            case "MBTN_SendToClient":
                SendPacketToClient();
                break;

            case "MBTN_SendToServer":
                SendPacketToServer();
                break;

            case "MBTN_ClearOngoing":
                ClearOngoingPacket();
                break;
        }
    }

    private void TBX_FilterCommand_TextChanged(object sender, TextChangedEventArgs e)
    {
        RefreshEnlistedPackets(TBX_FilterCommand.Text);
    }

    private void TBX_PacketSender_TextChanged(object sender, TextChangedEventArgs e)
    {
        RefreshOngoingPacket();
    }
    #endregion

}
