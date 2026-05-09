
using MediatR;

namespace CloudAccounting.Application.UseCases.VoucherTypes.DeleteVoucherType
{
    public record DeleteVoucherTypeCommand(int VoucherCode) : ICommand<Unit>;
}