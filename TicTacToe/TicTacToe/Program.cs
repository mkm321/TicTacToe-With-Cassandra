using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using TicTacToe.DataAccessLayer;

namespace TicTacToe
{
    public class Program
    {
        public static void Main(string[] args)
        {
            //Database database = new Database();
            DatabaseCassandra database = new DatabaseCassandra();
            database.RemoveExistingUsers();
            BuildWebHost(args).Run();
        }

        public static IWebHost BuildWebHost(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                .UseStartup<Startup>()
                .Build();
    }
}
