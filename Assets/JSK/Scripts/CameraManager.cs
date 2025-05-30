using System.Collections;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
    [Header("Target")]
    public Transform target;
    public Vector3 offset = new Vector3(0, 0, -10);

    [Header("Follow")]
    public float smoothSpeed = 5f;

    [Header("Zoom")]
    public float zoomSpeed = 2f;
    public float targetZoom = 5f; // ±‚∫ª ¡‹
    private Camera cam;

    [Header("Shake")]
    public float shakeDuration = 0.2f;
    public float shakeMagnitude = 0.2f;
    private Vector3 initialPosition;
    private Coroutine shakeRoutine;

    void Start()
    {
        cam = Camera.main;
        targetZoom = cam.orthographicSize;
    }

    void LateUpdate()
    {
        if (target == null) return;

        //Vector3 desiredPos = target.position + offset;
        //Vector3 smoothedPos = Vector3.Lerp(transform.position, desiredPos, smoothSpeed * Time.deltaTime);
        transform.position = target.position + offset;
    }

    public void SetZoom(float zoom)
    {
        targetZoom = zoom;
    }

    public void ShakeCamera(float duration = 0.2f, float magnitude = 0.2f)
    {
        if (shakeRoutine != null) StopCoroutine(shakeRoutine);
        shakeRoutine = StartCoroutine(Shake(duration, magnitude));
    }

    private IEnumerator Shake(float duration, float magnitude)
    {
        Vector3 originalPos = transform.position;
        float elapsed = 0f;

        while (elapsed < duration)
        {
            float x = Random.Range(-1f, 1f) * magnitude;
            float y = Random.Range(-1f, 1f) * magnitude;

            transform.position = new Vector3(originalPos.x + x, originalPos.y + y, originalPos.z);
            elapsed += Time.deltaTime;
            yield return null;
        }

        transform.position = originalPos;
    }
}
