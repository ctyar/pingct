namespace Ctyar.Pingct
{
    internal class EventManager
    {
        private readonly CommandManager _commandManager;
        private readonly string _onConnectedCommand;
        private readonly string _onConnectedCommandArgs;
        private readonly string _onDisconnectedCommand;
        private readonly string _onDisconnectedCommandArgs;

        public EventManager(CommandManager commandManager, Settings settings)
        {
            _commandManager = commandManager;
            _onConnectedCommand = settings.OnConnected;
            _onConnectedCommandArgs = settings.OnConnectedArgs;
            _onDisconnectedCommand = settings.OnDisconnected;
            _onDisconnectedCommandArgs = settings.OnDisconnectedArgs;
        }

        public void Connected()
        {
            _commandManager.Execute(_onConnectedCommand, _onConnectedCommandArgs);
        }

        public void Disconnected()
        {
            _commandManager.Execute(_onDisconnectedCommand, _onDisconnectedCommandArgs);
        }
    }
}