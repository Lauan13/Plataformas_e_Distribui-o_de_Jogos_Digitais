using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody), typeof(PlayerInput))]
public class PlayerController : MonoBehaviour
{
    // ...existing code...

    // Internal tuning values (not exposed in Inspector)
    private float moveSpeed = 10f; // units/s used as acceleration scale
    private float sprintMultiplier = 1.6f;
    private float jumpForce = 5f;

    private Rigidbody rb;
    private PlayerInput playerInput;
    private InputAction moveAction;
    private InputAction sprintAction;
    private InputAction jumpAction;

    // Ground check
    private float groundCheckDistance = 0.6f;

    // Inventário do jogador: total de moedas (não estático, cada jogador gerencia seu próprio total)
    private int _totalCoins = 0;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
        playerInput = GetComponent<PlayerInput>();

        if (rb == null)
            Debug.LogError("PlayerController requires a Rigidbody.");
        if (playerInput == null)
            Debug.LogError("PlayerController requires a PlayerInput component.");

        // cache actions from the PlayerInput's action asset
        if (playerInput != null && playerInput.actions != null)
        {
            moveAction = playerInput.actions["Move"];
            sprintAction = playerInput.actions["Sprint"];
            jumpAction = playerInput.actions["Jump"];
        }

        // try to set a better default ground check distance from collider if available
        var col = GetComponent<Collider>();
        if (col != null)
            groundCheckDistance = col.bounds.extents.y + 0.05f;
    }

    void OnEnable()
    {
        if (playerInput != null && playerInput.actions != null)
            playerInput.actions.Enable();

        if (jumpAction != null)
            jumpAction.performed += OnJumpPerformed;

        // Se inscreve no evento de coleta de moedas para desacoplar do Coin.cs
        PlayerObserverManager.OnCoinCollected += HandleCoinCollected;

        // Notifica o HUD/observadores do valor atual (inicial)
        // Isso garante que qualquer HUD que esteja inscrito receba o valor inicial ao habilitar o jogador.
        PlayerObserverManager.NotifyCoinsChanged(_totalCoins);
    }

    void OnDisable()
    {
        if (playerInput != null && playerInput.actions != null)
            playerInput.actions.Disable();

        if (jumpAction != null)
            jumpAction.performed -= OnJumpPerformed;

        // Se desinscreve do evento de coleta de moedas para evitar memory leaks
        PlayerObserverManager.OnCoinCollected -= HandleCoinCollected;
    }

    void FixedUpdate()
    {
        if (rb == null)
            return;

        Vector2 input2D = Vector2.zero;
        if (moveAction != null)
            input2D = moveAction.ReadValue<Vector2>();

        // convert to X/Z plane (Y = 0)
        Vector3 move = new Vector3(input2D.x, 0f, input2D.y);

        // sprint check
        bool sprinting = false;
        if (sprintAction != null)
            sprinting = sprintAction.ReadValue<float>() > 0.5f;

        float multiplier = sprinting ? sprintMultiplier : 1f;

        // apply acceleration force only on X/Z, do not override vertical velocity
        if (move.sqrMagnitude > 0f)
        {
            Vector3 force = move.normalized * moveSpeed * multiplier;
            rb.AddForce(force, ForceMode.Acceleration);
        }
    }

    private void OnJumpPerformed(InputAction.CallbackContext ctx)
    {
        if (rb == null)
            return;

        if (IsGrounded())
        {
            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
        }
    }

    private bool IsGrounded()
    {
        // Raycast down from slightly above the transform position to detect ground
        Vector3 origin = transform.position + Vector3.up * 0.1f;
        float distance = groundCheckDistance + 0.02f;
        return Physics.Raycast(origin, Vector3.down, distance);
    }

    /// <summary>
    /// Adiciona moedas ao inventário local do jogador e notifica observadores através do PlayerObserverManager.
    /// </summary>
    /// <param name="value">Quantidade de moedas a adicionar (deve ser >= 0).</param>
    public void AddCoins(int value)
    {
        if (value <= 0)
        {
            if (value < 0)
                Debug.LogWarning($"PlayerController.AddCoins(): valor negativo ({value}) rejeitado.");
            return;
        }

        int previous = _totalCoins;
        _totalCoins += value;
        Debug.Log($"[Player] Moedas +{value} | Total anterior: {previous} | Total atual: {_totalCoins}");

        // Notifica HUDs/observadores com o novo total do jogador
        PlayerObserverManager.NotifyCoinsChanged(_totalCoins);
    }

    /// <summary>
    /// Handler chamado quando uma moeda é coletada (via evento OnCoinCollected).
    /// Desacopla o PlayerController do Coin.cs através do padrão Observer.
    /// </summary>
    /// <param name="coinValue">O valor da moeda coletada.</param>
    private void HandleCoinCollected(int coinValue)
    {
        AddCoins(coinValue);
    }
}

