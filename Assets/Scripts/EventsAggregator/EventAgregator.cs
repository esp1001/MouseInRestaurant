using System;
using System.Collections.Generic;

public class EventAgregator : IEventAgregator
{
    public static EventAgregator I { get; private set; }

    private readonly List<Object> handlers;

    public EventAgregator()
    {
        I = this;
        handlers = new List<Object>();
    }

    public void AddHandler<T>(IHandler<T> handler) where T: EventMessage
    {
        if(!handlers.Contains(handler)) handlers.Add(handler);    
    }
        
    public void RemoveHandler<T>(IHandler<T> handler) where T: EventMessage
        {
            if (handlers.Contains(handler)) handlers.Remove(handler);
        }

    public void SendMessageAll<T>() where T: EventMessage, new()
    {
        var h = handlers.FindAll(handler => handler is IHandler<T>);
        foreach (var handler in h) ((IHandler<T>)handler).Handle(new T());
    }

    public void SendMessageAll<T>(T message) where T : EventMessage
        {
            var h = handlers.FindAll(handler => handler is IHandler<T>);
            foreach (var handler in h) ((IHandler<T>)handler).Handle(message);
        }

    public void SendMessageFirst<T>() where T: EventMessage, new()
    {
        var h = handlers.Find(handler => handler is IHandler<T>);
        if(h == null) return;
        ((IHandler<T>)h).Handle(new T());
    }

    public void SendMessageFirst<T>(T message) where T : EventMessage
    {
        var h = handlers.Find(handler => handler is IHandler<T>);
        if(h == null) return;
        ((IHandler<T>)h).Handle(message);
    }
}
