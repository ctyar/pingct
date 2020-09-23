namespace Ctyar.Pingct
{
    internal class PanelConsoleManager : IConsoleManager
    {
        private readonly ReportPanel _reportPanel;

        public PanelConsoleManager(ReportPanel reportPanel)
        {
            _reportPanel = reportPanel;
        }

        public void Print(string message)
        {
            Print(message, MessageType.Info);
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

        public void PrintLine()
        {
            _reportPanel.Add();
        }

        public void Remove()
        {
            _reportPanel.Remove();
        }

        private void PrintSuccess(string message)
        {
            _reportPanel.Append($"[green]{message}[/]");
        }

        private void PrintFailure(string message)
        {
            _reportPanel.Append($"[red]{message}[/]");
        }

        private void PrintWarning(string message)
        {
            _reportPanel.Append($"[yellow]{message}[/]");
        }

        private void PrintInfo(string message)
        {
            _reportPanel.Append(message);
        }
    }
}