

namespace CloudAccounting.Application.UseCases.DeleteCompany
{
    public class DeleteCompanyCommand : ICommand<MediatR.Unit>
    {
        public int CompanyCode { get; set; }
    }
}