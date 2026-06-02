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

    private void Start()
    {
        // Exibe o total inicial ao iniciar
        int initialCoins = PlayerObserverManager.GetTotalCoins();
//        Debug.Log($"[HUD] CoinHUD inicializado. Moedas atuais: {initialCoins}");
        UpdateCoinDisplay(initialCoins);
    }

    private void OnEnable()
    {
        // Se inscreve no evento de mudança de moedas
        PlayerObserverManager.OnCoinsChanged += OnCoinsChanged;
//        Debug.Log("[HUD] CoinHUD: inscrição ao evento OnCoinsChanged realizada.");
    }

    private void OnDisable()
    {
        // Se desinscreve do evento para evitar memory leaks
        PlayerObserverManager.OnCoinsChanged -= OnCoinsChanged;
//        Debug.Log("[HUD] CoinHUD: desinscrição ao evento OnCoinsChanged realizada.");
    }

    /// <summary>
    /// Callback que é acionado quando o evento OnCoinsChanged é disparado.
    /// Atualiza o texto do HUD com a nova quantidade de moedas.
    /// </summary>
    /// <param name="coinCount">A quantidade acumulada de moedas.</param>
    private void OnCoinsChanged(int coinCount)
    {
//        Debug.Log($"[HUD] OnCoinsChanged callback acionado com coinCount = {coinCount}");
        UpdateCoinDisplay(coinCount);
    }

    /// <summary>
    /// Atualiza o texto na tela com o valor de moedas.
    /// </summary>
    private void UpdateCoinDisplay(int coinCount)
    {
        if (coinText != null)
        {
            coinText.text = $"Moedas: {coinCount}";
            Debug.Log($"[HUD] Texto atualizado para: 'Moedas: {coinCount}'");
        }
        else
        {
//            Debug.LogWarning("[HUD] CoinHUD: Componente TextMeshProUGUI não foi atribuído no Inspector!");
        }
    }
}
