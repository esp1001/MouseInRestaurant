public interface IHandler<in T> where T : EventMessage
{
    void Handle(T message);
}
