using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZombieHealth : Health
{
    private float TIME_TO_START_FADE = 10f;
    private float TIME_TO_FADE = 5f;
    private Material mat;
    private Color originalColor;
    private Vector3 originalScale;
    private UnityEngine.AI.NavMeshAgent navMeshAgent;

    new void Start()
    {
        base.Start();
        mat = GetComponent<Renderer>().material;
        originalColor = mat.color;
        originalScale = transform.localScale;
        navMeshAgent = GetComponent<UnityEngine.AI.NavMeshAgent>();
    }

    public override void Die()
    {
        Debug.Log("New Die");
        Destroy(navMeshAgent);
        GetRigidBody().isKinematic = false;
        StartCoroutine(Fade());
    }

    IEnumerator Fade()
    {
        for (float t = 0f; t < TIME_TO_FADE + TIME_TO_START_FADE; t += Time.deltaTime)
        {
            if (t > TIME_TO_START_FADE)
            {
                float normalizedTime = (t - TIME_TO_START_FADE) / TIME_TO_FADE;
                mat.color = Color.Lerp(originalColor, Color.clear, normalizedTime);
                transform.localScale = Vector3.Lerp(originalScale, Vector3.zero, normalizedTime);
                // transform.localScale = new Vector3(0.5f, 0.5f, 0.5f);
                yield return null;
            } else
            {
                yield return null;
            }
        }
        Destroy(gameObject);
    }
}
