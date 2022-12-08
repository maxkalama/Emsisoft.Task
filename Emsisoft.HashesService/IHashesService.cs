using Emsisoft.Models;

namespace Emsisoft.HashesService
{
    public interface IHashesService
    {
        /// <summary>
        /// Gets random EmsisoftHash with provided or random date of current month.
        /// </summary>
        EmsisoftHash GetRandomHash(DateOnly? date = null);
        byte[] ToBinary(EmsisoftHash hash);
        EmsisoftHash FromBinary(byte[] bytes);

    }
}