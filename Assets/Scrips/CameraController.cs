using UnityEngine;
using Cinemachine;
using UnityEngine.EventSystems;

public class CameraController : MonoBehaviour
{
    public bool usingOrbitCamera { get; private set; } = false;

    [SerializeField] PlayerLandInput input;

    [HideInInspector] public CinemachineVirtualCamera activeCam;
    int activeCamPriorityModifer = 3010;

    public Camera mainCam;
    public CinemachineVirtualCamera cin1stPerson;
    public CinemachineVirtualCamera cin3rdPerson;
    public CinemachineVirtualCamera cinOrbit;
    public CinemachineVirtualCamera aim;

    private void Start()
    {
        ChangeCamera();
    }

    private void ChangeCamera()
    {
        mainCam.cullingMask &= ~(1 << LayerMask.NameToLayer("Player"));
        mainCam.cullingMask |= 1 << LayerMask.NameToLayer("PlayerGun");
        mainCam.cullingMask &= ~(1 << LayerMask.NameToLayer("PlayerGunVisual"));
        activeCam = cin1stPerson;
    }
}
