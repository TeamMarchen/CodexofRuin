using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
    public GameObject player;
    private Vector3 offset;

    private void LateUpdate()
    {
        if (player == null) return;

        Vector3 desiredPosition = player.transform.position + offset;
        desiredPosition.z = -10f;
        transform.position = desiredPosition;
    }

    public void Setting(GameObject gameObject)
    {
        player = gameObject;
    }
}
