using FluentValidation;

namespace FastDrive.Models.Validators
{
    public class CarValidator : AbstractValidator<Car>
    {
        public CarValidator()
        {
            RuleFor(c => c.Model).NotEmpty().Length(6);
            RuleFor(c => c.Patent).NotEmpty();
            RuleFor(c => c.Km).NotEmpty();
            RuleFor(c => c.Brand).NotEmpty();

        }
    }
}
