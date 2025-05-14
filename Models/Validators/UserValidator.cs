using FluentValidation;

namespace FastDrive.Models.Validators
{
    public class UserValidator : AbstractValidator<User>
    {
        public UserValidator()
        {
            RuleFor(u => u.Name).NotEmpty().Matches("^[a-zA-Z]+$");
            RuleFor(u => u.Surname).NotEmpty().Matches("^[a-zA-Z]+$");
            RuleFor(u => u.Age).GreaterThan(17).NotEmpty();
            RuleFor(u => u.DNI).NotEmpty();
            RuleFor(u => u.Email).EmailAddress().NotEmpty();
            //RuleFor(u => u.UserType).Must(e => e is EUserType).NotEmpty();

        }


        
    }
}
