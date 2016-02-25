using System.Net;
using System.Net.Http;
using System.Reflection;
using System.Threading.Tasks;
using System.Web.Http;
using Common;
using Contracts;
using log4net;
using MediatR;
using WebApi.Handlers.Features.User;

namespace WebApi.Controllers
{
    [RoutePrefix("Api/BUser")]
    public class BUserController : BaseController
    {
        public BUserController(IUow uow, IMediator mediatR, ILog log)
        {
            Uow = uow;
            MediatR = mediatR;
            Log = log;
        }

        [HttpGet]
        [Route("VersionTest")]
        public string Version()
        {
            Log.Debug("GET Request traced");
            return $"{Assembly.GetExecutingAssembly().GetName().Version}";
        }

        [Route("Add")]
        [HttpPost]
        public async Task<IHttpActionResult> UploadAttendenceRegister(UserCreateModel model)
        {
       
            var result = await MediatR.SendAsync(model);
            if (result.ResponseMessage.IsSuccessStatusCode)
            {
                return ResponseMessage(Request.CreateResponse(HttpStatusCode.OK,result));
            }
            return ResponseMessage(Request.CreateResponse(HttpStatusCode.BadRequest, result));
        }
    }
}