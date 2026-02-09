using VoucherDataModel = CloudAccounting.Infrastructure.Data.Models.Voucher;
using VoucherDomainModel = CloudAccounting.Core.Models.Voucher;

namespace CloudAccounting.Infrastructure.Data.Mappings
{
    public class VoucherMappingConfig : IRegister
    {
        // TypeAdapterConfig<TSource, TDestination>()

        public void Register(TypeAdapterConfig config)
        {
            // Map Voucher domain model to Voucher data model
            config.NewConfig<VoucherDomainModel, VoucherDataModel>()
                .Map(dest => dest.VoucherCode, src => src.VoucherCode)
                .Map(dest => dest.VoucherType, src => src.VoucherType)
                .Map(dest => dest.VoucherTitle, src => src.VoucherTitle)
                .Map(dest => dest.VoucherClassification, src => src.VoucherClassification);

            // Map Voucher domain model to Voucher data model
            config.NewConfig<VoucherDataModel, VoucherDomainModel>()
                .Map(dest => dest.VoucherCode, src => src.VoucherCode)
                .Map(dest => dest.VoucherType, src => src.VoucherType)
                .Map(dest => dest.VoucherTitle, src => src.VoucherTitle)
                .Map(dest => dest.VoucherClassification, src => src.VoucherClassification);
        }
    }
}