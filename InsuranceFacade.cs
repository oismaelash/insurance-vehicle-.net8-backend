using AutoMapper;
using CF.Insurance.Application.Dtos;
using CF.Insurance.Application.Facades.Interfaces;
using CF.Insurance.Domain.Models;
using CF.Insurance.Domain.Services.Interfaces;

namespace CF.Insurance.Application.Facades;

public class InsuranceFacade(IInsuranceService customerService, IMapper mapper) : IInsuranceFacade
{
    public async Task<long> CreateAsync(InsuranceRequestDto customerRequestDto, CancellationToken cancellationToken)
    {
        var customer = mapper.Map<Domain.Entities.Insurance>(customerRequestDto);

        var id = await customerService.CreateAsync(customer, cancellationToken);

        return id;
    }
}