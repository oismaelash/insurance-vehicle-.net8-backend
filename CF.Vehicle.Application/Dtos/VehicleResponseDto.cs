namespace CF.Insurance.Application.Dtos;

public record InsuranceResponseDto
{
    public long Id { get; set; }
    public string Value { get; set; }
    public string Brand { get; set; }
    public string Model { get; set; }
}