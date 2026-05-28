using UnityEngine;

public class DestroyAfterSeconds : MonoBehaviour
{
    public float seconds = 8f;
    void Start() { Destroy(gameObject, seconds); }
}