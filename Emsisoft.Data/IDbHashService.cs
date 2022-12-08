using Emsisoft.DB;

namespace Emsisoft.Data
{
    public interface IDbHashService
    {
        Dictionary<DateOnly, int> GetCounts();
        bool TryInsert(IEnumerable<Hash> hashesBatch);
    }
}