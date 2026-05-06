using UnityEngine;

public class Security : SystemParent
{
    [SerializeField] private LaserPivot laserPivot;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    protected override void OnPowerOn()
    {
        laserPivot.OnPowerRestored();
    }

    protected override void OnPowerOff()
    {
        laserPivot.OnPowerLost();
    }

    public void ShotByLaser()
    {
        Debug.Log("Player killed by laser.");


        status.color = Color.red;
        status.text = "LASER MALFUNCTION -> SHOT";
    }
}
