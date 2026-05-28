using UnityEngine;
using TMPro;

public class FeedbackText : MonoBehaviour
{
    [SerializeField] private float lifespan = 2f;
    [SerializeField] private float moveSpeed = 0.5f;
    [SerializeField] private float fadeSpeed = 0.5f;

    private TextMeshPro myText;

    void Awake()
    {
        // Usamos GetComponentInChildren para evitar el NullReferenceException
        // Esto encontrará el componente de texto sin importar cómo se llame el objeto hijo
        myText = GetComponentInChildren<TextMeshPro>();

        if (myText == null)
        {
            Debug.LogError("No se encontró el componente TextMeshPro en los hijos de " + gameObject.name);
        }

        Destroy(gameObject, lifespan);
    }

    void Update()
    {
        if (myText == null) return;

        transform.localPosition += Vector3.forward * Time.deltaTime * moveSpeed;
        SetFadingText();
    }

    private void SetFadingText()
    {
        Color c = myText.color;
        c.a -= Time.deltaTime * fadeSpeed;
        myText.color = c;
    }

    public void ChangeText(float value)
    {
        if (myText == null) return;

        float val = Mathf.Round(value);
        string strVal = val.ToString("N0");
        if (val > 0) strVal = "+" + strVal;

        myText.text = strVal;
        myText.color = (val > 0) ? Color.green : Color.red;
    }
}