public interface IEventAgregator
{
    void AddHandler<T>(IHandler<T> handler) where T : EventMessage;

    void RemoveHandler<T>(IHandler<T> handler) where T : EventMessage;

    void SendMessageAll<T>() where T : EventMessage, new();
        
    void SendMessageAll<T>(T message) where T : EventMessage;

    void SendMessageFirst<T>() where T : EventMessage, new();

    void SendMessageFirst<T>(T message) where T : EventMessage;
}
