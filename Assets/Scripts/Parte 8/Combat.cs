using UnityEngine;

public class Combat : MonoBehaviour
{
    private Animator anim;
    private bool isPlayer;
    [SerializeField] private LayerMask opponentMask;

    void Awake()
    {
        isPlayer = gameObject.CompareTag("Player");
        anim = GetComponent<Animator>();
    }

    void Update()
    {
        if (!isPlayer) return;

        // Input de ataque
        anim.SetBool("attack", Input.GetMouseButton(1));

        // Rotación hacia el cursor
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out RaycastHit hit))
        {
            Vector3 target = new Vector3(hit.point.x, transform.position.y, hit.point.z);
            transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.LookRotation(target - transform.position), Time.deltaTime * 10f);
        }
    }

    // Llamado por Animation Event
    public void ImpactEvent()
    {
        Collider[] hits = Physics.OverlapSphere(transform.position + transform.forward, 1f, opponentMask);
        foreach (var h in hits)
        {
            if (h.TryGetComponent(out Health hScript))
            {
                hScript.ChangeHealth(-20f); // Valor de daño
            }
        }
    }
}