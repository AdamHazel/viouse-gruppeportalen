using Gruppeportalen.HelperClasses;
using Gruppeportalen.Services.Interfaces;

namespace Gruppeportalen.Services.Classes;

public class NorwayCountryInformation : INorwayCountryInformation
{
    public List<string> GetAllCounties()
    {
        return new List<string>(Constants.Counties);
    }
}