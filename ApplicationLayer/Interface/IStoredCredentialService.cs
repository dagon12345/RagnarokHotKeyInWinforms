﻿using Domain.Model.DataModels;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace ApplicationLayer.Interface
{
    public interface IStoredCredentialService
    {
        Task SaveChangesAsync(StoredCredential storedCredential);
        Task<StoredCredential> FindCredential(string accessToken);
        Task<StoredCredential> SearchUser(string userEmail);
        Task SaveCredentials(StoredCredential credential);
    }
}
