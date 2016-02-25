using System.Net;
using System.Net.Http;
using System.Reflection;
using System.Threading.Tasks;
using System.Web.Http;
using Contracts;
using log4net;
using MediatR;
using WebApi.Handlers.Features.User;

namespace WebApi.Controllers
{
    [RoutePrefix("Api/User")]
    public class UserController : BaseController
    {
        public UserController(IUow uow, IMediator mediatR, ILog log)
        {
            Uow = uow;
            MediatR = mediatR;
            Log = log;
        }

        [HttpGet]
        [Route("VersionTest")]
        public string Version()
        {
            Log.Debug("HTTP GET Request traced");
            return $"{Assembly.GetExecutingAssembly().GetName().Version}";
        }

        [Route("Add")]
        [HttpPost]
        public async Task<IHttpActionResult> AddUser(UserCreateModel model)
        {
            Log.Debug("HTTP POST Request traced");
            if (ModelState.IsValid)
            {
                var result = await MediatR.SendAsync(model);
                if (result.ResponseMessage.IsSuccessStatusCode)
                {
                    return ResponseMessage(Request.CreateResponse(HttpStatusCode.OK, result));
                }
            }
            return ResponseMessage(Request.CreateResponse(HttpStatusCode.BadRequest, ModelState.Values));
        }

        [Route("GetById/{UserId}")]
        [HttpGet]
        public async Task<IHttpActionResult> GetUserById([FromUri] GetUserRequest model)
        {
            Log.Debug("HTTP GET Request traced");
            if (ModelState.IsValid)
            {
                var result = await MediatR.SendAsync(model);
                return ResponseMessage(Request.CreateResponse(HttpStatusCode.OK, result));
            }
            return ResponseMessage(Request.CreateResponse(HttpStatusCode.BadRequest, ModelState.Values));
        }

        [Route("GetAll")]
        [HttpGet]
        public async Task<IHttpActionResult> GetAll(GetAllUserRequest model)
        {
            Log.Debug("HTTP GET Request traced");
            var result = await MediatR.SendAsync(model);
            return ResponseMessage(Request.CreateResponse(HttpStatusCode.OK, result));
        }
    }
}