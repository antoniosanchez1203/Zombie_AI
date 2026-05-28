using UnityEngine;

public class EnemyDamage : MonoBehaviour
{
    [Header("Configuración de Daño")]
    [Tooltip("Cantidad de vida que quita por impacto")]
    [SerializeField] private float damageAmount = 10f;

    [Tooltip("Tiempo en segundos entre cada golpe")]
    [SerializeField] private float damageInterval = 1.0f;

    [Tooltip("Distancia mínima para infligir daño")]
    [SerializeField] private float damageRange = 1.5f;

    [Header("Configuración de Objetivo")]
    [SerializeField] private string targetTag = "Player";

    private float timer;
    private Transform target;
    private Health targetHealth;

    void Start()
    {
        // Buscamos al jugador al iniciar
        GameObject playerObj = GameObject.FindWithTag(targetTag);
        if (playerObj != null)
        {
            target = playerObj.transform;
            targetHealth = playerObj.GetComponent<Health>();
        }
    }

    void Update()
    {
        if (target == null) return;

        // Comprobamos la distancia
        float distance = Vector3.Distance(transform.position, target.position);

        if (distance <= damageRange)
        {
            timer += Time.deltaTime;

            if (timer >= damageInterval)
            {
                DealDamage();
                timer = 0f;
            }
        }
        else
        {
            // Reseteamos el timer si el jugador sale del rango
            timer = 0f;
        }
    }

    private void DealDamage()
    {
        if (targetHealth != null)
        {
            // Llamamos al método de daño que tenga tu script Health
            targetHealth.TakeDamage(damageAmount);
        }
    }

    // Esto dibuja un círculo rojo en el editor para que veas el rango de ataque
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, damageRange);
    }
}