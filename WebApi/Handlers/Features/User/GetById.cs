using System;
using System.Diagnostics;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Common;
using Contracts;
using FluentValidation;
using FluentValidation.Attributes;
using MediatR;
using static AutoMapper.Mapper;

#pragma warning disable 1998
#pragma warning disable 618

namespace WebApi.Handlers.Features.User
{

    #region Handler

    public class GetById : IAsyncRequestHandler<GetUserRequest, ResponseObject>
    {
        private readonly IUow _uow;

        public GetById(IUow uow)
        {
            _uow = uow;
        }

        public async Task<ResponseObject> Handle(GetUserRequest message)
        {
            var result = new Logic.User(_uow).GetUserById(message.UserId);
            if (result == null) return null;
            var dest = Map<GetUserResponse>(result);
            var response = new ResponseObject
            {
                ResponseMessage = new HttpResponseMessage(HttpStatusCode.OK),
                Data = dest,
                Message = "User retrieved Successfully",
                IsSuccessful = true
            };
            return response;
        }
    }

    #endregion

    #region Request/Response Model

    [Validator(typeof (GetUserResponseModel))]
    public class GetUserRequest : IAsyncRequest<ResponseObject>
    {
        public Guid UserId { get; set; }
    }

    public class GetUserResponse : IAsyncRequest<ResponseObject>
    {
        public string UserName { get; set; }
        public string Password { get; set; }
    }

    #endregion

    #region Validator

    public class GetUserResponseModel : AbstractValidator<GetUserRequest>
    {
        private readonly IUow _uow;

        public GetUserResponseModel(IUow uow)
        {
            _uow = uow;
            RuleFor(user => user.UserId).NotEmpty();
        }

        public void Handle(GetUserRequest request)
        {
            Debug.WriteLine("GetById Handler");
        }
    }

    #endregion
}