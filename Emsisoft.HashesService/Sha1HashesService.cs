using Emsisoft.Models;
using System.Security.Cryptography;

namespace Emsisoft.HashesService
{
    public class Sha1HashesService : IHashesService
    {
        public EmsisoftHash GetRandomHash(DateOnly? date = null)
        {
            EmsisoftHash hash = new ();
            hash.Date = date ?? DateOnly.FromDateTime(DateTime.Today);
            byte[] bytes = BitConverter.GetBytes(hash.Date.DayNumber);
            hash.Hash = SHA1.HashData(bytes);

            return hash;
        }
    }
}