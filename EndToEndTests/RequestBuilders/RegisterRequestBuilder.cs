using BookWheel.Application.Auth;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EndToEndTests.RequestBuilders
{
    public class RegisterRequestBuilder
    {
        public string DisplayName { get; set; } = Guid.NewGuid().ToString();
        public string Email { get; set; } = $"{Guid.NewGuid().ToString()}@test.com";
        public string Password { get; set; } = Guid.NewGuid().ToString();
        public bool IsCustomer { get; set; }

        public RegisterRequestBuilder WithEmail(string email)
        {
            Email = email;
            return this;
        }
        
        public RegisterRequestBuilder WithPassword(string password)
        {
            Password = password;
            return this;
        }

        public RegisterRequestBuilder WithDisplayname(string displayName)
        {
            DisplayName = displayName;
            return this;
        }

        public RegisterRequestBuilder WithCustomer()
        {
            IsCustomer = true;
            return this;
        }

        public RegisterRequestBuilder WithOwner()
        {
            IsCustomer = false;
            return this;
        }


        public RegisterDto Build()
        {
            return new RegisterDto()
            {
                Email = Email,
                Password = Password,
                DisplayName = DisplayName,
                IsCustomer = IsCustomer
            };
        }






    }
}
