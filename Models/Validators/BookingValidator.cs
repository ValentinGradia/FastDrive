using FluentValidation;

namespace FastDrive.Models.Validators
{
    public class BookingValidator : AbstractValidator<Booking>
    {

        public BookingValidator()
        {
            RuleFor(b => b.Status).Must(s => s is EBookingStatus).NotEmpty();
        }
    }
}
