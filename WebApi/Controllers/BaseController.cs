using System.Web.Http;
using Contracts;
using MediatR;

namespace WebApi.Controllers
{
    public class BaseController : ApiController
    {
        public IUow Uow { get; set; }
        public IMediator MediatR { get; set; }
    }
}