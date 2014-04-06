using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PetConsole
{
    public class ClientComm
    {
        public ClientComm()
        {
            Uri baseAddress = new Uri("http://localhost:17695/PetService");
            try
            {
                System.ServiceModel.Channels.Binding binding = new BasicHttpBinding();

            }
        }
    }
}
