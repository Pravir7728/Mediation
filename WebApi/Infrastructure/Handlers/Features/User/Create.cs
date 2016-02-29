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

    public class Create : IAsyncRequestHandler<UserCreateModel, ResponseObject>
    {
        private readonly IUow _uow;

        public Create(IUow uow)
        {
            _uow = uow;
        }

        public async Task<ResponseObject> Handle(UserCreateModel message)
        {
            var dest = Mapper.Map<Domain.User>(message);
            var result = new Logic.User(_uow).AddUser(dest);
            if (result == null) return null;
            var response = new ResponseObject
            {
                ResponseMessage = new HttpResponseMessage(HttpStatusCode.OK),
                Data = result,
                Message = "Users Added Successfully",
                IsSuccessful = true
            };
            return response;
        }
    }

    #endregion

    #region Request/Response Model

    [Validator(typeof (UserModelValidator))]
    public class UserCreateModel : IAsyncRequest<ResponseObject>
    {
        public string UserName { get; set; }
        public string Password { get; set; }
    }

    #endregion

    #region Validator

    public class UserModelValidator : AbstractValidator<UserCreateModel>
    {
        private readonly IUow _uow;

        public UserModelValidator(IUow uow)
        {
            _uow = uow;
            RuleFor(user => user.UserName).NotEmpty();
            RuleFor(user => user.UserName)
                .Length(3, 250)
                .WithMessage("Please Specify Username that is more than 3 characters");
            RuleFor(user => user.Password).NotEmpty();
        }

        public Task Handle(UserCreateModel request)
        {
            Debug.WriteLine("UserCreateModel Handler");
            return Task.FromResult(true);
        }
    }

    #endregion
}