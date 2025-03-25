using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StarWars.ConsoleClient;

public class CharacterDto
{
    public string Name { get; set; }
    public string Height { get; set; }
    public string Mass { get; set; }
}

public class FavoriteCharacterDto
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Height { get; set; }
    public string Mass { get; set; }
}

public class RequestHistoryDto
{
    public int Id { get; set; }
    public string Path { get; set; }
    public string Method { get; set; }
    public DateTime RequestedAt { get; set; }
}