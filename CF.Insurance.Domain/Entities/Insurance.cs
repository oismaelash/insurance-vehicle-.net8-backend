namespace CF.Insurance.Domain.Entities;

public class Insurance
{
    public long Id { get; set; }
    public long VehicleId { get; set; }
    public DateTime Created { get; set; }
    public float RiskRate { get; set; }
    public float RiskPremium { get; set; }
    public float PurePremium { get; set; }
    public float CommercialPremium { get; set; }
    public float InsuranceValue { get; set; }
}