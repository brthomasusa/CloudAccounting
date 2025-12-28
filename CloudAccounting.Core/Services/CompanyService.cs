using System.ComponentModel;
using System.Runtime.CompilerServices;
using CloudAccounting.Core.Models;
using CloudAccounting.Core.Repositories;

namespace CloudAccounting.Core.Services;


public class CompanyService(ICompanyRepository repository)
{
    private readonly ICompanyRepository _repository = repository;

    public void AddFiscalYear()
    {

    }
}
