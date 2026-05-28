using UnityEngine;
using UnityEngine.AI;

public class BreadcrumbGenerator : MonoBehaviour
{
    [SerializeField] private GameObject breadcrumbPrefab;
    [SerializeField] private float spawnInterval = 0.2f;
    [SerializeField] private bool showBreadcrumbs = true;

    private NavMeshAgent agent;
    private Transform tempContainer;

    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();

        GameObject tc = GameObject.Find("TempContainer");
        if (tc == null)
        {
            tc = new GameObject("TempContainer");
        }

        tempContainer = tc.transform;
    }

    private void Start()
    {
        InvokeRepeating(nameof(DropBreadcrumb), 0f, spawnInterval);
    }

    private void DropBreadcrumb()
    {
        if (breadcrumbPrefab == null) return;
        if (agent == null) return;

        // Solo dejar migas si el jugador realmente se está moviendo
        if (agent.velocity.magnitude <= 0.1f) return;

        GameObject crumb = Instantiate(
            breadcrumbPrefab,
            transform.position,
            Quaternion.identity,
            tempContainer
        );

#if !UNITY_EDITOR
        showBreadcrumbs = false;
#endif

        if (!showBreadcrumbs)
        {
            Transform gfx = crumb.transform.Find("GFX");
            if (gfx != null)
            {
                gfx.gameObject.SetActive(false);
            }
        }
    }
}
