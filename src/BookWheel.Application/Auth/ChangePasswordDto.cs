using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookWheel.Application.Auth
{
    public class ChangePasswordDto
    {
        public string OldPassword { get; set; }
        public string NewPassword { get; set; }
    }

    public class ChangePasswordDtoValidator : AbstractValidator<ChangePasswordDto>
    {
        public ChangePasswordDtoValidator()
        {
            RuleFor(c=>c.OldPassword).NotEmpty().WithMessage("Old password should not be empty");
            RuleFor(c=>c.NewPassword).NotEmpty().WithMessage("New password should not be empty");
        }
    }

}
