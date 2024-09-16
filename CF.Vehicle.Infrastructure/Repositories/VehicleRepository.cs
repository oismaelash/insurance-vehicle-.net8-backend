using CF.Vehicle.Domain.Repositories;
using CF.Vehicle.Infrastructure.DbContext;
using CF.Vehicle.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;

namespace CF.Vehicle.Infrastructure.Repositories;

public class VehicleRepository(VehicleContext context)
    : RepositoryBase<Domain.Entities.Vehicle>(context), IVehicleRepository
{

}