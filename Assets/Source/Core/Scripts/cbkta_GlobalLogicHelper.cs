using System.Collections;
using UnityEngine;

public class cbkta_GlobalLogicHelper : MonoBehaviour
{
    public System.Random GenerateRandomSystem()
    {
        return new System.Random(System.Guid.NewGuid().GetHashCode());
    }
    public void ShakeGameObject(GameObject obj, float magnitude, float duration = 0.3f, System.Action<GameObject> funcOn = null, System.Action<GameObject> funcEnd = null)
    {
        StartCoroutine(this.CoroutineShakingGameObject(obj, magnitude, duration, funcOn, funcEnd));
    }

    IEnumerator CoroutineShakingGameObject(GameObject obj, float magnitude, float duration, System.Action<GameObject> funcOn, System.Action<GameObject> funcEnd)
    {
        Transform targetT = null;
        RectTransform targetRT = null;

        bool useTargetT = false;
        if (!(useTargetT = obj.TryGetComponent<Transform>(out targetT)))
        {
            targetRT = obj.GetComponent<RectTransform>(); // panel UI
        }

        Vector3 originalPos = useTargetT ? targetT.localPosition : targetRT.anchoredPosition;
        float elapsed = 0f;

        while (elapsed < duration)
        {
            float offsetX = Random.Range(-1f, 1f) * magnitude;
            float offsetY = Random.Range(-1f, 1f) * magnitude;
            if (useTargetT) targetT.localPosition = originalPos + new Vector3(offsetX, offsetY, 0);
            else targetRT.anchoredPosition = originalPos + new Vector3(offsetX, offsetY, 0);

            elapsed += Time.unscaledDeltaTime;

            if (funcOn != null) funcOn(obj);

            yield return null;
        }

        if (useTargetT) targetT.localPosition = originalPos;
        else targetRT.anchoredPosition = originalPos;

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
