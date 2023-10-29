using RunnerUp.Models;

namespace RunnerUp.ViewModels;

public class HomeViewModel
{
    public IEnumerable<Club> Clubs { get; set; }
    public string City { get; set; }
    public string State { get; set; }
}