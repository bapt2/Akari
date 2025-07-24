using UnityEngine;

public class LastRoomCameraTrigger : MonoBehaviour
{
    [SerializeField] private CameraSwitcher cameraSwitcher;

    private bool hasTriggered = false;

    void OnTriggerEnter(Collider other)
    {
        if (hasTriggered) return;

        if (other.CompareTag("Player"))
        {
            hasTriggered = true;

            //  Forcer le switch en TPS dès l'entrée dans la pièce
            if (cameraSwitcher != null && cameraSwitcher.IsFPS())
            {
                cameraSwitcher.SwitchCamera(); // ou cameraSwitcher.SwitchToTPS() si tu as une méthode dédiée
                Debug.Log("Entrée dans la pièce → Vue TPS activée !");
            }
        }
    }
}
