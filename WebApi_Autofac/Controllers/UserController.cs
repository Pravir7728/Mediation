using System.Net;
using System.Net.Http;
using System.Reflection;
using System.Web.Http;
using Common;
using Contracts;
using Logic;
using MediatR;
using WebApi_Autofac.Handlers;

namespace WebApi_Autofac.Controllers
{
    [RoutePrefix("Api/User")]
    public class UserController : BaseController
    {
        public UserController(IUow uow, IMediator mediatR)
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

        [Route("GetAll")]
        public IHttpActionResult GetAll()
        {
            IHttpActionResult response =
                ResponseMessage(Request.CreateResponse(HttpStatusCode.InternalServerError, "Error Processing Request"));
            var result = new User(Uow).GetAll();

            if (result == null) return response;
            var responseMessage = Request.CreateResponse(HttpStatusCode.OK, new ResponseObject
            {
                Data = result,
                Message = "Users retrieved Successfully",
                IsSuccessful = true
            });

            if (result.Count == 0)
            {
                responseMessage = Request.CreateResponse(HttpStatusCode.OK, new ResponseObject
                {
                    Message = "No Users Currently Exist"
                });
            }
            response = ResponseMessage(responseMessage);
            return response;
        }

        [Route("Add")]
        [HttpPost]
        public object UploadAttendenceRegister(UserCreateModel model)
        {
            if (ModelState.IsValid)
            {
                var result = MediatR.Send(model);
                return result;
            }
            return null;
        }
    }
}