using CloudAccounting.Shared.VoucherType;

namespace CloudAccounting.Application.UseCases.VoucherTypes.CreateVoucherType
{
    public class CreateVoucherTypeCommand : ICommand<VoucherTypeDto>
    {
        public int VoucherCode { get; set; }

        public string? VoucherType { get; set; }

        public string? VoucherTitle { get; set; }

        public byte? VoucherClassification { get; set; }
    }
}