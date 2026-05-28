using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;

public class Health : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] private float healthMax = 100f;
    [SerializeField] private float health;
    [SerializeField] private float regenPerSecond = 1f;
    [SerializeField] private float regenDistance = 8f;

    [Header("Visual Smoothing")]
    [Tooltip("Velocidad a la que la barra se desliza al bajar")]
    [SerializeField] private float smoothSpeed = 10f;

    [Header("References")]
    [SerializeField] private LayerMask opponentMask;
    [SerializeField] private GameObject feedbackTextPrefab;
    [SerializeField] private Image healthBarFill;
    [SerializeField] private TextMeshProUGUI healthText;

    private bool isDead = false;
    private float targetFillAmount; // Variable para la suavización

    void Awake()
    {
        health = healthMax;
        targetFillAmount = 1f;
        UpdateUI();
        InvokeRepeating(nameof(Regeneration), 1f, 1f);
    }

    void Update()
    {
        // Esto hace que la barra se mueva suavemente hacia el objetivo
        if (healthBarFill != null)
        {
            healthBarFill.fillAmount = Mathf.Lerp(healthBarFill.fillAmount, targetFillAmount, Time.deltaTime * smoothSpeed);
        }
    }

    // --- Lógica de Daño ---
    public void TakeDamage(float amount)
    {
        float damage = Mathf.Abs(amount);
        ChangeHealth(-damage);
    }

    public void Heal(float amount)
    {
        float heal = Mathf.Abs(amount);
        ChangeHealth(heal);
    }

    public void ChangeHealth(float amount)
    {
        if (isDead) return;

        float oldHealth = health;
        health = Mathf.Clamp(health + amount, 0, healthMax);
        float actualChange = health - oldHealth;

        if (feedbackTextPrefab != null && Mathf.Abs(actualChange) > 0.01f)
        {
            GameObject go = Instantiate(feedbackTextPrefab, transform.position + Vector3.up * 2, Quaternion.identity);
            if (go.TryGetComponent<FeedbackText>(out var fb))
            {
                fb.ChangeText(actualChange);
            }
        }

        UpdateUI();

        if (health <= 0 && !isDead)
        {
            isDead = true;
            StartCoroutine(DeathSequence());
        }
    }

    // --- Flujo de Muerte y Respawn ---
    private IEnumerator DeathSequence()
    {
        yield return new WaitForSeconds(1f);

        PlayerRespawn respawnSystem = GetComponent<PlayerRespawn>();
        if (respawnSystem != null)
        {
            respawnSystem.TriggerRespawn();
        }

        isDead = false;
        health = healthMax;
        UpdateUI();
    }

    private void UpdateUI()
    {
        // En lugar de asignar directamente, actualizamos el objetivo del suavizado
        if (healthBarFill != null) targetFillAmount = health / healthMax;

        if (healthText != null) healthText.text = $"{Mathf.Round(health)}/{healthMax}";
    }

    private void Regeneration()
    {
        bool isAlone = Physics.OverlapSphere(transform.position, regenDistance, opponentMask).Length == 0;

        if (isAlone && health < healthMax && !isDead)
        {
            ChangeHealth(regenPerSecond);
        }
    }
}