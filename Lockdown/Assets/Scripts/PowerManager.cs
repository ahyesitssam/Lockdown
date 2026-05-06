using System.Collections.Generic;
using UnityEngine;

public class PowerManager : MonoBehaviour
{
    [Header("Power Settings")]
    [SerializeField] private int maxPower = 3;
    [SerializeField] private int currentPower = 0;

    // Track all systems in the scene
    List<SystemParent> systems = new List<SystemParent>();

    void Awake()
    {
        // Auto-find all systems in scene
        systems.AddRange(Object.FindObjectsByType<SystemParent>(FindObjectsSortMode.None));
    }

    /// <summary>
    /// Called by systems when they want power.
    /// </summary>
    public bool RequestPower(SystemParent system)
    {
        if (system == null) return false;

        currentPower++;

        if (currentPower > maxPower)
        {
            TriggerLockdown();
            return false;
        }

        Debug.Log(system.name + " gained power. Current power: " + currentPower);
        return true;
    }

    /// <summary>
    /// Called when a system is powered off.
    /// </summary>
    public void RemovePower(SystemParent system)
    {
        if (system == null) return;

        currentPower = Mathf.Max(0, currentPower - 1);
        Debug.Log(system.name + " lost power. Current power: " + currentPower);
    }

    /// <summary>
    /// Hard reset of all systems.
    /// </summary>
    public void TriggerLockdown()
    {
        Debug.Log("!!! LOCKDOWN TRIGGERED !!!");

        currentPower = 0;

        foreach (var sys in systems)
        {
            if (sys != null && sys.IsPowered())
            {
                sys.PowerOff();
            }
        }

        // optional hook for Terminal system later
        OnLockdown();
    }

    /// <summary>
    /// Optional extension point for terminal or UI systems.
    /// </summary>
    private void OnLockdown()
    {
        Debug.Log("Lockdown handler complete.");
    }
}