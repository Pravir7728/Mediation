﻿using System.Diagnostics;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using AutoMapper;
using Common;
using Contracts;
using Domain;
using FluentValidation;
using FluentValidation.Attributes;
using MediatR;
using WebApi.Infrastructure;

#pragma warning disable 618

namespace WebApi.Features
{

    #region Handler

    public class UserCreateHandler : IAsyncRequestHandler<UserCreateModel, ResponseObject>
    {
        private readonly IUow Uow;
        private readonly IValidatorFactory Validator;

        public UserCreateHandler(IUow uow, ModelValidatorFactory validatorFactory)
        {
            Uow = uow;
            Validator = validatorFactory;
        }

        public async Task<ResponseObject> Handle(UserCreateModel message)
        {
            var validationResult = Validator.GetValidator<UserModelValidator>().Validate(message);
            var response = new ResponseObject
            {
                ResponseMessage = new HttpResponseMessage(HttpStatusCode.BadRequest),
                Data = validationResult.Errors,
                Message = "Validation Failed on one or more properties",
                IsSuccessful = false
            };
            if (!validationResult.IsValid) return response;
            var dest = Mapper.Map<User>(message);
            var result = new Logic.User(Uow).AddUser(dest);
            if (result == null) return null;
            response = new ResponseObject
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

    #region View Model

    [Validator(typeof (UserModelValidator))]
    public class UserCreateModel : IAsyncRequest<ResponseObject>
    {
        public string UserName { get; set; }
        public string Password { get; set; }
    }

    #endregion

    #region Validation

    public class UserModelValidator : AbstractValidator<UserCreateModel>
    {
        public UserModelValidator()
        {
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