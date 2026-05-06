using UnityEngine;

public class ReactorSpin : MonoBehaviour
{
    [Header("Spin Settings")]
    [SerializeField] private float spinSpeed = 300f;
    [SerializeField] private float currentSpeed = 0f;
    [SerializeField] private float acceleration = 5f;

    private float targetSpeed = 0f;

    void Update()
    {
        currentSpeed = Mathf.Lerp(currentSpeed, targetSpeed, Time.deltaTime * acceleration);

        // Z-axis only (like you wanted)
        transform.Rotate(0f, 0f, currentSpeed * Time.deltaTime, Space.Self);
    }

    public void StartSpinning()
    {
        targetSpeed = spinSpeed;
    }

    public void StopSpinning()
    {
        targetSpeed = 0f;
    }
}