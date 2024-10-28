namespace HeadCode.Api.Endpoints.Problems.Add;

using FluentValidation;

public class AddProblemRequestValidator : AbstractValidator<AddProblemRequest>
{
    public AddProblemRequestValidator()
    {
        RuleFor(x => x.Title).NotEmpty()
                             .Length(0, 50);
        RuleFor(x => x.Description).NotEmpty()
                                   .Length(0, 1500);
    }
}