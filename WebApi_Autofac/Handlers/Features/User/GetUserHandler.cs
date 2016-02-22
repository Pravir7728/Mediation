using System;
using System.Diagnostics;
using System.Net;
using System.Net.Http;
using AutoMapper;
using Common;
using Contracts;
using FluentValidation;
using FluentValidation.Attributes;
using MediatR;
using WebApi_Autofac.Infrastructure.Processes;

#pragma warning disable 618

namespace WebApi_Autofac.Handlers.Features.User
{

    #region Handler

    public class GetUserHandler : MediatR.IRequestHandler<GetUserRequest, ResponseObject>
    {
        private readonly IUow Uow;

        public GetUserHandler(IUow uow)
        {
            Uow = uow;
        }

        public ResponseObject Handle(GetUserRequest message)
        {
            var result = new Logic.User(Uow).GetUserById(message.UserId);
            if (result == null) return null;
            var dest = Mapper.Map<Domain.User>(result);
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

    #region View Model

    [Validator(typeof (UserModelValidator))]
    public class GetUserRequest : IRequest<ResponseObject>
    {
        public Guid UserId { get; set; }
    }

    [Validator(typeof (UserModelValidator))]
    public class GetUserResponse : IRequest<ResponseObject>
    {
        public string UserName { get; set; }
        public string Password { get; set; }
    }

    #endregion

    #region

    public class GetUserResponseModel : AbstractValidator<GetUserRequest>, IPreRequestHandler<GetUserRequest>
    {
        private readonly IUow Uow;

        public GetUserResponseModel(IUow uow)
        {
            Uow = uow;
            RuleFor(user => user.UserId).NotEmpty();
        }

        public void Handle(GetUserRequest request)
        {
            Debug.WriteLine("GetAccountPreProcessor Handler");
        }
    }

    #endregion
}