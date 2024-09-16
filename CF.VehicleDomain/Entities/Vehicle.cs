namespace CF.Vehicle.Domain.Entities;

public class Vehicle
{
    public long Id { get; set; }
    public DateTime Created { get; set; }
    public string Value { get; set; }
    public string Brand { get; set; }
    public string Model { get; set; }
}