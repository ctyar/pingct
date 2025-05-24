namespace Ctyar.Pingct;

internal class EventManager
{
    private static readonly ProcessManager ProcessManager = new();
    private readonly string _onConnectedCommand;
    private readonly string _onConnectedCommandArgs;
    private readonly string _onDisconnectedCommand;
    private readonly string _onDisconnectedCommandArgs;

    public EventManager(Settings settings)
    {
        _onConnectedCommand = settings.OnConnected;
        _onConnectedCommandArgs = settings.OnConnectedArgs;
        _onDisconnectedCommand = settings.OnDisconnected;
        _onDisconnectedCommandArgs = settings.OnDisconnectedArgs;
    }

    public void Connected()
    {
        ProcessManager.Execute(_onConnectedCommand, _onConnectedCommandArgs);
    }

    public void Disconnected()
    {
        ProcessManager.Execute(_onDisconnectedCommand, _onDisconnectedCommandArgs);
    }
}