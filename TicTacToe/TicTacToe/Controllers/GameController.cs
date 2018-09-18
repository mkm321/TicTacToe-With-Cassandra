using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TicTacToe.BusinessLogic;

namespace TicTacToe.Controllers
{
    [Produces("application/json")]
    [Route("api/Game")]
    public class GameController : Controller
    {
        [HttpPut]
        [Authorize]
        [Log]
        public IActionResult MakeAMove([FromBody] int move)
        {
            Board board = new Board();
            bool response = board.IsSquareAvailable(move);
            if (response == true)
             {
                bool result = board.AddPlayerMove(move);
                if(result == false)
                {
                    LogAttribute.Request = "Make a Move";
                    LogAttribute.Response = "Failure";
                    LogAttribute.Exception = "Bad Request";
                    LogAttribute.Comment = "One player can not choose twice";
                    return BadRequest("One player can not have choose simultaneously!!");
                }
                LogAttribute.Request = "Make a Move";
                LogAttribute.Response = "Success";
                LogAttribute.Exception = "No Exception";
                LogAttribute.Comment = "Move submitted successfully!";
                 return Ok("Move submitted successfully!!!");
            }
            LogAttribute.Request = "Make a Move";
            LogAttribute.Response = "Failure";
            LogAttribute.Exception = "Bad Request";
            LogAttribute.Comment = "This move is already choosen";
            return BadRequest("This move is already choosen!!!!!");
        }
        [HttpGet]
        [Log]
        public IActionResult GetStatus()
        {
            Board board = new Board();
            string response = board.CheckStatus();
            LogAttribute.Request = "Make a Move";
            LogAttribute.Response = "Success";
            LogAttribute.Exception = "No Exception";
            LogAttribute.Comment = "Game status :-  " + response;
            return Ok(response);
        }
    }
}