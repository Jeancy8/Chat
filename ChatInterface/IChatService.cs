using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;

namespace ChatInterface
{
    // REMARQUE : vous pouvez utiliser la commande Renommer du menu Refactoriser pour changer le nom d'interface "IChatService" à la fois dans le code et le fichier de configuration.
    [ServiceContract(CallbackContract=typeof(Client))]
    public interface IChatService
    {
        [OperationContract]
        int Login(string userName);
        [OperationContract]
        void Logout();
        [OperationContract]
        void SendMessageToAll(string message, string userName);
        [OperationContract]
        List<string> GetCurrentUsers();
    }
}
