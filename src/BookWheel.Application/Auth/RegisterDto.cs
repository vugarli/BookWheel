using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookWheel.Application.Auth
{
    public class RegisterDto
    {
        public string DisplayName { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public string Password { get; set; }
        public bool IsCustomer { get; set; }
    }

    public class RegisterDtoValidator : AbstractValidator<RegisterDto>
    {
        public RegisterDtoValidator()
        {
            RuleFor(c=>c.DisplayName).NotEmpty().WithMessage("Provide displayname");
            RuleFor(c=>c.Email).NotEmpty().WithMessage("Provide displayname");
            RuleFor(c=>c.Email).EmailAddress().WithMessage("Email address invalid");
            RuleFor(c=>c.PhoneNumber)
                .Matches(@"^[\\+]?[(]?[0-9]{3}[)]?[-\\s\\.]?[0-9]{3}[-\\s\\.]?[0-9]{4,6}$")
                .WithMessage("PhoneNumber invalid");
            RuleFor(c=>c.Password).NotEmpty().WithMessage("Password invalid");
        }
    }

}
