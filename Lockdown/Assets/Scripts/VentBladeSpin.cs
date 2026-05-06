using UnityEngine;

public class VentBladeSpin : MonoBehaviour
{
    [Header("Rotation")]
    [Tooltip("Axis the vent spins around")]
    public Vector3 rotationAxis =  new Vector3(0,0,1);

    [Header("Speed Control")]
    public float targetSpeed = 200f;
    public float currentSpeed = 0f;
    public float acceleration = 5f;

    [Header("Space")]
    public bool useWorldSpace = false;

    void Update()
    {
        // Smoothly move current speed toward target speed
        currentSpeed = Mathf.Lerp(currentSpeed, targetSpeed, Time.deltaTime * acceleration);

        // Apply rotation
        if (useWorldSpace)
        {
            transform.Rotate(rotationAxis * currentSpeed * Time.deltaTime, Space.World);
        }
        else
        {
            transform.Rotate(rotationAxis * currentSpeed * Time.deltaTime, Space.Self);
        }
    }

    /// <summary>
    /// Called when the ventilation system powers ON
    /// </summary>
    public void SetOn()
    {
        targetSpeed = 200f;
    }

    /// <summary>
    /// Called when the ventilation system powers OFF
    /// </summary>
    public void SetOff()
    {
        targetSpeed = 0f;
    }
}