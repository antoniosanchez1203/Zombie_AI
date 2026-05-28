using UnityEngine;

public static class Tools
{
    // Este método es el que los PDFs llaman para remapear valores (como el fillAmount de la barra de salud)
    public static float MapValues(float value, float min1, float max1, float min2, float max2)
    {
        return min2 + (value - min1) * (max2 - min2) / (max1 - min1);
    }
}