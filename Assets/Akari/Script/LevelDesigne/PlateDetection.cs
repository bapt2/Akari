using UnityEngine;

public class PlateDetection : MonoBehaviour
{

    public float speed;
    public float startYDoor;
    float lerpTime;
    float lerpValue;
    public GameObject door;


    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.CompareTag("GrowingObject"))
        {
            StartCoroutine(GameManager.instance.OpenDoor(door, lerpTime, lerpValue, speed, startYDoor));
        }
    }
}
