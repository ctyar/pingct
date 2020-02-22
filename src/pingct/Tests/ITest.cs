using System.Threading.Tasks;

namespace Ctyar.Pingct.Tests
{
    internal interface ITest
    {
        Task<bool> RunAsync();

        void Report();
    }
}