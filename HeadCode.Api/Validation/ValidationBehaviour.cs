namespace HeadCode.Api.Validation;

using System.Text;
using Core.Models;
using FluentValidation;
using FluentValidation.Results;
using MediatR;
using Microsoft.AspNetCore.Http.HttpResults;

public class ValidationBehaviour<TRequest, TResult> : IPipelineBehavior<TRequest, Results<TResult, BadRequest<string>>> where TRequest : notnull where TResult : IResult
{
    private readonly IValidator<TRequest> _validator;

    public ValidationBehaviour(IValidator<TRequest> validator)
    {
        _validator = validator;
    }

    public async Task<Results<TResult, BadRequest<string>>> Handle(TRequest request, RequestHandlerDelegate<Results<TResult, BadRequest<string>>> next, CancellationToken cancellationToken)
    {
        ValidationResult? validationResult = await _validator.ValidateAsync(request, cancellationToken);
        if (!validationResult.IsValid)
        {
            return TypedResults.BadRequest(GenerateErrorMessage(validationResult));
        }

        return await next.Invoke();
    }

    private string GenerateErrorMessage(ValidationResult validationResult)
    {
        StringBuilder stringBuilder = new StringBuilder();
        stringBuilder.AppendLine("Validation failed:");

        foreach (ValidationFailure failure in validationResult.Errors.ToList())
        {
            stringBuilder.AppendLine(failure.ErrorMessage);
        }
        
        return stringBuilder.ToString();
    }
}