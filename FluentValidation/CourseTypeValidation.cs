using FluentValidation;
using GraphQLDemo.Schema.Queries;

namespace GraphQLDemo.FluentValidation
{
    public class CourseTypeValidation : AbstractValidator<string>
    {
        public CourseTypeValidation()
        {
            RuleFor(name => name)
                .NotEmpty().MinimumLength(3)
                .MaximumLength(50)
                .WithMessage("Error occured  name length must be within 3 to 50 Range")
                .WithErrorCode("Course_Name_Length");
        }
    }
}
