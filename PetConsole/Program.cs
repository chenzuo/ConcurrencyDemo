using PetClubLib.Models;
using PetConsole.PetService;
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
            _client = new PetServiceClient();
            _key = _client.RegisterClient("Test");
            _running = true;

            while (_running)
            {
                Console.Write(@"Input: ");
                string[] input = Console.ReadLine().Trim().Split(',');

                if (input[0] == "exit")
                {
                    _running = false;
                }
                else if (input[0] == "listen")
                {
                    _client.GetPetOwnerAsync();
                }
                else if (input[0] == "set")
                {
                    if (input.Count() >= 4)
                    {
                        if (input[1] == "PetOwner")
                        {
                            _client.AddPetOwner(_key, new PetOwner() { Name = input[2], Occupation = input[3] });
                        }
                        else if (input[1] == "Pet")
                        {
                        }
                    }
                }
            }
        }

        static PetServiceClient _client;
        static SessionKey _key;
        static bool _running;
    }
}
