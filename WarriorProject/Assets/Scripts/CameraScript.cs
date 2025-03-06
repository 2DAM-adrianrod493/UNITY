using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraScript : MonoBehaviour
{
    public Transform Warrior;

    void Update()
    {
        if (Warrior != null)
        {
            Vector3 position = transform.position;
            position.x = Warrior.position.x;
            transform.position = position;
        }
    }
}
