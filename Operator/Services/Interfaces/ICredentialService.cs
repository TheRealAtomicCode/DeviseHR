﻿using Models;
using OP.DTO.Inbound;
using OP.DTO.Outbound;

namespace OP.Services.Interfaces
{
    public interface ICredentialService
    {
        Task<LoginResponse> FindByCredentials(LoginRequest loginRequest);
        Task<Operator> FindAndRefreshOperatorById(int id);
        void IncrementLoginAttemt(Operator op);
    }
}
