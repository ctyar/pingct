using System;

namespace Tether
{
    internal class ConsoleReportManager : IReportManager
    {
        public void ReportResult(bool isSuccess)
        {
            if (isSuccess)
            {
                PrintSucess("succeed");
            }
            else
            {
                PrintFailure("failed");
            }
        }

        public void ReportValue(long value, long maxSuccessValue, long maxWarningValue)
        {
            if (value <= maxSuccessValue)
            {
                PrintSucess(value.ToString());
            }
            else if (value <= maxWarningValue)
            {
                PrintWarning(value.ToString());
            }
            else
            {
                PrintFailure(value.ToString());
            }
        }

        public void Report(string message, MessageType messageType)
        {
            if (messageType == MessageType.Info)
            {
                PrintInfo(message);
            }
            else if (messageType == MessageType.Sucess)
            {
                PrintSucess(message);
            }
            else if (messageType == MessageType.Warning)
            {
                PrintWarning(message);
            }
            else
            {
                PrintFailure(message);
            }
        }

        private static void PrintSucess(string message)
        {
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine(message);

            Console.ResetColor();
        }

        private static void PrintFailure(string message)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine(message);

            Console.ResetColor();
        }

        private static void PrintWarning(string message)
        {
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine(message);

            Console.ResetColor();
        }

        private static void PrintInfo(string message)
        {
            Console.WriteLine(message);
        }
    }
}