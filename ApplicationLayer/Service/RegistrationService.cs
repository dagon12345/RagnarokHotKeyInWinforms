using ApplicationLayer.Interface;
using Domain.Model.DataModels;
using Domain.Security;
using FluentResults;
using Infrastructure.Helpers;
using System;
using System.Threading.Tasks;

namespace ApplicationLayer.Service
{
    public class RegistrationService : IRegistrationService
    {
        private readonly IStoredCredentialService _storedCredentialService;
        private readonly IEmailService _emailService;
        private readonly IHasher _hasher;
        private readonly ISignIn _signIn;
        public RegistrationService(IStoredCredentialService storedCredentialService, IEmailService emailService, IHasher hasher, ISignIn signIn)
        {
            _storedCredentialService = storedCredentialService;
            _emailService = emailService;
            _hasher = hasher;
            _signIn = signIn;
        }
        public async Task<StoredCredential> Register(string email, string password, string name)
        {
            var salt = SecurityHelper.GenerateSalt();
            var hash = SecurityHelper.HashPassword(password, salt);
            var token = Guid.NewGuid().ToString();

            //Search through stored credential database
            var searchUser = await _storedCredentialService.SearchUser(email);
            if (searchUser == null)
            {
                var user = new StoredCredential
                {
                    Id = Guid.NewGuid(),
                    Name = name,
                    UserEmail = email,
                    Salt = salt,
                    PasswordHash = hash,
                    IsEmailConfirmed = false,
                    ConfirmationToken = token,
                    LastLoginTime = DateTime.UtcNow,
                };

                await _storedCredentialService.SaveCredentials(user);


                string rawGuid = Guid.NewGuid().ToString("N"); // 32-character hex without dashes
                string confirmationCode = rawGuid.Substring(0, 6).ToUpper(); // E.g., "A2C9FD"

                user.SetConfirmationCode(confirmationCode, _hasher, TimeSpan.FromMinutes(15));
                //Send the email
                await _emailService.SendConfirmationLinkAsync(user.UserEmail, confirmationCode);
            }
            else
            {
                //Update the stored credential for new lastlogin and accesstoken
                searchUser.LastLoginTime = DateTime.UtcNow;
                await _storedCredentialService.SaveChangesAsync();
            }


            //Search existed user
            var searchExisitingUser = await _signIn.SearchExistingUser(email);
            if (searchExisitingUser == null)
            {
                //if not existed then add to database Email/ReferenceCode.
                BaseTable baseTable = new BaseTable
                {
                    ReferenceCode = Guid.NewGuid(),
                    Email = email,
                };
                await _signIn.CreateUser(baseTable);
            }
            return searchUser;
        }
        //Email code confirmation sent from the users email.
        public async Task<Result> ConfirmEmail(string email, string inputToken)
        {
            var user = await _storedCredentialService.SearchUser(email);
            if (user == null)
                return Result.Fail("User not found");
            if (!user.IsValidConfirmationCode(inputToken, _hasher))
                return Result.Fail("Invalid or expired confirmation code");
            //Else success
            user.ConfirmEmail();
            await _storedCredentialService.SaveChangesAsync();
            return Result.Ok();
        }
    }
}
