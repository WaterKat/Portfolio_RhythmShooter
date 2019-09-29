using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateWithMouse : MonoBehaviour
{
    private void Update()
    {
        transform.rotation *= Quaternion.Euler(Input.GetAxis("Mouse Y")*2, Input.GetAxis("Mouse X")*2, 0);
        transform.position += transform.forward * 2;
    }
}
