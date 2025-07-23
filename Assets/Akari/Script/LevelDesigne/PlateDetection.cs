using UnityEngine;

public class PlateDetection : MonoBehaviour
{

    public float speed;
    public float startYDoor;
    float lerpTime;
    float lerpValue;
    public GameObject door;

    public float wantedMinumumSize;
    public float wantedMaximumSize;
    public GameObject wantedObject;

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.CompareTag("GrowingObject") && WantedSize(wantedObject, wantedMinumumSize, wantedMaximumSize))
        {
            StartCoroutine(GameManager.instance.OpenDoor(door, lerpTime, lerpValue, speed, startYDoor));
        }
    }

    public bool WantedSize(GameObject objectSize,float minimumSize, float maximumSize)
    {
        if (objectSize.transform.localScale.x < maximumSize && objectSize.transform.localScale.z < maximumSize
            && objectSize.transform.localScale.x > minimumSize && objectSize.transform.localScale.z > minimumSize)
            return true;
        else
            return false;
    }
}
