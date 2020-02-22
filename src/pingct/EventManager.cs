namespace Ctyar.Pingct
{
    internal class EventManager
    {
        private readonly ProcessManager _processManager;
        private readonly string _onConnectedCommand;
        private readonly string _onConnectedCommandArgs;
        private readonly string _onDisconnectedCommand;
        private readonly string _onDisconnectedCommandArgs;

        public EventManager(ProcessManager processManager, Settings settings)
        {
            _processManager = processManager;
            _onConnectedCommand = settings.OnConnected;
            _onConnectedCommandArgs = settings.OnConnectedArgs;
            _onDisconnectedCommand = settings.OnDisconnected;
            _onDisconnectedCommandArgs = settings.OnDisconnectedArgs;
        }

        public void Connected()
        {
            _processManager.Execute(_onConnectedCommand, _onConnectedCommandArgs);
        }

        public void Disconnected()
        {
            _processManager.Execute(_onDisconnectedCommand, _onDisconnectedCommandArgs);
        }
    }
}