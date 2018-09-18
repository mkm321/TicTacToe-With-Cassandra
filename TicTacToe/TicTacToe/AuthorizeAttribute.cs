using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TicTacToe.DataAccessLayer;

namespace TicTacToe
{
    public class AuthorizeAttribute : ResultFilterAttribute, IActionFilter
    {
        Logger logger = Logger.getInstance();
        public void OnActionExecuted(ActionExecutedContext context)
        {
            //throw new NotImplementedException();
        }

        public void OnActionExecuting(ActionExecutingContext context)
        {
            var token = context.HttpContext.Request.Headers["token"].ToString();
            if (string.IsNullOrEmpty(token))
            {
                logger.LoggingDetails("Starting the game", "Failure", "Unauthorized Access Exception", "Token in header is null!");
                throw new UnauthorizedAccessException("Token not passed");
            }
            else
            {
                DatabaseCassandra database = new DatabaseCassandra();
                //Database database = new Database();
                bool response = database.IsTokenAvailable(token.ToString());
                if (response == false)
                {
                    logger.LoggingDetails("Starting the game", "Failure", "Unauthorized Access Exception", "Token is Invalid!");
                    throw new UnauthorizedAccessException("Invalid token passed");
                }
                bool isSpaceAvailable = database.IsSpaceAvailable(token.ToString());
                if (isSpaceAvailable == false)
                {
                    logger.LoggingDetails("Starting the game", "Failure", "Entry Point Not Found Exception", "No Available Space for This token!");
                    throw new EntryPointNotFoundException("No Available Space for This token");
                }
            }
            logger.LoggingDetails("Starting the game", "Success", "No Exception", "User is successfully entered into playing arena!");
        }
    }
}
