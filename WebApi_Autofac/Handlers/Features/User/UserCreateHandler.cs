﻿using System.Net;
using System.Net.Http;
using AutoMapper;
using Common;
using Contracts;
using FluentValidation;
using FluentValidation.Attributes;
using FluentValidation.Results;
using MediatR;

#pragma warning disable 618

namespace WebApi_Autofac.Handlers.Features.User
{

    #region Handler

    public class UserCreateHandler : IRequestHandler<UserCreateModel, ResponseObject>
    {
        private readonly IUow Uow;

        public UserCreateHandler(IUow uow)
        {
            Uow = uow;
        }

        public ResponseObject Handle(UserCreateModel message)
        {
            var dest = Mapper.Map<Domain.User>(message);
            var result = new Logic.User(Uow).AddUser(dest);
            if (result == null) return null;
            var response = new ResponseObject
            {
                ResponseMessage = new HttpResponseMessage(HttpStatusCode.OK),
                Data = result,
                Message = "Users retrieved Successfully",
                IsSuccessful = true
            };
            return response;
        }
    }

    #endregion

    #region View model

    [Validator(typeof (UserModelValidator))]
    public class UserCreateModel : IRequest<ResponseObject>
    {
        public string UserName { get; set; }
        public string Password { get; set; }
    }

    #endregion

    #region Validation

    public class UserModelValidator : AbstractValidator<UserCreateModel>
    {
        private readonly IUow Uow;

        public UserModelValidator(IUow uow)
        {
            Uow = uow;
            RuleFor(user => user.UserName).NotEmpty();
            RuleFor(user => user.UserName)
                .Length(3, 250)
                .WithMessage("Please Specify Username that is more than 3 characters");
            RuleFor(user => user.Password).NotEmpty();
            Custom(rm =>
            {
                var userProfile = new Logic.User(Uow).GetUserByName(rm.UserName);
                return userProfile != null
                    ? new ValidationFailure("UserName", "This Username is already registered")
                    : null;
            });
        }
    }

    #endregion
}