using TMPro;
using UnityEngine;

/// <summary>
/// CoinHUD: Exibe a quantidade de moedas coletadas usando TextMeshPro.
/// Se inscreve no evento estático PlayerObserverManager.OnCoinsChanged para atualizar o texto em tempo real.
/// 
/// Requisitos:
/// - Este GameObject deve ter um componente TextMeshProUGUI (geralmente em um Canvas).
/// - O script deve ser atribuído a um GameObject que tenha acesso ao componente TextMeshProUGUI.
/// </summary>
public class CoinHUD : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI coinText;

    private void OnEnable()
    {
        // Se inscreve no evento de mudança de moedas
        PlayerObserverManager.OnCoinsChanged += OnCoinsChanged;
    }

    private void OnDisable()
    {
        // Se desinscreve do evento para evitar memory leaks
        PlayerObserverManager.OnCoinsChanged -= OnCoinsChanged;
    }

    /// <summary>
    /// Callback que é acionado quando o evento OnCoinsChanged é disparado.
    /// Atualiza o texto do HUD com a nova quantidade de moedas.
    /// </summary>
    /// <param name="coinCount">A quantidade de moedas coletadas.</param>
    private void OnCoinsChanged(int coinCount)
    {
        if (coinText != null)
        {
            coinText.text = $"Moedas: {coinCount}";
        }
        else
        {
            Debug.LogWarning("CoinHUD: Componente TextMeshProUGUI não foi atribuído no Inspector.");
        }
    }
}

