namespace Gruppeportalen.Services.Interfaces;

public interface IGenerateMemberList
{
    public byte[] GenerateActiveMembershipsCsv(Guid localgroupid);
    bool IsMembershipListEmpty(Guid localgroupid);
}