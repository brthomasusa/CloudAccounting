using CloudAccounting.Shared.VoucherType;

namespace CloudAccounting.Application.UseCases.VoucherTypes.UpdateVoucherType
{
    public class UpdateVoucherTypeCommand : ICommand<VoucherTypeDto>
    {
        public int VoucherCode { get; set; }

        public string? VoucherType { get; set; }

        public string? VoucherTitle { get; set; }

        public byte? VoucherClassification { get; set; }
    }
}