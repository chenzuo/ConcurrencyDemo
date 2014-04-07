using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PetConsole
{
    class Program
    {
        static void Main(string[] args)
        {
            _client = new PetClient();
            _running = true;

            while (_running)
            {
                Console.Write(@"Input: ");
                string input = Console.ReadLine().Trim();

                if (input == "exit")
                {
                    _running = false;
                }
                else if (input == "listen")
                {
                    _client.GetPetOwnerAsync();
                }
                else if (input.StartsWith("set"))
                {
                }
            }
        }

        static PetClient _client;
        static bool _running;
    }
}
