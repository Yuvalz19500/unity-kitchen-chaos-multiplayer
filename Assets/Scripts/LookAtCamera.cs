using System;
using UnityEngine;
using Debug = System.Diagnostics.Debug;

public class LookAtCamera : MonoBehaviour
{
    private enum Mode
    {
        LookAt,
        LookAtInverted,
        CameraForward,
        CameraForwardInverted,
    }
    
    [SerializeField] private Mode mode = Mode.LookAt;

    private void LateUpdate()
    {
        Debug.Assert(Camera.main != null, "Camera.main != null");
        
        switch (mode)
        {
            case Mode.LookAt:
                transform.LookAt(Camera.main.transform);
                break;
            case Mode.LookAtInverted:
                Vector3 directionFromCamera = transform.position - Camera.main.transform.position;
                transform.LookAt(transform.position + directionFromCamera);
                break;
            case Mode.CameraForward:
                transform.forward = Camera.main.transform.forward;
                break;
            case Mode.CameraForwardInverted:
                transform.forward = -Camera.main.transform.forward;
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
    }
}
