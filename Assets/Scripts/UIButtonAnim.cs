using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;

public class UIButtonAnim : MonoBehaviour
{
    private Vector3 originalScale;
    public float punchScale = 0.9f;

    private void Start()
    {
        originalScale = transform.localScale;
    }

    public void OnPointerDown(PointerEventData eventdata)
    {
        StopAllCoroutines();
        StartCoroutine(AnimateScale(punchScale));
    }

    public void OnPointerUp(PointerEventData eventdata)
    {
        StopAllCoroutines();
        StartCoroutine(AnimateScale(1f));
    }

    private IEnumerator AnimateScale(float target)
    {
        Vector3 targetScale = originalScale * target;
        float t = 0;
        while (t < 1)
        {
            t = Time.unscaledDeltaTime * 10f;
            transform.localScale = Vector3.Lerp(transform.localScale, targetScale, t);
            yield return null;
        }
        transform.localScale = targetScale;
    }
}
