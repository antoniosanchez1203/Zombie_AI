using UnityEngine;
using System.Collections.Generic;

public class PriorityManager : MonoBehaviour
{
    public static PriorityManager Instance;
    public int maxAttackers = 3;
    public List<EnemyAI> currentAttackers = new List<EnemyAI>();

    private void Awake()
    {
        Instance = this;
    }

    public void RegisterAttacker(EnemyAI enemy)
    {
        if (!currentAttackers.Contains(enemy))
            currentAttackers.Add(enemy);
    }

    public void UnregisterAttacker(EnemyAI enemy)
    {
        if (currentAttackers.Contains(enemy))
            currentAttackers.Remove(enemy);
    }

    public bool CanAttack(EnemyAI enemy)
    {
        // Lógica simple de prioridad basada en lista
        return currentAttackers.Count < maxAttackers || currentAttackers.Contains(enemy);
    }
}