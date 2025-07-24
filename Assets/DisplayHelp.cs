using UnityEngine;
using TMPro;


public class DisplayHelp : MonoBehaviour
{
    public GameObject helpSizeUp;
    public GameObject helpSizeDown;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            helpSizeUp.SetActive(true);
            helpSizeDown.SetActive(true);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            helpSizeUp.SetActive(false);
            helpSizeDown.SetActive(false);
        }
    }
}
