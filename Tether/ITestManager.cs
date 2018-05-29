using System.Threading.Tasks;

namespace Tether
{
    internal interface ITestManager
    {
        Task Scan(Config config);
    }
}