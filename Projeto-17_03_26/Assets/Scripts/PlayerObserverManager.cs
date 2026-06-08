using System;
using UnityEngine;

// Static Event Manager - gerencia eventos do jogador (moedas, pontos, etc)
public static class PlayerObserverManager
{
    // Evento disparado quando a quantidade de moedas muda
    public static event Action<int> OnCoinsChanged;

    // Evento disparado quando uma moeda é coletada (desacopla Coin do PlayerController)
    public static event Action<int> OnCoinCollected;

    // Notifica observadores sobre mudança de moedas
    public static void NotifyCoinsChanged(int coinCount)
    {
        Debug.Log($"[EVENT] OnCoinsChanged disparado com valor: {coinCount}");
        OnCoinsChanged?.Invoke(coinCount);
    }

    // Notifica observadores sobre coleta de moeda
    public static void NotifyCoinCollected(int coinValue)
    {
        Debug.Log($"[EVENT] OnCoinCollected disparado com valor: {coinValue}");
        OnCoinCollected?.Invoke(coinValue);
    }

    // Método legado - retorna 0 (estado mantido em PlayerController)
    [Obsolete("Use PlayerController para estado do jogador")]
    public static int GetTotalCoins()
    {
        return 0;
    }
}
