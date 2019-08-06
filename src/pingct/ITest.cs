using System.Threading.Tasks;

namespace Ctyar.Pingct
{
    internal interface ITest
    {
        Task<bool> Run();

        void Report();
    }
}