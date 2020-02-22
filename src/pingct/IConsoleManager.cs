namespace Ctyar.Pingct
{
    internal interface IConsoleManager
    {
        void Print(string message);

        void Print(string message, MessageType messageType);
        
        void PrintLine();
    }
}