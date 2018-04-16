using ChatInterface;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;

namespace ChatServer
{
    [ServiceBehavior(ConcurrencyMode = ConcurrencyMode.Multiple, InstanceContextMode = InstanceContextMode.Single)]
    // REMARQUE : vous pouvez utiliser la commande Renommer du menu Refactoriser pour changer le nom de classe "ChatService" à la fois dans le code et le fichier de configuration.
    public class ChatService : IChatService
    {
        public ConcurrentDictionary<string, ConnectedClient> _connectedClients = new ConcurrentDictionary<string, ConnectedClient>();

        public int Login(string userName)
        {

            /// <summary> 
            /// Si Quelqu'un se connecte avec le meme nom d'utilisateur
            /// </summary>
            foreach(var client in  _connectedClients)
            {
                if(client.Key.ToLower() == userName.ToLower())
                {
                    /// <summary>
                    /// Si c'est oui
                    /// </summary> 
                    return 1;
                }
            }

            var establishedUserConnection = OperationContext.Current.GetCallbackChannel<Client>();

            ConnectedClient newClient = new ConnectedClient();

            newClient.connection = establishedUserConnection;
            newClient.UserName = userName;

            _connectedClients.TryAdd(userName, newClient);

            updateHelper(0, userName);

            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("{0} vient de se connecter; Date et heure de connexion: {1}", newClient.UserName, System.DateTime.Now);
            Console.ResetColor();

            return 0;
        }

        public void Logout()
        {
            ConnectedClient client = GetMyClient();
            if(client != null)
            {
                ConnectedClient removedClient;
                _connectedClients.TryRemove(client.UserName, out removedClient);

                updateHelper(1, removedClient.UserName);

                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("{0} vient de se déconnecter; Date et heure de déconnexion: {1}", removedClient.UserName, System.DateTime.Now);
                Console.ResetColor();
            }
        }

        public ConnectedClient GetMyClient()
        {
            var establishedUserConnection = OperationContext.Current.GetCallbackChannel<Client>();
            foreach(var client in _connectedClients)
            {
                if(client.Value.connection == establishedUserConnection)
                {
                    return client.Value;
                }
            }
            return null;
        }

        public void SendMessageToAll(string message, string userName)
        {
           foreach(var client in _connectedClients)
            {
                if (client.Key.ToLower() != userName.ToLower())
                {
                    client.Value.connection.GetMessage(message, userName);
                }
            }
        }
        private void updateHelper(int value, string userName)
        {
            foreach (var client in _connectedClients)
            {
                if(client.Value.UserName.ToLower() != userName.ToLower())
                {

                    client.Value.connection.GetUpdate(value, userName);

                }
            }
        }

        public List<string> GetCurrentUsers()
        {
            List<string> listOfUsers = new List<string>();
            foreach(var client in _connectedClients)
            {
                listOfUsers.Add(client.Value.UserName);
            }
            return listOfUsers;
        }
    }
}
