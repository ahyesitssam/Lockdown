using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ventilation : SystemParent
{
    [Header("References")]
    [SerializeField] private List<VentBladeSpin> fanVisuals = new List<VentBladeSpin>();

    [Header("Oxygen Settings")]
    [SerializeField] private float timeUntilSuffocation = 30f;

    private Coroutine oxygenRoutine;
    private bool isFailing;

    protected override void OnPowerOn()
    {
        // Turn all vents ON (spin up)
        foreach (var fan in fanVisuals)
        {
            if (fan != null)
                fan.SetOn();
        }

        StopOxygenFailure();
    }

    protected override void OnPowerOff()
    {
        // Turn all vents OFF (spin down)
        foreach (var fan in fanVisuals)
        {
            if (fan != null)
                fan.SetOff();
        }

        StartOxygenFailure();
    }

    private void StartOxygenFailure()
    {
        if (oxygenRoutine != null) return;

        oxygenRoutine = StartCoroutine(OxygenCountdown());
    }

    private void StopOxygenFailure()
    {
        isFailing = false;

        if (oxygenRoutine != null)
        {
            StopCoroutine(oxygenRoutine);
            oxygenRoutine = null;
        }
    }

    private IEnumerator OxygenCountdown()
    {
        isFailing = true;

        float timer = timeUntilSuffocation;

        while (timer > 0f && isFailing)
        {
            timer -= Time.deltaTime;
            yield return null;
        }

        if (isFailing)
        {
            TriggerSuffocation();
        }
    }

    private void TriggerSuffocation()
    {
        Debug.Log("VENTILATION FAILURE - PLAYER SUFFOCATION");

        // Hook into your PowerManager lockdown system later
        // powerManager.TriggerLockdown();
    }
}
