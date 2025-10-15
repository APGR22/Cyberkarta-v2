using System.Collections;
using UnityEngine;

public class VisualShake : MonoBehaviour
{
    public void ShakeGameObject(GameObject obj, float magnitude, float duration = 0.3f, System.Action<GameObject> funcOn = null, System.Action<GameObject> funcEnd = null)
    {
        StartCoroutine(this.CoroutineShakingGameObject(obj, magnitude, duration, funcOn, funcEnd));
    }

    IEnumerator CoroutineShakingGameObject(GameObject obj, float magnitude, float duration, System.Action<GameObject> funcOn, System.Action<GameObject> funcEnd)
    {
        VisualTransformData visualTransformData = new(obj);

        Vector2 originalPos = visualTransformData.LocalAnchoredPosition;
        float elapsed = 0f;

        while (elapsed < duration)
        {
            float offsetX = Random.Range(-1f, 1f) * magnitude;
            float offsetY = Random.Range(-1f, 1f) * magnitude;
            visualTransformData.LocalAnchoredPosition = originalPos + new Vector2(offsetX, offsetY);

            elapsed += Time.unscaledDeltaTime;

            if (funcOn != null) funcOn(obj);

            yield return null;
        }

        visualTransformData.LocalAnchoredPosition = originalPos;

        if (funcEnd != null) funcEnd(obj);
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
