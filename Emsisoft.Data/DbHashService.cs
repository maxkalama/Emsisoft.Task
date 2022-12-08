using Emsisoft.DB;

namespace Emsisoft.Data
{
    public class DbHashService : IDbHashService
    {
        public bool TryInsert(IEnumerable<Hash> hashesBatch)
        {
            using var context = new HashesContext();
            context.Hashes.AddRange(hashesBatch);
            try
            {
                context.SaveChanges();
                return true;
            }
            catch
            {
                return false;
            }
        }

        public Dictionary<DateOnly, int> GetCounts()
        {
            using var context = new HashesContext();
            var hashes = context.Hashes.GroupBy(h => h.Date)
                .Select(g => new { g.Key, Count = g.Count() })
                .ToDictionary(g => g.Key, g => g.Count);
            return hashes;
        }
    }
}