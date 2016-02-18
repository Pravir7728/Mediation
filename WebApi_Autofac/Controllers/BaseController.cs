using System.Web.Http;
using Contracts;
using MediatR;

namespace WebApi_Autofac.Controllers
{
    public class BaseController : ApiController
    {
        public IUow Uow { get; set; }
        public IMediator MediatR { get; set; }
    }
}