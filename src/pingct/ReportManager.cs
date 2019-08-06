using System;

namespace Ctyar.Pingct
{
    internal class ReportManager : IReportManager
    {
        public void PrintLine()
        {
            Console.WriteLine();
        }

        public void Print(string message, MessageType messageType)
        {
            if (messageType == MessageType.Info)
            {
                PrintInfo(message);
            }
            else if (messageType == MessageType.Success)
            {
                PrintSuccess(message);
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

        private static void PrintSuccess(string message)
        {
            Console.ForegroundColor = ConsoleColor.Green;
            Console.Write(message);

            Console.ResetColor();
        }

        private static void PrintFailure(string message)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.Write(message);

            Console.ResetColor();
        }

        private static void PrintWarning(string message)
        {
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.Write(message);

            Console.ResetColor();
        }

        private static void PrintInfo(string message)
        {
            Console.Write(message);
        }
    }
}