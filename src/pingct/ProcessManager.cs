using System.Diagnostics;

namespace Ctyar.Pingct;

internal class ProcessManager
{
    public void Execute(string command, string arguments)
    {
        if (string.IsNullOrEmpty(command))
        {
            return;
        }

        using var process = new Process
        {
            StartInfo = new ProcessStartInfo
            {
                FileName = command,
                Arguments = arguments,
                CreateNoWindow = true
            }
        };

        process.Start();
    }
}
