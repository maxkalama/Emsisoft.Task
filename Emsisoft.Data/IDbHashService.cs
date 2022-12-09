using Emsisoft.DB;

namespace Emsisoft.Data
{
    public interface IDbHashService
    {
        Task<Dictionary<DateOnly, int>> GetCountsAsync();
        Task<bool> TryInsertAsync(IEnumerable<Hash> hashesBatch);
    }
}