using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CubeLifetime : MonoBehaviour
{
    void Start()
    {
        StartCoroutine(ShrinkAndDestroy(3f, 0.5f));
    }

    IEnumerator ShrinkAndDestroy(float waitTime, float shrinkDuration)
    {
        yield return new WaitForSeconds(waitTime);
        
        Vector3 startScale = transform.localScale;
        float elapsedTime = 0f;
        
        while (elapsedTime < shrinkDuration)
        {
            elapsedTime += Time.deltaTime;
            float progress = elapsedTime / shrinkDuration;
            transform.localScale = Vector3.Lerp(startScale, Vector3.zero, progress);
            yield return null;
        }
        
        Destroy(gameObject);
    }
}