using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TicTacToe.DataAccessLayer;

namespace TicTacToe.Controllers
{
    [Produces("application/json")]
    [Route("api/Identity")]
    public class IdentityController : Controller
    {
        [HttpPost]
        [Log]
        public IActionResult RegisterUser([FromBody] User user)
        {
            DatabaseCassandra database = new DatabaseCassandra();
            User userResponse = new User();
            Guid guid = Guid.NewGuid();
            string _token = Convert.ToBase64String(guid.ToByteArray());
            _token = _token.Replace("=", "");
            _token = _token.Replace("+", "");
            userResponse = database.PostUserRequest(user.FirstName, user.LastName, user.Username, _token);
            if (userResponse.RegisterMessage.Equals("Failure"))
            {
                LogAttribute.Request = "Add User";
                LogAttribute.Response = "Failure";
                LogAttribute.Exception = "Bad Request";
                LogAttribute.Comment = "User Failed to add into database";
                return BadRequest(userResponse);
            }
            LogAttribute.Request = "Add User";
            LogAttribute.Response = "Success";
            LogAttribute.Exception = "No Exception";
            LogAttribute.Comment = "User Successfully added into database";
            return Ok(userResponse);
        }
    }
}