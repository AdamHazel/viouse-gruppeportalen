namespace Gruppeportalen.Services.Interfaces;

public interface IGenerateMemberList
{
    byte[] GenerateActiveMembershipsCsv(Guid localgroupid);
}