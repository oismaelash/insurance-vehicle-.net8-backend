using CF.Insurance.Domain.Repositories;
using CF.Insurance.Infrastructure.DbContext;
using CF.Insurance.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;

namespace CF.Insurance.Infrastructure.Repositories;

public class InsuranceRepository(InsuranceContext context)
    : RepositoryBase<Domain.Entities.Insurance>(context), IInsuranceRepository
{

}