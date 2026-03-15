

namespace CloudAccounting.Application.UseCases.VoucherTypes.CreateVoucherType
{
    public class CreateVoucherTypeCommandValidator : AbstractValidator<CreateVoucherTypeCommand>
    {
        private readonly IVoucherTypeRepository _repository;

        public CreateVoucherTypeCommandValidator(IVoucherTypeRepository repository)
        {
            _repository = repository;

            // RuleFor(voucherType => voucherType.VoucherCode)
            //                                 .GreaterThan(0).WithMessage("Missing voucher code.");

            RuleFor(voucherType => voucherType.VoucherType)
                                            .NotEmpty().WithMessage("Missing voucher type.")
                                            .NotNull().WithMessage("Missing voucher type.")
                                            .MaximumLength(6).WithMessage("Voucher type cannot exceed 6 characters.")
                                            .MustAsync(IsUniqueVoucherTypeNameForCreate).WithMessage("This voucher type already exists.");

            RuleFor(voucherType => voucherType.VoucherTitle)
                                            .NotEmpty().WithMessage("Missing voucher title.")
                                            .NotNull().WithMessage("Missing voucher title.")
                                            .MaximumLength(30).WithMessage("Voucher title cannot exceed 30 characters.");

            RuleFor(voucherType => voucherType.VoucherClassification)
                                            .NotNull().WithMessage("Missing voucher classification.")
                                            .InclusiveBetween((byte)1, (byte)3).WithMessage("Voucher classification must be between 1 and 3.");

        }

        private async Task<bool> IsUniqueVoucherTypeNameForCreate(string? voucherTypeName, CancellationToken cancellationToken)
        {
            Result<bool> result = await _repository.IsUniqueVoucherTypeNameForCreate(voucherTypeName!);

            return result.IsSuccess && result.Value;
        }
    }
}