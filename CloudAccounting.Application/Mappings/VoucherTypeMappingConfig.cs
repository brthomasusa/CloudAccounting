using CloudAccounting.Shared.VoucherType;
// using CloudAccounting.Application.UseCases.VoucherType.CreateVoucherType;

namespace CloudAccounting.Application.Mappings
{
    public class VoucherTypeMappingConfig : IRegister
    {
        // TypeAdapterConfig<TSource, TDestination>()

        public void Register(TypeAdapterConfig config)
        {
            // Map CreateVoucherTyprCommand to Voucher domain model
            // config.NewConfig<CreateVoucherTypeCommand, Voucher>()
            //     .Map(dest => dest.VoucherCode, src => src.VoucherCode)
            //     .Map(dest => dest.VoucherType, src => src.VoucherType)
            //     .Map(dest => dest.VoucherTitle, src => src.VoucherTitle)
            //     .Map(dest => dest.VoucherClassification, src => src.VoucherClassification);

            // Map Voucher to VoucherTypeDto
            config.NewConfig<Voucher, VoucherTypeDto>()
                .Map(dest => dest.VoucherCode, src => src.VoucherCode)
                .Map(dest => dest.VoucherType, src => src.VoucherType)
                .Map(dest => dest.VoucherTitle, src => src.VoucherTitle)
                .Map(dest => dest.VoucherClassification, src => src.VoucherClassification);
        }
    }
}