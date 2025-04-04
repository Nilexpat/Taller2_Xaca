using System;
using System.Collections.Generic;

//  SINGLETON 
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
    private IAttackStrategy _attackStrategy;

    public Player(string name, IAttackStrategy attackStrategy)
    {
        Name = name;
        _attackStrategy = attackStrategy;
        EventManager.Instance.OnEventTriggered += OnEventReceived;
    }

    public void OnEventReceived(string eventName)
    {
        Console.WriteLine($"{Name} recibió el evento: {eventName}");
        _attackStrategy.Attack();
    }
}

//  STRATEGY
public interface IAttackStrategy
{
    void Attack();
}

public class SwordAttack : IAttackStrategy
{
    public void Attack() => Console.WriteLine("Ataca con espada!");
}

public class BowAttack : IAttackStrategy
{
    public void Attack() => Console.WriteLine("Dispara una flecha!");
}

//  DECORATOR 
public interface IEnemy
{
    void Attack();
}

public class BasicEnemy : IEnemy
{
    public void Attack() => Console.WriteLine("Enemigo ataca con 10 de daño.");
}

public class ShieldedEnemy : IEnemy
{
    private IEnemy _enemy;
    public ShieldedEnemy(IEnemy enemy) => _enemy = enemy;
    public void Attack()
    {
        Console.WriteLine("¡Enemigo con escudo!");
        _enemy.Attack();
    }
}

//  ADAPTER 
public class OldSoundSystem
{
    public void PlayOldSound() => Console.WriteLine("Reproduciendo sonido en sistema antiguo...");
}

public class NewSoundSystem
{
    public void PlayNewSound() => Console.WriteLine("Reproduciendo sonido en sistema moderno...");
}

public class SoundAdapter : OldSoundSystem
{
    private NewSoundSystem _newSoundSystem = new NewSoundSystem();
    public void PlayOldSound() => _newSoundSystem.PlayNewSound();
}

//FACADE 
public class GameFacade
{
    private EventManager _eventManager = EventManager.Instance;
    private OldSoundSystem _soundSystem = new SoundAdapter();
    private IEnemy _enemy;

    public GameFacade()
    {
        _enemy = new ShieldedEnemy(new BasicEnemy());
        _eventManager.OnEventTriggered += (eventName) => _enemy.Attack();
    }

    public void StartGame()
    {
        Console.WriteLine("Iniciando juego...");
        _soundSystem.PlayOldSound();
        _eventManager.TriggerEvent("EnemyAttack");
    }
}

//  DELEGADOS
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
        Player player1 = new Player("Alice", new SwordAttack());
        Player player2 = new Player("Bob", new BowAttack());

        GameFacade game = new GameFacade();
        game.StartGame();

        GameActions.ExecuteCustomAction(msg => Console.WriteLine($"Acción personalizada: {msg}"), "El juego ha comenzado");
    }
}
