using UnityEngine;
using System.Collections;

public class ClickTarget : MonoBehaviour
{
    // Variables serializadas para control desde el Inspector [cite: 44]
    [SerializeField] private float growthSpeed = 2f; // Velocidad de crecimiento [cite: 46]
    [SerializeField] private float fadeSpeed = 5f;   // Velocidad de desvanecimiento [cite: 48]

    // Referencias privadas a componentes [cite: 44, 49]
    private Transform gfx;
    private SpriteRenderer sr;

    private void Awake()
    {
        // Obtener referencias buscando al objeto hijo "GFX" [cite: 57, 58, 62]
        gfx = transform.Find("GFX");
        sr = gfx.GetComponent<SpriteRenderer>();

        // Iniciar el proceso animado en cuanto el objeto se instancie [cite: 59, 60]
        StartCoroutine(AnimateTarget());
    }

    private IEnumerator AnimateTarget()
    {
        // Configuración inicial de escala [cite: 68, 69, 70]
        float baseScale = gfx.localScale.x;
        gfx.localScale = Vector3.zero;
        float currentScale = 0f;

        // Fase 1: Crecimiento [cite: 71]
        // Escala el objeto de 0 a la escala deseada [cite: 64, 72]
        while (currentScale < baseScale)
        {
            currentScale += Time.deltaTime * growthSpeed; // [cite: 75]
            gfx.localScale = Vector3.one * currentScale;  // [cite: 76]
            yield return null;                            // [cite: 77]
        }
        gfx.localScale = Vector3.one * baseScale;         // Asegurar escala final [cite: 78]

        // Fase 2: Desvanecimiento [cite: 79]
        // Cambia el canal alfa hasta que sea invisible [cite: 52, 81]
        Color myColor = sr.color;                         // [cite: 80]
        while (myColor.a > 0f)
        {
            myColor.a -= Time.deltaTime * fadeSpeed;      // [cite: 83]
            sr.color = myColor;                           // [cite: 84]
            yield return null;                            // [cite: 85]
        }

        // Fase 3: Destrucción [cite: 87]
        // Elimina el objeto de la escena al terminar [cite: 87, 119]
        Destroy(gameObject);
    }
}