using UnityEngine;
using UnityEngine.AI;

public class PlayerCombat : MonoBehaviour
{
    private Animator anim;
    private NavMeshAgent agent;
    private Camera cam;

    void Awake()
    {
        anim = GetComponent<Animator>();
        agent = GetComponent<NavMeshAgent>();
        cam = Camera.main;
    }

    void Update()
    {
        // Se ha eliminado la llamada a "attack" para evitar el error de parámetros en el Animator
        // Si necesitas volver a implementar el ataque, asegúrate de crear el parámetro en el Animator primero.

        RotateTowardsMouse();
    }

    void RotateTowardsMouse()
    {
        Ray ray = cam.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out RaycastHit hit))
        {
            Vector3 targetPoint = new Vector3(hit.point.x, transform.position.y, hit.point.z);
            Quaternion targetRotation = Quaternion.LookRotation(targetPoint - transform.position);
            transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, Time.deltaTime * 10f);
        }
    }
}