using UnityEngine;

public class CommandManager : MonoBehaviour
{
    [Header("Systems")]
    [SerializeField] Ventilation ventilation;

    private void Start()
    {
        ventilation = GameObject.Find("Systems").GetComponent<Ventilation>();
        
        if (ventilation == null)
        {
            Debug.LogError("Cannot find Ventalation???");
        }
    }

    public void ProcessText(string text, GameObject target)
    {
        text = text.ToLower();

        if (text.Contains("divert power"))
        {
            // use raycast info to power on that system
            if (!DetermineTargetSystem(target, true))
            {
                Debug.Log("Nothing to divert power to");
            }
        }
        else if (text.Contains("power off"))
        {
            // use raycast info to power off that system
            if (!DetermineTargetSystem(target, false))
            {
                Debug.Log("Nothing to power off");
            }
            
        }
    }

    private bool DetermineTargetSystem(GameObject target, bool turingPowerOn)
    {
        switch (target.tag)
        {
            case "Vent":
                if (turingPowerOn)
                {
                    ventilation.TryPowerOn();
                }
                else
                {
                    ventilation.PowerOff();
                }
                break;
            case "Laser":
                if (turingPowerOn)
                {
                    //Security.TryPowerOn();
                }
                else
                {
                    //Security.PowerOff();
                }
                break;
            case "Light":
                if (turingPowerOn)
                {
                    //Lights.TryPowerOn();
                }
                else
                {
                    //Lights.PowerOff();
                }
                break;
            case "Terminal":
                if (turingPowerOn)
                {
                    //Terminal.TryPowerOn();
                }
                break;
            case "Reactor":
                if (turingPowerOn)
                {
                    //Reactor.TryPowerOn();
                }
                else
                {
                    //Reactor.PowerOff();
                }
                break;
            default:
                return false;
        }

        return true;
    }

} // end class
