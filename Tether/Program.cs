using System.Threading.Tasks;

namespace Tether
{
    internal class Program
    {
        private static async Task Main(string[] args)
        {
            var testManager = new TestManager();

            await testManager.Scan();
        }
    }
}