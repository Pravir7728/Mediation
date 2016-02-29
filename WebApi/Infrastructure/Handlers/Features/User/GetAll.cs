using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using AutoMapper;
using Common;
using Contracts;
using MediatR;

#pragma warning disable 1998
#pragma warning disable 618

namespace WebApi.Infrastructure.Handlers.Features.User
{
    #region Handler
    public class GetAll : IAsyncRequestHandler<GetAllUserRequest, ResponseObject>
    {
        private readonly IUow _uow;

        public GetAll(IUow uow)
        {
            _uow = uow;
        }

        public async Task<ResponseObject> Handle(GetAllUserRequest message)
        {
            var result = new Logic.User(_uow).GetAll();
            if (result == null) return null;
            var mappedResult = result.Select(Mapper.Map<GetAllUserResponse>).ToList();
            var response = new ResponseObject
            {
                ResponseMessage = new HttpResponseMessage(HttpStatusCode.OK),
                Data = mappedResult,
                Message = "Users retrieved Successfully",
                IsSuccessful = true
            };
            return response;
        }
    }
    #endregion

    #region Request/Response Model
    public class GetAllUserRequest : IAsyncRequest<ResponseObject>
    {

    }

    public class GetAllUserResponse : IAsyncRequest<ResponseObject>
    {
        public string UserName { get; set; }
    }
    #endregion
}