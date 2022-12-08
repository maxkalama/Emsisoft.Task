using Emsisoft.Models;
using System.Security.Cryptography;
using System.Text;

namespace Emsisoft.HashesService
{
    public class Sha1HashesService : IHashesService
    {
        const int Sha1Length = 20;
        private Random _random;

        public Sha1HashesService()
        {
            _random = new Random();

        }
        public EmsisoftHash FromBinary(byte[] bytes)
        {
            EmsisoftHash hash = new EmsisoftHash();
            hash.Hash = bytes[..Sha1Length];
            string dateString = Encoding.UTF8.GetString(bytes[Sha1Length..]);

            if (DateTime.TryParse(dateString, out var dateTime))
                hash.Date = DateOnly.FromDateTime(dateTime);
            else
                throw new Exception($"Can't parse dateString '{dateString}'");

            return hash;
        }

        public byte[] ToBinary(EmsisoftHash hash)
        {
            var dateBytes = Encoding.UTF8.GetBytes(hash.Date.ToString());
            byte[] result = new byte[Sha1Length + dateBytes.Length];
            Array.Copy(hash.Hash, result, Sha1Length);
            Array.Copy(dateBytes, 0, result, Sha1Length, dateBytes.Length);

            return result;
        }

        public EmsisoftHash GetRandomHash(DateOnly? date = null)
        {
            EmsisoftHash hash = new ();
            hash.Date = date ?? GetDate();
            byte[] bytes = BitConverter.GetBytes(hash.Date.DayNumber);
            hash.Hash = SHA1.HashData(bytes);

            return hash;
        }

        private DateOnly GetDate()
        {
            var now = DateTime.UtcNow;
            var daysInMonth = DateTime.DaysInMonth(now.Year, now.Month);
            var randomDay = _random.Next(1,daysInMonth);

            return new DateOnly(now.Year, now.Month, randomDay);
        }
    }
}