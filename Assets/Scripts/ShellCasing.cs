using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShellCasing : MonoBehaviour
{
    public float LIFE_TIME = 5f;
    private Material mat;
    private Color originalColor;
    private Rigidbody rigidBody;
    
    void Start()
    {
        mat = GetComponent<Renderer>().material;
        rigidBody = GetComponent<Rigidbody>();
        originalColor = mat.color;
        StartCoroutine(Fade());
    }

    IEnumerator Fade()
    {
        for (float t = 0f; t < LIFE_TIME; t += Time.deltaTime)
        {
            float normalizedTime = t / LIFE_TIME;
            mat.color = Color.Lerp(originalColor, Color.clear, normalizedTime);
            yield return null;
        }
        Destroy(gameObject);
    }

    void OnTriggerEnter(Collider collider)
    {
        Debug.Log("Collision with " + collider.name);
        if (collider.tag == "Ground")
        {
            rigidBody.Sleep();
        }
    }
}
