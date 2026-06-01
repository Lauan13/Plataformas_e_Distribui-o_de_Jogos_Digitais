using UnityEngine;

/// <summary>
/// Coin: Script de moeda coletável.
/// Quando um objeto com a tag 'Player' colidir com esta moeda (OnTriggerEnter),
/// a moeda notifica através do PlayerObserverManager e se autodestrói.
/// 
/// Requisitos:
/// - Este GameObject deve ter um Collider com "Is Trigger" marcado.
/// - O Player deve ter a tag 'Player' configurada.
/// </summary>
public class Coin : MonoBehaviour
{
    [SerializeField]
    private int coinValue = 1; // Valor da moeda em unidades

    /// <summary>
    /// Detecta colisão com objetos que possuem a tag 'Player'.
    /// </summary>
    private void OnTriggerEnter(Collider collision)
    {
        // Verifica se o objeto que colidiu é o Player
        if (collision.CompareTag("Player"))
        {
//            Debug.Log($"[COIN] Colisão detectada com Player! Moeda: {gameObject.name}, Valor: {coinValue}");
            
            // Adiciona a moeda ao total acumulado no PlayerObserverManager
            PlayerObserverManager.AddCoins(coinValue);
            
//            Debug.Log($"[COIN] AddCoins({coinValue}) foi chamado. Moeda será destruída agora.");
            
            // Destroi este objeto (a moeda)
            Destroy(gameObject);
        }
    }
}
