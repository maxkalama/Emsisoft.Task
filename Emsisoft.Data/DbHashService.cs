using Emsisoft.DB;
using Microsoft.EntityFrameworkCore;

namespace Emsisoft.Data
{
    public class DbHashService : IDbHashService
    {
        public async Task<bool> TryInsertAsync(IEnumerable<Hash> hashesBatch)
        {
            using var context = new HashesContext();
            context.Hashes.AddRange(hashesBatch);
            try
            {
                await context.SaveChangesAsync();
                return true;
            }
            catch
            {
                return false;
            }
        }

        public async Task<Dictionary<DateOnly, int>> GetCountsAsync()
        {
            using var context = new HashesContext();
            var hashes = await context.Hashes.GroupBy(h => h.Date)
                .Select(g => new { g.Key, Count = g.Count() })
                .OrderByDescending(k => k.Key)
                .ToDictionaryAsync(g => g.Key, g => g.Count);
            return hashes;
        }
    }
}