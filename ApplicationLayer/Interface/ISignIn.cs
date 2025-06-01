using Domain.Model.DataModels;
using System;
using System.Threading.Tasks;

namespace ApplicationLayer.Interface
{
    public interface ISignIn
    {
        Task<string> SearchExistingUser(string ReferenceCode);
        Task CreateUser(BaseTable baseTable, string email);
    }
}
