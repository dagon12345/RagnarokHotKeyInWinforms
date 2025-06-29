using ApplicationLayer.Models.RagnarokModels;
using System.Collections.Generic;

namespace ApplicationLayer.Singleton.RagnarokSingleton
{
    public class ClientListSingleton
    {

        private static List<Client> clients = new List<Client>();

        public static void AddClient(Client c)
        {
            clients.Add(c);
        }

        public static void RemoveClient(Client c)
        {
            clients.Remove(c);
        }

        public static List<Client> GetAll()
        {
            return clients;
        }

        public static bool ExistsByProcessName(string processName)
        {
            return clients.Exists(client => client.processName == processName);
        }

    }
}
