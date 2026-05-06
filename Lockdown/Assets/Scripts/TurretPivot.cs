using System.Collections;
using UnityEngine;

public class TurretPivot : MonoBehaviour
{
    [Header("Rotation Settings")]
    public float leftAngle = -45f;
    public float rightAngle = 45f;
    public float rotationSpeed = 2f;

    [Header("Axis of Rotation")]
    public Vector3 rotationAxis = Vector3.up;

    private float t = 0f;
    private bool isPaused = false;

    void Update()
    {
        if (isPaused) return;

        // Ping-pong value between 0 and 1
        t += Time.deltaTime * rotationSpeed;
        float lerpValue = (Mathf.Sin(t) + 1f) / 2f;

        // Interpolate between left and right angles
        float angle = Mathf.Lerp(leftAngle, rightAngle, lerpValue);

        transform.localRotation = Quaternion.AngleAxis(angle, rotationAxis);
    }

    // Call this from another script to interrupt and snap center
    public void SnapToCenterAndDoAction()
    {
        StartCoroutine(SnapRoutine());
    }

    private IEnumerator SnapRoutine()
    {
        isPaused = true;

        // Smoothly rotate to center (0 degrees)
        Quaternion startRot = transform.localRotation;
        Quaternion targetRot = Quaternion.AngleAxis(0f, rotationAxis);

        float duration = 0.5f;
        float elapsed = 0f;

        while (elapsed < duration)
        {
            transform.localRotation = Quaternion.Slerp(startRot, targetRot, elapsed / duration);
            elapsed += Time.deltaTime;
            yield return null;
        }

        transform.localRotation = targetRot;

        // ---------------------------------------
        // INSERT WHATEVER FUNCTION YOU WANT HERE
        // Example:
        // yield return StartCoroutine(YourOtherCoroutine());
        // ---------------------------------------

        // Small optional delay
        yield return new WaitForSeconds(0.5f);

        isPaused = false;
    }
}
