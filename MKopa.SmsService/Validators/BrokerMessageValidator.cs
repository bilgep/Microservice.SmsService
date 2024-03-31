using FluentValidation;
using MKopa.Common.BrokerContracts;

namespace MKopa.SmsService.Validators
{
    public class BrokerMessageValidator : AbstractValidator<BrokerMessage>
    {
        public BrokerMessageValidator() 
        {
            RuleFor(message => message.Id)
                .NotEmpty()
                .WithMessage($"{nameof(BrokerMessage)} Id is invalid");
            RuleFor(message => message.PhoneNumber)
                .Length(11)
                .NotEmpty()
                .WithMessage($"{nameof(BrokerMessage)} Phone Number is invalid");
            RuleFor(message => message.SmsText)
                .NotEmpty()
                .MaximumLength(160)
                .WithMessage($"{nameof(BrokerMessage)} Sms Text is invalid");
            RuleFor(message => message.CountryCode)
                .NotEmpty()
                .MinimumLength(2)
                .WithMessage($"{nameof(BrokerMessage)} Country Code is invalid");
        }
    }
}
