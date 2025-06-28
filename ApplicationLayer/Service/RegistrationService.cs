using ApplicationLayer.Forms;
using ApplicationLayer.Interface;
using Domain.Model.DataModels;
using Domain.Security;
using FluentResults;
using Infrastructure.Helpers;
using Infrastructure.Repositories.Interface;
using System;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ApplicationLayer.Service
{
    public class RegistrationService
    {
        private readonly IStoredCredentialRepository _credentialRepository;
        private readonly IEmailService _emailService;
        private readonly IHasher _hasher;
        public RegistrationService(IStoredCredentialRepository storedCredentialRepository, IEmailService emailService, IHasher hasher)
        {
            _credentialRepository = storedCredentialRepository;
            _emailService = emailService;
            _hasher = hasher;
        }
        public async void Register(string email, string password)
        {
            var salt = SecurityHelper.GenerateSalt();
            var hash = SecurityHelper.HashPassword(password, salt);
            var token = Guid.NewGuid().ToString();

            var user = new StoredCredential
            {
                Id = Guid.NewGuid(),
                UserEmail = email,
                Salt = salt,
                PasswordHash = hash,
                IsEmailConfirmed = false,
                ConfirmationToken = token
            };

            _credentialRepository.Add(user);
            await _credentialRepository.SaveChangesAsync();

            string rawGuid = Guid.NewGuid().ToString("N"); // 32-character hex without dashes
            string confirmationCode = rawGuid.Substring(0, 6).ToUpper(); // E.g., "A2C9FD"

            user.SetConfirmationCode(confirmationCode, _hasher, TimeSpan.FromMinutes(15));
            await _emailService.SendConfirmationLinkAsync(user.UserEmail, confirmationCode);

            //After registration open the confirmation form
            using (var confirmForm = new ConfirmForm()) // TextBox + OK button
            {
                Result result = new Result();
                if (confirmForm.ShowDialog() == DialogResult.OK)
                {
                    var inputToken = confirmForm.txtTokenInput.Text;
                    result = await ConfirmEmail(token, inputToken);

                    if (result.IsSuccess)
                    {
                        MessageBox.Show(result.Successes.FirstOrDefault()?.Message ?? "Operation completed successfully!", "Success");
                    }
                    else
                    {
                        MessageBox.Show(result.Errors.FirstOrDefault()?.Message ?? "Something went wrong.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
                else
                {
                    //If the confirmation cancelled
                    MessageBox.Show(result.Errors.FirstOrDefault()?.Message ?? "User registered successfully, but cancelled the confirmation", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }

        }
        //Email code confirmation sent from the users email.
        public async Task<Result> ConfirmEmail(string token, string inputToken)
        {
            var tokenRetrieved = await _credentialRepository.GetByConfirmationToken(token);
            var user = await _credentialRepository.GetByEmail(tokenRetrieved.UserEmail);
            
            if (user == null)
                return Result.Fail("User not found");
            if (!user.IsValidConfirmationCode(inputToken, _hasher))
                return Result.Fail("Invalid or expired confirmation code");
            //Else success
            user.ConfirmEmail();
            await _credentialRepository.SaveChangesAsync();
            return Result.Ok();
        }
    }
}
