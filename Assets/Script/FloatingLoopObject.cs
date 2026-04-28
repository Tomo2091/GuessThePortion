using UnityEngine;
using System.Collections;

public class FloatingLooObject : MonoBehaviour
{
    public float floatDistance = 0.5f;
    public float floatDuration = 1f;
    public float hiddenDuration = 0.5f;
    public float scaleMultiplier = 1.2f;

    private Vector3 startPos;
    private Vector3 startScale;
    private Renderer[] renderers;

    void Start()
    {
        startPos = transform.localPosition;
        startScale = transform.localScale;
        renderers = GetComponentsInChildren<Renderer>();

        StartCoroutine(FloatLoop());
    }

    IEnumerator FloatLoop()
    {
        while (true)
        {
            transform.localPosition = startPos;
            transform.localScale = startScale;
            SetRenderersVisible(true);

            float timer = 0f;

            while (timer < floatDuration)
            {
                timer += Time.deltaTime;
                float t = timer / floatDuration;
                float smoothT = Mathf.SmoothStep(0f, 1f, t);

                float yOffset = Mathf.Lerp(0f, floatDistance, smoothT);
                transform.localPosition = startPos + new Vector3(0f, yOffset, 0f);

                float scale = Mathf.Lerp(scaleMultiplier, 1f, smoothT);
                transform.localScale = startScale * scale;

                yield return null;
            }

            SetRenderersVisible(false);
            yield return new WaitForSeconds(hiddenDuration);
        }
    }

    void SetRenderersVisible(bool visible)
    {
        foreach (Renderer r in renderers)
        {
            r.enabled = visible;
        }
    }
}