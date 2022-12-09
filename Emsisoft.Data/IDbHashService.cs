using Emsisoft.DB;

namespace Emsisoft.Data
{
    public interface IDbHashService
    {
        Dictionary<DateOnly, int> GetCounts();
        Task<bool> TryInsertAsync(IEnumerable<Hash> hashesBatch);
    }
}