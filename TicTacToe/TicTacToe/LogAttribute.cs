using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TicTacToe
{
    public class LogAttribute : ResultFilterAttribute, IActionFilter
    {

        public static string Request { get; set; }
        public static string Response { get; set; }
        public static string Exception { get; set; }
        public static string Comment { get; set; }
        public void OnActionExecuted(ActionExecutedContext context)
        {
            Logger logger = Logger.getInstance();
            logger.LoggingDetails(Request, Response, Exception, Comment);
        }

        public void OnActionExecuting(ActionExecutingContext context)
        {
            
        }
    }
}
