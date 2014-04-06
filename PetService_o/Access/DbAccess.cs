using System.Configuration;
using System.Reflection;
using System.Web.Configuration;

namespace PetService.Access
{
    internal static class DbAccess
    {
        //internal static void ExecuteQuery(Action<IDbConnection> query)
        //{
        //    using (var dbContext = new SqlConnection(ConnectionString))
        //    {
        //        dbContext.InfoMessage += dbContext_InfoMessage;
        //        dbContext.Open();
        //        query(dbContext);
        //    }
        //}

        //static void dbContext_InfoMessage(object sender, SqlInfoMessageEventArgs e)
        //{
        //    if (e != null && e.Message != null && Debugger.IsAttached)
        //        Trace.WriteLine(e.Message);
        //}

        static DbAccess()
        {
            ConnectionString = ConfigurationManager.ConnectionStrings["PetClubDb"].ConnectionString;
        }

        public static string ConnectionString { get; private set; }
    }
}