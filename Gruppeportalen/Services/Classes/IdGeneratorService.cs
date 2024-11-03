using Gruppeportalen.Services.Interfaces;
using Microsoft.EntityFrameworkCore.ValueGeneration;

namespace Gruppeportalen.Services.Classes;

public class IdGeneratorService : IIdGeneratorService
{
    public string GenerateId()
    {
        return Guid.NewGuid().ToString();
    }
}