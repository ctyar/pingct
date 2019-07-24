using System.Threading.Tasks;

namespace Tether
{
    internal interface ITest
    {
        Task<bool> Run();

        void Report();
    }
}