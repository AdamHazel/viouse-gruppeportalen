using NuGet.Protocol.Plugins;

namespace Gruppeportalen.Services.Interfaces;

public interface IIdGeneratorService
{
    string GenerateId();
}