using Emsisoft.Models;
using System.Security.Cryptography;

namespace Emsisoft.HashesService
{
    public interface IHashesService
    {
        EmsisoftHash GetRandomHash(DateOnly? date = null);
    }
}