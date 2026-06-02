using System;
using UnityEngine;

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
/// // Para adicionar moedas:
/// PlayerObserverManager.AddCoins(1);
/// 
/// // Para desinscrever (importante para evitar memory leaks):
/// PlayerObserverManager.OnCoinsChanged -= HandleCoinsChanged;
/// </summary>
public static class PlayerObserverManager
{
    /// <summary>
    /// Evento disparado quando a quantidade de moedas do jogador muda.
    /// Parâmetro: coinCount (int) — a nova quantidade total de moedas.
    /// </summary>
    public static event Action<int> OnCoinsChanged;
    /// <summary>
    /// Dispara o evento OnCoinsChanged para notificar todos os observadores.
    /// (Método mantido para compatibilidade com código legado)
    /// </summary>
    /// <param name="coinCount">A nova quantidade de moedas do jogador.</param>
    public static void NotifyCoinsChanged(int coinCount)
    {
        Debug.Log($"[EVENT] OnCoinsChanged disparado com valor: {coinCount}");
        OnCoinsChanged?.Invoke(coinCount);
    }

    /// <summary>
    /// OBS: Este método existe apenas para compatibilidade com código legado
    /// que pode chamar PlayerObserverManager.GetTotalCoins().
    /// Como este manager agora é apenas um canal de eventos (não guarda estado),
    /// retornamos 0 aqui. O estado deve ser mantido pelo PlayerController.
    /// </summary>
    [Obsolete("GetTotalCoins is deprecated. PlayerObserverManager no longer stores state; use PlayerController for player-specific state.")]
    public static int GetTotalCoins()
    {
        return 0;
    }
}
