namespace Ctyar.Pingct
{
    internal class PanelManager
    {
        private readonly ReportPanel _reportPanel;

        public PanelManager(ReportPanel reportPanel)
        {
            _reportPanel = reportPanel;
        }

        public void Print(string message, MessageType messageType)
        {
            _reportPanel.Append(message, messageType);
        }

        public void PrintLine()
        {
            _reportPanel.Add();
        }

        public void Remove()
        {
            _reportPanel.Remove();
        }
    }
}