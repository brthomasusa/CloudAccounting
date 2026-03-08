
namespace CloudAccounting.Application.UseCases.VoucherTypes.DeleteVoucherType
{
    public record class DeleteVoucherTypeCommand(int VoucherCode) : ICommand<MediatR.Unit>;
}