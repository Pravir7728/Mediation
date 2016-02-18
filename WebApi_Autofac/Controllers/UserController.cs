using System.Net;
using System.Net.Http;
using System.Reflection;
using System.Web.Http;
using Common;
using Contracts;
using Logic;
using MediatR;
using WebApi_Autofac.Handlers;
using WebApi_Autofac.Handlers.Features.User;

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


        [Route("Add")]
        [HttpPost]
        public object UploadAttendenceRegister(UserCreateModel model)
        {
            if (!ModelState.IsValid) return null;
            var result = MediatR.Send(model);
            return result;
        }
    }
}