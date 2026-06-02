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
    /// Total acumulado de moedas do jogador.
    /// STATIC: persiste durante toda a execução do jogo.
    /// IMPORTANTE: Esta variável é estática e mantém seu valor entre chamadas.
    /// </summary>
    private static int _totalCoins = 0;

    /// <summary>
    /// Evento disparado quando a quantidade de moedas do jogador muda.
    /// Parâmetro: coinCount (int) — a nova quantidade total de moedas.
    /// </summary>
    public static event Action<int> OnCoinsChanged;

    /// <summary>
    /// Adiciona moedas ao total acumulado e dispara o evento com o novo total.
    /// Usa o operador += para garantir acúmulo correto.
    /// </summary>
    /// <param name="value">O valor a adicionar ao total de moedas.</param>
    public static void AddCoins(int value)
    {
        if (value < 0)
        {
//            Debug.LogWarning($"PlayerObserverManager.AddCoins(): valor negativo ({value}) rejeitado.");
            return;
        }

        if (value == 0)
        {
//            Debug.LogWarning($"PlayerObserverManager.AddCoins(): tentativa de adicionar 0 moedas.");
            return;
        }

        // Registra o valor anterior para debug
        int previousTotal = _totalCoins;
        
        // Operador += garante acúmulo correto (não sobrescreve, adiciona)
        _totalCoins += value;

        Debug.Log($"[COINS] Moeda(s) adicionada(s): +{value} | Total anterior: {previousTotal} | Total atual: {_totalCoins}");
        
        // Dispara o evento com o novo total ACUMULADO
        NotifyCoinsChanged(_totalCoins);
    }

    /// <summary>
    /// Retorna o total acumulado de moedas.
    /// </summary>
    public static int GetTotalCoins()
    {
        return _totalCoins;
    }

    /// <summary>
    /// Reseta o contador de moedas para 0.
    /// Útil ao trocar de cena ou reiniciar fase.
    /// </summary>
    public static void ResetCoins()
    {
//        Debug.Log($"[COINS] Resetando contador. Total anterior: {_totalCoins} → Total novo: 0");
        _totalCoins = 0;
        NotifyCoinsChanged(0);
    }

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
}
