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

#pragma warning disable 1998

namespace WebApi.Handlers.Features.User
{

    #region Handler

    public class Delete : IAsyncRequestHandler<UserDeleteRequestModel, ResponseObject>
    {
        private readonly IUow _uow;

        public Delete(IUow uow)
        {
            _uow = uow;
        }

        public async Task<ResponseObject> Handle(UserDeleteRequestModel message)
        {
            new Logic.User(_uow).DeleteUserById(message.UserId);
            var response = new ResponseObject
            {
                ResponseMessage = new HttpResponseMessage(HttpStatusCode.OK),
                Data = new UserDeleteResponseModel {DeleteIsSuccessful = true},
                Message = "User Deleted Successfully",
                IsSuccessful = true
            };
            return response;
        }
    }

    #endregion

    #region Request/Response Model

    [Validator(typeof (DeleteUserValidator))]
    public class UserDeleteRequestModel : IAsyncRequest<ResponseObject>
    {
        public Guid UserId { get; set; }
    }

    public class UserDeleteResponseModel : IAsyncRequest<ResponseObject>
    {
        public bool DeleteIsSuccessful { get; set; }
    }

    #endregion

    #region Validator

    public class DeleteUserValidator : AbstractValidator<GetUserRequest>
    {
        private readonly IUow _uow;

        public DeleteUserValidator(IUow uow)
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