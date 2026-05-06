using UnityEngine;

public class TerminalManager : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ProcessText(string text)
    {
        text = text.ToLower();

        if (text.Contains("divert power"))
        {
            // use raycast info to power on that system
        }
        else if (text.Contains("power off"))
        {
            // use raycast info to power off that system
        }
    }
} // end class
