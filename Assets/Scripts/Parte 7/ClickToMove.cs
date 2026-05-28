using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class ClickToMove : MonoBehaviour
{
    [SerializeField] GameObject clickTarget;

    RaycastHit hit = new RaycastHit();
    NavMeshAgent agent;
    Transform tempContainer;
    Camera cam;

    void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        cam = Camera.main;

        GameObject tc = GameObject.Find("TempContainer") ?? GameObject.Find("TempContainer");
        if (tc == null) 
        {
            tc = new GameObject("TempContainer");           
        }
        tempContainer = tc.transform;


        // Si la cámara no está bien configurada, te avisa sin romper el juego
        if (cam == null)
        {
            Debug.LogError("¡AVISO! No encuentro la cámara. Tu cámara debe tener el Tag 'MainCamera'.");
        }
        if (clickTarget == null)
        {
            Debug.LogWarning("clicktomove no asignado" );
        }

        // Si se te olvidó crear el Temp Container, el script lo crea por ti automáticamente
        GameObject container = GameObject.Find("Temp Container");
        if (container == null)
        {
            container = new GameObject("Temp Container");
        }
        tempContainer = container.transform;
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if (cam == null) return; // Evita el pantallazo rojo si falla la cámara

            Ray ray = cam.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray, out hit))
            {
                if (clickTarget != null)
                {

                   //GameObject goClick = Instantiate(clickTarget, hit.point, Quaternion.identity, tempContainer);
                   //goClick.name = "Target";
                }
                else
                {
                    Debug.LogWarning("¡AVISO! El personaje camina, pero olvidaste arrastrar el 'Target' al cuadrito del script.");
                }

                if (agent != null)
                {
                    agent.SetDestination(hit.point);
                }
            }
        }
    }
}