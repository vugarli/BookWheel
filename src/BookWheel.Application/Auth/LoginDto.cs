using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookWheel.Application.Auth
{
    public class LoginDto
    {
        public string Email { get; set; }
        public string Password { get; set; }
        public bool IsCustomer { get; set; }
    }

    public class LoginDtoValidator : AbstractValidator<LoginDto>
    {
        public LoginDtoValidator()
        {
            RuleFor(c=>c.Email).EmailAddress().WithMessage("Invalid Email address.");
            RuleFor(c=>c.Password).NotEmpty().WithMessage("Invalid Password.");
        }
    }

}
