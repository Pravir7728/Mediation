using System.Reflection;
using System.Web.Http;
using Contracts;
using MediatR;
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

        //Version Test
        [HttpGet]
        [Route("VersionTest")]
        public string Version()
        {
            return string.Format("{0}", Assembly.GetExecutingAssembly().GetName().Version);
        }

        //Create 
        [Route("Add")]
        [HttpPost]
        public object UploadAttendenceRegister(UserCreateModel model)
        {
            if (!ModelState.IsValid) return null;
            var result = MediatR.Send(model);
            return result;
        }

        //Read
        [Route("GetById/{userId}")]
        [HttpGet]
        public object GetById([FromUri] GetUserRequest model)
        {
            var result = MediatR.Send(model);
            return result;
        }

        //Read All
        [Route("GetAll")]
        [HttpGet]
        public object GetAll(GetAllUserRequest model)
        {
            var result = MediatR.Send(model);
            return result;
        }

        //Update
        [Route("Update")]
        [HttpPost]
        public object Update(UserCreateModel model)
        {
            if (!ModelState.IsValid) return null;
            var result = MediatR.Send(model);
            return result;
        }

        //Delete
        [Route("Delete")]
        [HttpPost]
        public object Delete(UserCreateModel model)
        {
            if (!ModelState.IsValid) return null;
            var result = MediatR.Send(model);
            return result;
        }
    }
}