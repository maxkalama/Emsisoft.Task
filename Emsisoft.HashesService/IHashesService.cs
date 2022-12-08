using Emsisoft.Models;
using System.Security.Cryptography;

namespace Emsisoft.HashesService
{
    public interface IHashesService
    {
        /// <summary>
        /// Gets random EmsisoftHash with provided or current date.
        /// </summary>
        EmsisoftHash GetRandomHash(DateOnly? date = null);
    }
}