using Cassandra;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TicTacToe.DataAccessLayer
{
    public class DatabaseCassandra
    {
        public string PreviousUser { get; set; }
        public string CurrentUser { get; set; }
        public  bool LogWriter(string request,string response,string exception,string comment)
        {
            // Connect to the TicTacToe keyspace on our cluster running at 127.0.0.1
            Cluster cluster = Cluster.Builder().AddContactPoint("127.0.0.1").Build();
            ISession session = cluster.Connect("tictactoe");
            //session.Execute()
            //Prepare a statement once
            string query = "INSERT INTO \"Logger\" (\"LogNumber\", \"Response\", \"Request\", \"Exception\", \"Comment\") VALUES (?,?,?,?,?)";

            var ps = session.Prepare(query);
            Random random = new Random();
            int logNumber = random.Next();
            //...bind different parameters every time you need to execute
            var statement = ps.Bind(logNumber, request, response, exception, comment);
            //Execute the bound statement with the provided parameters
            session.Execute(statement);
            return true;
        }
        public User PostUserRequest(string firstName, string lastName, string username, string token)
        {
            User user = null;
            Logger logger = Logger.getInstance();
            try
            {
                Cluster cluster = Cluster.Builder().AddContactPoint("127.0.0.1").Build();
                ISession session = cluster.Connect("tictactoe");
                //session.Execute()
                //Prepare a statement once
                string query = "INSERT INTO \"Users\" (\"Token\", \"FirstName\", \"LastName\", \"Username\") VALUES (?,?,?,?)";

                var ps = session.Prepare(query);
                Random random = new Random();
                int logNumber = random.Next();
                //...bind different parameters every time you need to execute
                var statement = ps.Bind(token, firstName, lastName, username);
                //Execute the bound statement with the provided parameters
                session.Execute(statement);

                user = new User
                {
                    FirstName = firstName,
                    LastName = lastName,
                    Username = username,
                    Token = token,
                    RegisterMessage = "Success"
                };
                return user;
            }
            catch (Exception exception)
            {
                Console.WriteLine(exception.StackTrace);
                string exc = exception.ToString().Substring(0, 49);
                string comment = exception.StackTrace.Substring(0, 49);
                logger.LoggingDetails("Add user", "Failure", exc, comment);
                user = new User
                {
                    RegisterMessage = "Failure"
                };
                return user;
            }
        }

        public bool IsSpaceAvailable(string token)
        {
            int count = 0;
            try
            {
                List<string> tokens = new List<string>();
                string query = "Select \"Token\" from \"Game\"";
                Cluster cluster = Cluster.Builder().AddContactPoint("127.0.0.1").Build();
                ISession session = cluster.Connect("tictactoe");
                var result = session.Execute(query);
                foreach(var row in result)
                {
                    string value = row.GetValue<string>("Token");
                    tokens.Add(value);
                    count++;
                }
                if (count < 2)
                {
                    if (tokens.Contains(token))
                    {
                        return true;
                    }
                    CurrentUser = token;
                    query = "INSERT INTO \"Game\" (\"Token\") VALUES (?)";

                    var ps = session.Prepare(query);
                    //...bind different parameters every time you need to execute
                    var statement = ps.Bind(token);
                    //Execute the bound statement with the provided parameters
                    session.Execute(statement);
                    return true;
                }
                else
                {
                    if (tokens.Contains(token))
                    {
                        return true;
                    }
                }
                return false;
            }
            catch (Exception exception)
            {
                Console.WriteLine(exception.StackTrace);
                return false;
            }
        }
        public bool IsTokenAvailable(string token)
        {
            try
            {
                Cluster cluster = Cluster.Builder().AddContactPoint("127.0.0.1").Build();
                ISession session = cluster.Connect("tictactoe");
                string query = "Select \"Token\" from \"Users\"";
                var result = session.Execute(query);
                foreach (var row in result)
                {
                    string value = row.GetValue<string>("Token");
                    if (value.Equals(token))
                    {
                        return true;
                    }
                }
                return false;
            }
            catch (Exception exception)
            {
                Console.WriteLine(exception.StackTrace);
                return false;
            }
        }
        public void RemoveExistingUsers()
        {
            try
            {
                Cluster cluster = Cluster.Builder().AddContactPoint("127.0.0.1").Build();
                ISession session = cluster.Connect("tictactoe");
                string query = "truncate \"Game\"";
                //Execute the bound statement with the provided parameters
                session.Execute(query);
            }
            catch (Exception exception)
            {
                Console.WriteLine(exception.StackTrace);
            }
        }

    }
}
