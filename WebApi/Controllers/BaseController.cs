using System.Web.Http;
using Contracts;
using log4net;
using MediatR;

namespace WebApi.Controllers
{
    public class BaseController : ApiController
    {
        public IUow Uow { get; set; }
        public IMediator MediatR { get; set; }
        public ILog Log { get; set; }
    }
}