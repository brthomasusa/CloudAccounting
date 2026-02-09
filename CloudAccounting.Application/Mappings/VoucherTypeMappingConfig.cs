using CloudAccounting.Shared.VoucherType;

namespace CloudAccounting.Application.Mappings
{
    public class VoucherTypeMappingConfig : IRegister
    {
        // TypeAdapterConfig<TSource, TDestination>()

        public void Register(TypeAdapterConfig config)
        {
            // Map Voucher domain model to Voucher data model
            config.NewConfig<Voucher, VoucherTypeDto>()
                .Map(dest => dest.VoucherCode, src => src.VoucherCode)
                .Map(dest => dest.VoucherType, src => src.VoucherType)
                .Map(dest => dest.VoucherTitle, src => src.VoucherTitle)
                .Map(dest => dest.VoucherNature, src => src.VoucherNature);

            // Map Voucher domain model to Voucher data model

        }
    }
}