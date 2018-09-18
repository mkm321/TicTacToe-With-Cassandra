using System;
using System.Collections.Generic;
using System.Data.SqlClient;
namespace TicTacToe
{
    public class Database
    {
        public string PreviousUser { get; set; }
        public string CurrentUser { get; set; }
        public User PostUserRequest(string firstName, string lastName, string username, string token)
        {
            SqlConnection connection = null;
            User user = null;
            Logger logger = Logger.getInstance();
            try
            {
                connection = new SqlConnection();
                connection.ConnectionString = "Data Source=TAVDESK083;Initial Catalog=TicTacToe;User ID= sa; Password=test123!@#";
                connection.Open();
                string query = "Insert into Users(FirstName,LastName,Username,Token) values(@FirstName,@LastName,@Username,@Token)";
                SqlCommand sqlCommand = new SqlCommand(query, connection);
                sqlCommand.Parameters.Add(new SqlParameter("FirstName", firstName));
                sqlCommand.Parameters.Add(new SqlParameter("LastName", lastName));
                sqlCommand.Parameters.Add(new SqlParameter("Username", username));
                sqlCommand.Parameters.Add(new SqlParameter("Token", token));
                sqlCommand.ExecuteNonQuery();
                connection.Close();
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
                connection.Close();
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
            SqlConnection connection = null;
            try
            {
                connection = new SqlConnection();
                connection.ConnectionString = "Data Source=TAVDESK083;Initial Catalog=TicTacToe;User ID= sa; Password=test123!@#";
                connection.Open();
                List<string> tokens = new List<string>();
                string query = "Select Token from Game";
                SqlCommand sqlCommand = new SqlCommand(query, connection);
                SqlDataReader sqlDataReader = sqlCommand.ExecuteReader();
                while (sqlDataReader.Read())
                {
                    tokens.Add(sqlDataReader.GetValue(0).ToString());
                    count++;
                }
                connection.Close();
                connection.Open();
                if (count < 2)
                {
                    if (tokens.Contains(token))
                    {
                        return true;
                    }
                    CurrentUser = token;
                    query = "Insert into Game(Token) values(@Token)";
                    sqlCommand = new SqlCommand(query, connection);
                    sqlCommand.Parameters.Add(new SqlParameter("Token", token));
                    sqlCommand.ExecuteNonQuery();
                    connection.Close();
                    return true;
                }
                else
                {
                    if (tokens.Contains(token))
                    {
                        return true;
                    }
                }
                connection.Close();
                return false;
            }
            catch (Exception exception)
            {
                connection.Close();
                Console.WriteLine(exception.StackTrace);
                return false;
            }
        }

        public bool IsTokenAvailable(string token)
        {
            SqlConnection connection = null;
            try
            {
                connection = new SqlConnection();
                connection.ConnectionString = "Data Source=TAVDESK083;Initial Catalog=TicTacToe;User ID= sa; Password=test123!@#";
                connection.Open();
                string query = "Select Token from Users";
                SqlCommand sqlCommand = new SqlCommand(query, connection);
                SqlDataReader sqlDataReader = sqlCommand.ExecuteReader();
                while (sqlDataReader.Read())
                {
                    if (sqlDataReader.GetValue(0).Equals(token))
                    {
                        connection.Close();
                        return true;
                    }
                }
                connection.Close();
                return false;
            }
            catch (Exception exception)
            {
                connection.Close();
                Console.WriteLine(exception.StackTrace);
                return false;
            }
        }
        public void RemoveExistingUsers()
        {
            SqlConnection connection = null;
            try
            {
                connection = new SqlConnection();
                connection.ConnectionString = "Data Source=TAVDESK083;Initial Catalog=TicTacToe;User ID= sa; Password=test123!@#";
                connection.Open();
                string query = "Delete from Game";
                SqlCommand sqlCommand = new SqlCommand(query, connection);
                sqlCommand.ExecuteNonQuery();
                connection.Close();
            }
            catch (Exception exception)
            {
                connection.Close();
                Console.WriteLine(exception.StackTrace);
            }
        }
        public void AddLoggingDetails(string request, string response, string exception, string comment)
        {
            SqlConnection connection = null;
            try
            {
                connection = new SqlConnection();
                connection.ConnectionString = "Data Source=TAVDESK083;Initial Catalog=TicTacToe;User ID=sa;Password=test123!@#";
                connection.Open();
                string query = "Insert into Logger(Request,Response,Exception,Comment) values(@Request,@Response,@Exception,@Comment)";
                SqlCommand sqlCommand = new SqlCommand(query, connection);
                sqlCommand.Parameters.Add(new SqlParameter("Request", request));
                sqlCommand.Parameters.Add(new SqlParameter("Response", response));
                sqlCommand.Parameters.Add(new SqlParameter("Exception", exception));
                sqlCommand.Parameters.Add(new SqlParameter("Comment", comment));
                sqlCommand.ExecuteNonQuery();
                connection.Close();
            }
            catch (Exception e)
            {
                connection.Close();
                Console.WriteLine(e.StackTrace);
            }
        }
    }
}
