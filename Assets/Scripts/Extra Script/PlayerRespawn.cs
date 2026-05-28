using UnityEngine;
using System.Collections.Generic;
using System.Linq;

[RequireComponent(typeof(Health))]
public class PlayerRespawn : MonoBehaviour
{
    [Header("Configuración de Respawns")]
    [Tooltip("Arrastra aquí todos los objetos vacíos que servirán como puntos de aparición.")]
    [SerializeField] private List<Transform> spawnPoints = new List<Transform>();

    [Header("Configuración de Revivimiento")]
    [Tooltip("Cantidad de vida que tendrá el jugador al respawnear.")]
    [SerializeField] private float healthOnRespawn = 100f;

    private Health healthScript;

    void Awake()
    {
        // Esto asegura que siempre tengamos acceso al script de vida
        healthScript = GetComponent<Health>();
    }

    /// <summary>
    /// Lógica principal para mover al jugador a un lugar seguro y curarlo.
    /// </summary>
    public void TriggerRespawn()
    {
        Transform bestSpawn = GetSafestSpawn();

        if (bestSpawn != null)
        {
            // --- AQUÍ ESTÁ EL CAMBIO ---
            UnityEngine.AI.NavMeshAgent agent = GetComponent<UnityEngine.AI.NavMeshAgent>();

            if (agent != null)
            {
                // Warp es la forma correcta de "teletransportar" un NavMeshAgent
                agent.Warp(bestSpawn.position);
            }
            else
            {
                // Fallback si por alguna razón no tuviera agente
                transform.position = bestSpawn.position;
            }

            // Rotación (esto sí puedes hacerlo directo)
            transform.rotation = bestSpawn.rotation;

            // Restaurar vida
            healthScript.ChangeHealth(9999f);

            Debug.Log("Jugador respawneado en: " + bestSpawn.name);
        }
    }

    private Transform GetSafestSpawn()
    {
        // Buscamos a los enemigos para calcular distancias
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");

        // Si no hay enemigos, devolvemos el primero de la lista por seguridad
        if (enemies == null || enemies.Length == 0)
        {
            return spawnPoints[0];
        }

        // Algoritmo: Encontrar el spawn cuya distancia media a todos los enemigos sea la mayor
        return spawnPoints.OrderByDescending(spawn =>
            enemies.Average(enemy => Vector3.Distance(spawn.position, enemy.transform.position))
        ).FirstOrDefault();
    }
}