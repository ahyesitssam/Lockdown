using UnityEngine;

public class CommandManager : MonoBehaviour
{
    [Header("Systems")]
    [SerializeField] private Ventilation ventilation;
    [SerializeField] private Security security;
    [SerializeField] private Reactor reactor;

    private void Start()
    {
        ventilation = GameObject.Find("Systems").GetComponent<Ventilation>();
        security = GameObject.Find("Systems").GetComponent<Security>();
        reactor = GameObject.Find("Systems").GetComponent<Reactor>();
    }

    public void ProcessText(string text, GameObject target)
    {
        text = text.ToLower();

        if (text.Contains("power on"))
        {
            // use raycast info to power on that system
            if (!DetermineTargetSystem(target, true))
            {
                Debug.Log("Nothing to divert power to");
            }
        }
        else if (text.Contains("power off") || text.Contains("power of"))
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
                    security.TryPowerOn();
                }
                else
                {
                    security.PowerOff();
                }
                break;
            case "Reactor":
                if (turingPowerOn)
                {
                    reactor.TryPowerOn();
                }
                else
                {
                    reactor.PowerOff();
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
            default:
                return false;
        }

        return true;
    }

} // end class
