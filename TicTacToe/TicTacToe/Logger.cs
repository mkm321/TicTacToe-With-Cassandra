using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TicTacToe.DataAccessLayer;

namespace TicTacToe
{
    public class Logger
    {
        private static Logger _Logger = null;
        private static Object _classLock = typeof(Logger);
        Database database = new Database();
        DatabaseCassandra databaseCassandra = new DatabaseCassandra();
        private Logger()
        {

        }

        public static Logger getInstance()
        {
            //lock object to make it thread safe  
            lock (_classLock)
            {
                if (_Logger == null)
                {
                    _Logger = new Logger();

                }
            }
            return _Logger;
        }

        public void LoggingDetails(string request, string response, string exception, string comment)
        {
            database.AddLoggingDetails(request, response, exception, comment);
            bool result = databaseCassandra.LogWriter(request, response, exception, comment);
        }
    }
}
