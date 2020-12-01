using ChatClient.Commands;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using Microsoft.AspNetCore.SignalR.Client;
using System.Threading.Tasks;
using System;
using System.Windows.Controls;
using System.Windows;
using System.Collections.ObjectModel;

namespace ChatClient
{
    public class ClientVM : INotifyPropertyChanged
    {

        #region Members

        HubConnection connection;
        private bool m_isSendEnable;
        private bool m_isConnected;
        private string m_Username;
        private string m_Message;
        private ObservableCollection<ListBoxItem> m_ItemList;


        #endregion

        #region Ctor

        public ClientVM()
        {
            InitializeFirstConnection();
            InitializeDefaults();

        }

        #endregion

        #region Properties

        public ConnectCommand ConnectHub { get; private set; }
        public SendCommand SendMessage { get; private set; }

        public bool isSendEnable
        {
            get { return m_isSendEnable; }
            set
            {
                if (m_isSendEnable != value)
                {
                    m_isSendEnable = value;
                    OnPropertyChanged();
                }
            }
        }


        public bool isConnected
        {
            get { return !m_isConnected; }
            set
            {
                if (m_isConnected != value)
                {
                    m_isConnected = value;
                    OnPropertyChanged();
                }
            }
        }



        public ObservableCollection<ListBoxItem> ItemList
        {
            get { return m_ItemList; }
            set
            {
                if (m_ItemList != value)
                {
                    m_ItemList = value;
                    OnPropertyChanged();
                }
            }
        }


        public string Username
        {
            get { return m_Username; }
            set
            {
                if (m_Username != value)
                {
                    m_Username = value;
                    OnPropertyChanged();
                }

            }
        }


        public string Message
        {
            get { return m_Message; }
            set
            {
                if (m_Message != value)
                {
                    m_Message = value;
                    OnPropertyChanged();
                }
            }
        }



        #endregion

        #region Methods

        private async void OnConnectButton()
        {
            connection.On<string>("OnConnection", (connectionID) =>
            {
                Application.Current.Dispatcher.Invoke(() =>
                {
                    ItemList.Add(new ListBoxItem() { Content = $"Connection ID: {connectionID}" });
                });
            });

            connection.On<string, string>("ReceiveMessage", (user, message) =>
             {
                 Application.Current.Dispatcher.Invoke(() =>
                 {
                     ItemList.Add(new ListBoxItem() { Content = $"{user}: {message}" });
                 });
             });

            try
            {
                await connection.StartAsync();
                isSendEnable = true;
                isConnected = true;

            }
            catch (Exception ex)
            {
                ItemList.Add(new ListBoxItem() { Content = ex.Message });
            }
        }

        private async void OnSendMessage()
        {
            try
            {
                await connection.InvokeAsync("SendMessage", Username, Message);
            }
            catch (Exception ex)
            {
                ItemList.Add(new ListBoxItem() { Content = ex.Message });
            }
        }

        private void InitializeFirstConnection()
        {
            connection = new HubConnectionBuilder()
            .WithUrl("https://localhost:5001/chatHub")
            .Build();

            connection.Closed += async (error) =>
            {
                await Task.Delay(new Random().Next(0, 5) * 1000);
                await connection.StartAsync();
            };
        }

        private void InitializeDefaults()
        {
            ItemList = new ObservableCollection<ListBoxItem>();
            ConnectHub = new ConnectCommand(OnConnectButton);
            SendMessage = new SendCommand(OnSendMessage);
            isSendEnable = false;
            isConnected = false;
        }

        #endregion

        #region NotifyChanged

        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged([CallerMemberName] string name = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }

        #endregion
    }
}
