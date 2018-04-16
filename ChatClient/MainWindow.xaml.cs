using ChatInterface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace ChatClient
{
    /// <summary>
    /// Logique d'interaction pour MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public static IChatService Server;
        private static DuplexChannelFactory<IChatService> _channelFactory;

        public MainWindow()
        {
            InitializeComponent();

            _channelFactory = new DuplexChannelFactory<IChatService>(new ClientCallBack(), "ChatServiceEndPoint");
            Server = _channelFactory.CreateChannel();
            MessageTextBox.IsEnabled = false;
            SendButton.IsEnabled = false;
           
        }

        public void TakeMessage(string message, string userName)
        {
            TextDisplayTextBox.Text += userName + ": " + message + "\n";
            TextDisplayTextBox.ScrollToEnd();
        }

        private void SendButton_Click(object sender, RoutedEventArgs e)
        {
            if(MessageTextBox.Text.Length == 0)
            {
                return;
            }
            Server.SendMessageToAll(MessageTextBox.Text, UserNameTextBox.Text);
            TakeMessage(MessageTextBox.Text, "Vous");
            MessageTextBox.Text = "";

        }

        private void LoginButton_Click(object sender, RoutedEventArgs e)
        {
            int returnValue = Server.Login(UserNameTextBox.Text);
            if(returnValue == 1)
            {
                MessageBox.Show("Vous êtes déjà connecté " + UserNameTextBox.Text + "! Changer de pseudo !!");
               
                
            }
            else if(returnValue == 0)
            {

                MsgLoginLabel.Visibility = Visibility.Hidden;
                UserNameTextBox.IsEnabled = false;
                LoginButton.IsEnabled = false;
                MessageTextBox.IsEnabled = true;
                SendButton.IsEnabled = true;

                /// <summary>
                /// Chargement de tout les utilisateurs
                /// </summary>
                LoadUserList(Server.GetCurrentUsers());
                
            }
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            Server.Logout();
        }

        public void AddUserList(string userName)
        {
            if (OnlineListBox.Items.Contains(userName))
            {
                return;
            }
            OnlineListBox.Items.Add(userName);
        }

        public void RemoveUserFromList(string userName)
        {
            if (OnlineListBox.Items.Contains(userName))
            {
                OnlineListBox.Items.Remove(userName);
            }
        }

        private void LoadUserList(List<string> users)
        {
            foreach(var user in users)
            {
                AddUserList(user);
            }
        }
    }
}
