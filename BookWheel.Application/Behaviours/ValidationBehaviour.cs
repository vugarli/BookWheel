using Azure.Core;
using BookWheel.Application.Exceptions;
using FluentValidation;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookWheel.Application.Behaviours
{
    public class ValidationBehaviour<TReq, TRes> : IPipelineBehavior<TReq, TRes>
    {
        public IEnumerable<IValidator<TReq>> _validators { get; }
        public ValidationBehaviour(IEnumerable<IValidator<TReq>> validators)
        {
            _validators = validators;
        }


        public async Task<TRes> Handle
            (
            TReq request,
            RequestHandlerDelegate<TRes> next,
            CancellationToken cancellationToken
            )
        {
            var context = new ValidationContext<TReq>(request);

            var validationResults = await Task.WhenAll(_validators
                .Select(v => v.ValidateAsync(context, cancellationToken)));

            var errors = validationResults
                .Where(v => !v.IsValid)
                .SelectMany(res => res.Errors)
                .GroupBy(f => f.PropertyName);

            if (errors.Any())
            {
                var validationEx = new ModelValidationException();

                foreach (var error in errors)
                {
                    foreach (var errorDetail in error.ToList())
                        validationEx.UpsertDataList(error.Key, errorDetail.ErrorMessage);
                }

                throw validationEx;
            }

            return await next();
        }
    }
}
