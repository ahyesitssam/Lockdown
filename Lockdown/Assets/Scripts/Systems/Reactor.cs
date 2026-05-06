using System.Collections;
using UnityEngine;

public class Reactor : SystemParent
{
    [Header("References")]
    [SerializeField] private ReactorSpin spin;
    [SerializeField] private GameObject implosionEffect;
    [SerializeField] private GameObject explosionEffect;
    [SerializeField] private GameObject core;

    [Header("Timing")]
    [SerializeField] private float warningTime = 5f;
    [SerializeField] private float explosionDelay = 0.2f;

    private Coroutine meltdownRoutine;
    private bool isCoreActive = true;

    private void Start()
    {
        if (this.isPowered)
        {
            spin.StartSpinning();
        }
    }

    protected override void OnPowerOn()
    {
        CancelMeltdown();

        if (!isCoreActive)
        {
            isCoreActive = true;
            core.SetActive(true);
        }

        if (spin != null)
            spin.StartSpinning();

        if (implosionEffect != null)
            implosionEffect.SetActive(false);

        if (explosionEffect != null)
            explosionEffect.SetActive(false);
    }

    protected override void OnPowerOff()
    {
        if (spin != null)
            spin.StopSpinning();

        if (meltdownRoutine == null)
            meltdownRoutine = StartCoroutine(MeltdownSequence());
    }

    private IEnumerator MeltdownSequence()
    {
        yield return new WaitForSeconds(warningTime);

        if (implosionEffect != null)
            implosionEffect.SetActive(true);

        yield return new WaitForSeconds(explosionDelay);

        if (explosionEffect != null)
            explosionEffect.SetActive(true);

        if (core  != null)
        {
            core.SetActive(false);
            isCoreActive = false;
        }

        ExplodePlayer();

        meltdownRoutine = null;
    }

    private void CancelMeltdown()
    {
        if (meltdownRoutine != null)
        {
            StopCoroutine(meltdownRoutine);
            meltdownRoutine = null;
        }
    }

    private void ExplodePlayer()
    {
        Debug.Log("Player killed by reactor explosion.");

        status.color = Color.red;
        status.text = "REACTOR UNSTABLE -> EXPLOSION";
    }
}