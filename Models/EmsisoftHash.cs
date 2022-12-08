using System.Security.Cryptography;

namespace Emsisoft.Models
{
    public class EmsisoftHash
    {
        public byte[] Hash { get; set; }
        public DateOnly Date { get; set; }
    }
}