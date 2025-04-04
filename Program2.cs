

using System;
using System.Collections.Generic;

// SINGLETON
public class EventManager
{
    private static EventManager _instance;
    public static EventManager Instance => _instance ??= new EventManager();

    private EventManager() { }

    public event Action<string> OnEventTriggered;
    
    public void TriggerEvent(string eventName)
    {
        Console.WriteLine($"Evento activado: {eventName}");
        OnEventTriggered?.Invoke(eventName);
    }
}

//  OBSERVER 
public interface IObserver
{
    void OnEventReceived(string eventName);
}

public class Player : IObserver
{
    public string Name { get; }
    
    public Player(string name)
    {
        Name = name;
        EventManager.Instance.OnEventTriggered += OnEventReceived;
    }
    
    public void OnEventReceived(string eventName)
    {
        Console.WriteLine($"{Name} recibió el evento: {eventName}");
    }
}

// DELEGADOS 
public class GameActions
{
    public delegate void CustomAction(string message);
    public static void ExecuteCustomAction(CustomAction action, string message)
    {
        action?.Invoke(message);
    }
}

// DEMOSTRACIÓN EN CONSOLA
class Program
{
    static void Main()
    {
        Player player1 = new Player("Alice");
        Player player2 = new Player("Bob");
        
        EventManager.Instance.TriggerEvent("GameStarted");
        
        GameActions.ExecuteCustomAction(msg => Console.WriteLine($"Acción personalizada: {msg}"), "El juego ha comenzado");
    }
}



modifica este codigo para implementar estos patrones