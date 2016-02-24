using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using AutoMapper;
using Common;
using Contracts;
using MediatR;

namespace WebApi_Autofac.Handlers.Features.User
{
    public class GetAllUserHandler : IRequestHandler<GetAllUserRequest, ResponseObject>
    {
        private IUow Uow;

        public ResponseObject Handle(GetAllUserRequest message)
        {
            var result = new Logic.User(Uow).GetAll();
            if (result == null) return null;
            var dest = result.Select(Mapper.Map<GetAllUserResponse>).ToList();
            var response = new ResponseObject
            {
                ResponseMessage = new HttpResponseMessage(HttpStatusCode.OK),
                Data = dest,
                Message = "Users retrieved Successfully",
                IsSuccessful = true
            };
            return response;
        }
    }

    public class GetAllUserRequest : IRequest<ResponseObject>
    {
    }

    public class GetAllUserResponse : IRequest<ResponseObject>
    {
        public string UserName { get; set; }
    }
}