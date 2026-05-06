using System.Collections;
using UnityEngine;

public class LaserPivot : MonoBehaviour
{
    private Security security;

    [Header("Rotation Settings")]
    public float leftAngle = -45f;
    public float rightAngle = 45f;
    public float rotationSpeed = 2f;

    [Header("Axis of Rotation")]
    public Vector3 rotationAxis = Vector3.up;

    [Header("Attack Sequence")]
    [SerializeField] private GameObject electricityEffect;
    [SerializeField] private GameObject laserBeam;

    [SerializeField] private float chargeTime = 10f;

    private float t = 0f;
    private bool isPaused = false;

    private Coroutine attackRoutine;

    private void Start()
    {
        security = GameObject.Find("Systems").GetComponent<Security>();
    }

    void Update()
    {
        if (isPaused) return;

        t += Time.deltaTime * rotationSpeed;
        float lerpValue = (Mathf.Sin(t) + 1f) / 2f;
        float angle = Mathf.Lerp(leftAngle, rightAngle, lerpValue);

        transform.localRotation = Quaternion.AngleAxis(angle, rotationAxis);
    }

   
    public void OnPowerLost()
    {
        if (attackRoutine != null) return;

        attackRoutine = StartCoroutine(SnapAndAttackRoutine());
    }

  
    public void OnPowerRestored()
    {
        if (attackRoutine != null)
        {
            StopCoroutine(attackRoutine);
            attackRoutine = null;
        }

        // Reset visuals
        if (electricityEffect != null)
            electricityEffect.SetActive(false);

        if (laserBeam != null)
            laserBeam.SetActive(false);

        isPaused = false;
    }

    private IEnumerator SnapAndAttackRoutine()
    {
        isPaused = true;

        // Snap to center
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

        // START WARNING PHASE
        if (electricityEffect != null)
            electricityEffect.SetActive(true);

        // Wait for charge time
        yield return new WaitForSeconds(chargeTime);

        // FIRE LASER
        if (laserBeam != null)
            laserBeam.SetActive(true);

        security.ShotByLaser();

        attackRoutine = null;
    }

    
}