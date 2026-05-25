using System;

/// <summary>
/// PlayerObserverManager: Static Event Manager utilizando o padrão Observer.
/// Gerencia eventos estáticos relacionados ao jogador (moedas, pontos, etc).
/// Nenhuma classe precisa referenciar diretamente umas às outras — apenas se inscrevem no evento.
/// 
/// Exemplo de uso:
/// 
/// // Para se inscrever no evento de moedas:
/// PlayerObserverManager.OnCoinsChanged += HandleCoinsChanged;
/// 
/// // Para disparar o evento:
/// PlayerObserverManager.NotifyCoinsChanged(50);
/// 
/// // Para desinscrever (importante para evitar memory leaks):
/// PlayerObserverManager.OnCoinsChanged -= HandleCoinsChanged;
/// </summary>
public static class PlayerObserverManager
{
    /// <summary>
    /// Evento disparado quando a quantidade de moedas do jogador muda.
    /// Parâmetro: coinCount (int) — a nova quantidade de moedas.
    /// </summary>
    public static event Action<int> OnCoinsChanged;

    /// <summary>
    /// Dispara o evento OnCoinsChanged para notificar todos os observadores.
    /// </summary>
    /// <param name="coinCount">A nova quantidade de moedas do jogador.</param>
    public static void NotifyCoinsChanged(int coinCount)
    {
        OnCoinsChanged?.Invoke(coinCount);
    }
}

