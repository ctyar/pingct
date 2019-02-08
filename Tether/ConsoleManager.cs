using System;

namespace Tether
{
    internal class ConsoleManager
    {
        private const string Indentation = "    ";

        public void PrintLine()
        {
            Console.WriteLine();
        }

        public void Print(string message, MessageType messageType)
        {
            var finalMessage = Indentation + message;

            if (messageType == MessageType.Info)
            {
                PrintInfo(finalMessage);
            }
            else if (messageType == MessageType.Success)
            {
                PrintSuccess(finalMessage);
            }
            else if (messageType == MessageType.Warning)
            {
                PrintWarning(finalMessage);
            }
            else
            {
                PrintFailure(finalMessage);
            }
        }

        public void PrintResult(bool isSuccess, string successValue, string failValue)
        {
            if (isSuccess)
            {
                PrintSuccess(successValue);
            }
            else
            {
                PrintFailure(failValue);
            }

            PrintLine();
        }

        public void PrintPing(string ip, long time, long maxSuccessTime, long maxWarningTime)
        {
            PrintInfo($"Reply from {ip}: time=");

            PrintPingValue(time, maxSuccessTime, maxWarningTime);

            PrintInfo("ms");

            PrintLine();
        }

        private void PrintPingValue(long value, long maxSuccessValue, long maxWarningValue)
        {
            if (value == 0)
            {
                PrintFailure(value.ToString());
            }
            else if (value <= maxSuccessValue)
            {
                PrintSuccess(value.ToString());
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