using ApplicationLayer.Models.RagnarokModels;
using System.Collections.Generic;

namespace ApplicationLayer.Singleton.RagnarokSingleton
{
    public class ClientSingleton
    {
        private static Client client;
        private ClientSingleton(Client client)
        {
            ClientSingleton.client = client;
        }

        public static ClientSingleton Instance(Client client)
        {
            return new ClientSingleton(client);
        }

        public static Client GetClient()
        {
            return client;
        }

    }
}
