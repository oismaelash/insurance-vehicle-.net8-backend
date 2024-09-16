namespace CF.Insurance.Application.Dtos;

public record InsuranceResponseDto
{
    public long Id { get; set; }
    public long VehicleId { get; set; }
    public float RiskRate { get; set; }
    public float RiskPremium { get; set; }
    public float PurePremium { get; set; }
    public float CommercialPremium { get; set; }
    public float InsuranceValue { get; set; }
}