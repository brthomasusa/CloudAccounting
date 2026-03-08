namespace CloudAccounting.Application.UseCases.Companies.DeleteCompany;

    public class DeleteCompanyCommand : ICommand<MediatR.Unit>
    {
        public int CompanyCode { get; set; }
    }
