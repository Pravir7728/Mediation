using System;
using System.Diagnostics;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using AutoMapper;
using Common;
using Contracts;
using FluentValidation;
using FluentValidation.Attributes;
using MediatR;

#pragma warning disable 1998
#pragma warning disable 618

namespace WebApi.Infrastructure.Handlers.Features.User
{

    #region Handler

    public class Update : IAsyncRequestHandler<UserUpdateModel, ResponseObject>
    {
        private readonly IUow _uow;

        public Update(IUow uow)
        {
            _uow = uow;
        }

        public async Task<ResponseObject> Handle(UserUpdateModel message)
        {
            var dest = Mapper.Map<Domain.User>(message);
            var result = new Logic.User(_uow).UpdateUser(dest);
            if (result == null) return null;
            var response = new ResponseObject
            {
                ResponseMessage = new HttpResponseMessage(HttpStatusCode.OK),
                Data = result,
                Message = "User Updated Successfully",
                IsSuccessful = true
            };
            return response;
        }
    }

    #endregion

    #region Request/Response Model

    [Validator(typeof (UpdateUserValidator))]
    public class UserUpdateModel : IAsyncRequest<ResponseObject>
    {
        public Guid UserId { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
    }

    #endregion

    #region Validator

    public class UpdateUserValidator : AbstractValidator<UserUpdateModel>
    {
        private readonly IUow _uow;

        public UpdateUserValidator(IUow uow)
        {
            _uow = uow;
            RuleFor(user => user.UserId).NotEmpty();
            RuleFor(user => user.UserName).NotEmpty();
            RuleFor(user => user.UserName)
                .Length(3, 250)
                .WithMessage("Please Specify Username that is more than 3 characters");
            RuleFor(user => user.Password).NotEmpty();
        }

        public Task Handle(UserUpdateModel request)
        {
            Debug.WriteLine("UserUpdateModel Handler");
            return Task.FromResult(true);
        }
    }

    #endregion
}