namespace CloudAccounting.Application.UseCases.VoucherTypes.UpdateVoucherType
{
    public class UpdateVoucherTypeCommandValidator : AbstractValidator<UpdateVoucherTypeCommand>
    {
        private readonly IVoucherTypeRepository _repository;

        public UpdateVoucherTypeCommandValidator(IVoucherTypeRepository repository)
        {
            _repository = repository;

            RuleFor(voucherType => voucherType.VoucherCode)
                                            .GreaterThan(0).WithMessage("Missing voucher code.")
                                            .MustAsync(ValidateVoucherCode).WithMessage("The voucher code is not valid.");

            RuleFor(voucherType => voucherType.VoucherType)
                                            .NotEmpty().WithMessage("Missing voucher type.")
                                            .NotNull().WithMessage("Missing voucher type.")
                                            .MaximumLength(6).WithMessage("Voucher type cannot exceed six characters.")
                                            .MustAsync(IsUniqueVoucherTypeNameForUpdate).WithMessage("This voucher type already exists.");

            RuleFor(voucherType => voucherType.VoucherTitle)
                                            .NotEmpty().WithMessage("Missing voucher title.")
                                            .NotNull().WithMessage("Missing voucher title.")
                                            .MaximumLength(10).WithMessage("Voucher title cannot exceed 10 characters.");

            RuleFor(voucherType => voucherType.VoucherClassification)
                                            .NotNull().WithMessage("Missing voucher classification.")
                                            .InclusiveBetween((byte)1, (byte)3).WithMessage("Voucher classification must be between 1 and 3.");

        }

        private async Task<bool> IsUniqueVoucherTypeNameForUpdate(UpdateVoucherTypeCommand command, string? voucherTypeName, CancellationToken cancellationToken)
        {
            Result<bool> result = await _repository.IsUniqueVoucherTypeNameForUpdate(command.VoucherCode, voucherTypeName!);

            return result.IsSuccess && result.Value;
        }

        private async Task<bool> ValidateVoucherCode(int voucherCode, CancellationToken cancellationToken)
        {
            Result<bool> result = await _repository.IsValidVoucherCode(voucherCode);

            return result.Value;
        }
    }
}