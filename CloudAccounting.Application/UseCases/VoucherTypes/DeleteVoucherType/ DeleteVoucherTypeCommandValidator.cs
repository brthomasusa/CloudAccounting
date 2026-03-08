

namespace CloudAccounting.Application.UseCases.VoucherTypes.DeleteVoucherType
{
    public class DeleteVoucherTypeCommandValidator : AbstractValidator<DeleteVoucherTypeCommand>
    {
        private readonly IVoucherTypeRepository _repository;

        public DeleteVoucherTypeCommandValidator(IVoucherTypeRepository repository)
        {
            _repository = repository;

            RuleFor(voucherType => voucherType.VoucherCode)
                                            .GreaterThan(0).WithMessage("Missing voucher code.")
                                            .MustAsync(ValidateVoucherCode).WithMessage("The voucher code is not valid.");
        }

        private async Task<bool> ValidateVoucherCode(int voucherCode, CancellationToken cancellationToken)
        {
            Result<bool> result = await _repository.IsValidVoucherCode(voucherCode);

            return result.Value;
        }
    }
}