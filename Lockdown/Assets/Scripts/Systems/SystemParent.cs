using UnityEngine;

public abstract class SystemParent : MonoBehaviour
{
    [Header("System State")]
    [SerializeField] protected bool isPowered;

    [Header("System Identity")]
    [SerializeField] protected string systemName;

    protected PowerManager powerManager;

    protected virtual void Awake()
    {
        // Automatically find PowerManager in scene
        powerManager = FindFirstObjectByType<PowerManager>();

        if (powerManager == null)
        {
            Debug.LogError(systemName + ": No PowerManager found in scene!");
        }
    }

    /// <summary>
    /// Public entry point: system tries to turn on.
    /// </summary>
    public void TryPowerOn()
    {
        if (powerManager == null) return;

        bool approved = powerManager.RequestPower(this);

        if (approved)
        {
            isPowered = true;
            Debug.Log(systemName + " powered ON");
            OnPowerOn();
        }
        else
        {
            Debug.Log(systemName + " denied power request (lockdown or limit reached)");
        }
    }

    /// <summary>
    /// Public entry point: system turns off and informs manager.
    /// </summary>
    public void PowerOff()
    {
        if (!isPowered) return;

        isPowered = false;

        if (powerManager != null)
            powerManager.RemovePower(this);

        Debug.Log(systemName + " powered OFF");

        OnPowerOff();
    }

    /// <summary>
    /// Custom behavior when system turns on.
    /// </summary>
    protected virtual void OnPowerOn() { }

    /// <summary>
    /// Custom behavior when system turns off.
    /// </summary>
    protected virtual void OnPowerOff() { }

    /// <summary>
    /// External state check.
    /// </summary>
    public bool IsPowered()
    {
        return isPowered;
    }
}