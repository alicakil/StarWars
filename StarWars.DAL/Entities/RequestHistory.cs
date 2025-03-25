namespace StarWars.DAL.Entities;

public class RequestHistory
{
    public int Id { get; set; }
    public string Endpoint { get; set; } = string.Empty;
    public DateTime RequestedAt { get; set; }
}
