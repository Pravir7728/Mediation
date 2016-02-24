using System.Net;
using System.Net.Http;
using System.Reflection;
using System.Threading.Tasks;
using System.Web.Http;
using Common;
using Contracts;
using MediatR;
using WebApi.Features;

namespace WebApi.Controllers
{
    [RoutePrefix("Api/BUser")]
    public class BUserController : BaseController
    {
        public BUserController(IUow uow, IMediator mediatR)
        {
            Uow = uow;
            MediatR = mediatR;
        }

        [HttpGet]
        [Route("VersionTest")]
        public string Version()
        {
            return string.Format("{0}", Assembly.GetExecutingAssembly().GetName().Version);
        }

        //Create 
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