using API.Data;
using DatingApi.Helper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace DatingApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [ServiceFilter(typeof(ILogUserActivity))]
    public class BaseApiController : ControllerBase 
    {

        public BaseApiController()
        {
        }
    }
}
