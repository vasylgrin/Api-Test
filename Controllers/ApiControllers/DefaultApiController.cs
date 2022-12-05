using Api_Test.Controllers.DataBase;
using Api_Test.Models;
using Microsoft.AspNetCore.Mvc;

namespace Api_Test.Controllers.ApiControllers
{
    [Route("api/v1/")]
    public class DefaultApiController : ControllerBase
    {
        readonly GetController getController = new();
        readonly PostController postController = new();

        [HttpGet, Route("person")]
        public object GetAllCharacter(string name) => getController.GetPerson(name);


        [HttpPost, Route("check-person")]
        public object CheckPerson(PostRequest request) => postController.CheckIfExistPersonInTheEpisode(request);
    }
}
