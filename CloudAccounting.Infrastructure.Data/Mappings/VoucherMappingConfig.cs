using CloudAccounting.Infrastructure.Data.Models;
using CloudAccounting.Core.Models;

namespace CloudAccounting.Infrastructure.Data.Mappings
{
    public class VoucherMappingConfig : IRegister
    {
        // TypeAdapterConfig<TSource, TDestination>()

        public void Register(TypeAdapterConfig config)
        {
            // Map Voucher domain model to Voucher data model
            config.NewConfig<Voucher, VoucherDM>()
                .Map(dest => dest.VoucherCode, src => src.VoucherCode)
                .Map(dest => dest.VoucherType, src => src.VoucherType)
                .Map(dest => dest.VoucherTitle, src => src.VoucherTitle)
                .Map(dest => dest.VoucherClassification, src => src.VoucherClassification);

            // Map Voucher data model to Voucher domain model
            config.NewConfig<VoucherDM, Voucher>()
                .Map(dest => dest.VoucherCode, src => src.VoucherCode)
                .Map(dest => dest.VoucherType, src => src.VoucherType)
                .Map(dest => dest.VoucherTitle, src => src.VoucherTitle)
                .Map(dest => dest.VoucherClassification, src => src.VoucherClassification);
        }
    }
}