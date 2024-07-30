using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraRecoil : Singleton<CameraRecoil>
{
    Coroutine shakeCoroutine;

    Vector3 originPos;

    private void Start()
    {
        originPos = transform.position;
    }

    public void Shake(float duration, float magnitude)
    {
        if (shakeCoroutine != null)
        {
            StopCoroutine(shakeCoroutine);
        }
        shakeCoroutine = StartCoroutine(ShakeCoroutine(duration, magnitude));
    }

    public IEnumerator ShakeCoroutine(float duration, float magnitude)
    {
        float elapsed = 0.0f;

        while (elapsed < duration)
        {
            float x = Random.Range(-1f, 1f) * magnitude;
            float y = Random.Range(-1f, 1f) * magnitude;

            transform.localPosition = new Vector3(x, y, originPos.z);
            elapsed += Time.deltaTime;

            yield return null;
        }

        transform.localPosition =originPos;
    }
}
