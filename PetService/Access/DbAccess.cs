using System.Configuration;

namespace PetService.Access
{
    internal static class DbAccess
    {
        static DbAccess()
        {
            ConnectionString = ConfigurationManager.ConnectionStrings["PetClubDb"].ConnectionString;
        }

        public static string ConnectionString { get; private set; }
    }
}