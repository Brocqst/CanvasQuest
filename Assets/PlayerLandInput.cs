using UnityEngine;
using UnityEngine.Animations;
using UnityEngine.InputSystem;

public class PlayerLandInput : MonoBehaviour
{
    public Vector2 moveInput { get; private set; } = Vector2.zero;
    public Vector2 lookInput { get; private set; } = Vector2.zero;

    public bool changeCameraWasPressedThisFrame { get; private set; } = false;

    public bool runIsPressed { get; private set; }
    public bool jumpIsPressed { get; private set; }
    public bool shootIsPressed { get; private set; }

    public bool aimIsPressed { get; private set; }

    InputActions input;

    public bool moveIsPressed { get; private set; }

    private void OnEnable()
    {
        input = new InputActions();
        input.HumaniodLand.Enable();

        input.HumaniodLand.Move.performed += SetMove;
        input.HumaniodLand.Move.canceled += SetMove;

        input.HumaniodLand.Look.performed += SetLook;
        input.HumaniodLand.Look.canceled += SetLook;

        input.HumaniodLand.Run.started += SetRun;
        input.HumaniodLand.Run.canceled += SetRun;

        input.HumaniodLand.Jump.started += SetJump;
        input.HumaniodLand.Jump.canceled += SetJump;

        input.HumaniodLand.Shoot.started += SetShoot;
        input.HumaniodLand.Shoot.canceled += SetShoot;

        input.HumaniodLand.Aim.started += SetAim;
        input.HumaniodLand.Aim.canceled += SetAim;
    }

    private void OnDisable()
    {
        input.HumaniodLand.Move.performed -= SetMove;
        input.HumaniodLand.Move.canceled -= SetMove;

        input.HumaniodLand.Look.performed -= SetLook;
        input.HumaniodLand.Look.canceled -= SetLook;

        input.HumaniodLand.Run.started -= SetRun;
        input.HumaniodLand.Run.canceled -= SetRun;

        input.HumaniodLand.Jump.started -= SetJump;
        input.HumaniodLand.Jump.canceled -= SetJump;

        input.HumaniodLand.Shoot.started -= SetShoot;
        input.HumaniodLand.Shoot.canceled -= SetShoot;

        input.HumaniodLand.Aim.started -= SetAim;
        input.HumaniodLand.Aim.canceled -= SetAim;

        input.HumaniodLand.Disable();
    }

    private void Update()
    {
        changeCameraWasPressedThisFrame = input.HumaniodLand.ChangeCamera.WasPressedThisFrame();
    }

    private void SetMove(InputAction.CallbackContext ctx)
    {
        moveInput = ctx.ReadValue<Vector2>();
        moveIsPressed = !(moveInput == Vector2.zero);
    }
     
    private void SetLook(InputAction.CallbackContext ctx)
    {
        lookInput = ctx.ReadValue<Vector2>();
    }

    private void SetRun(InputAction.CallbackContext ctx)
    {
        runIsPressed = ctx.started;
    }

    private void SetJump(InputAction.CallbackContext ctx)
    {
        jumpIsPressed = ctx.started;
    }
    private void SetShoot(InputAction.CallbackContext ctx)
    {
        shootIsPressed = ctx.started;
    }

    private void SetAim(InputAction.CallbackContext ctx)
    {
        aimIsPressed = ctx.started;
    }

}
