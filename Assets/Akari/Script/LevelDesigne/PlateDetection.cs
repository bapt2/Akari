using UnityEngine;

public class PlateDetection : MonoBehaviour
{
    [Header("Value to open door")]
    public float speed;
    public float startYDoor;
    float lerpTime;
    float lerpValue;
    public GameObject door;

    [Header("Need Specific size")]
    public bool needSize;
    public float wantedMinumumSize;
    public float wantedMaximumSize;
    public GameObject wantedObject;

    [Header("Change TPS camera position")]
    public Transform tpsCamera;
    public Vector3 newPosition;
    public Quaternion newRotation;

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.CompareTag("GrowingObject") && needSize && WantedSize(wantedObject, wantedMinumumSize, wantedMaximumSize))
        {
            StartCoroutine(GameManager.instance.OpenDoor(door, lerpTime, lerpValue, speed, startYDoor));
            ChangeTPSCameraPos();
        }
        else if (collision.collider.CompareTag("GrowingObject") && !needSize)
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

    public void ChangeTPSCameraPos()
    {
        tpsCamera.transform.localPosition = newPosition;
        tpsCamera.transform.localRotation = newRotation;
    }
}
