using UnityEngine;
using UnityEngine.InputSystem;

public class GameInput : MonoBehaviour
{
    [SerializeField]
    private float moveByKeySpeed = 4f;
    
    [SerializeField]
    private float lookSpeed = 2f;

    [SerializeField]
    private float zoomSpeed = 2f;

    private bool buttonCameraForward;
    private bool buttonCameraBack;
    private bool buttonCameraLeft;
    private bool buttonCameraRight;
    private bool buttonCameraUp;
    private bool buttonCameraDown;
    private bool buttonCameraLookAround;
    private Vector2 look;
    private float zoom;
    private float timeMovingStarted;

    private void OnLook(InputValue value)
    {
        look = value.Get<Vector2>();
    }

    private void OnZoom(InputValue value)
    {
        zoom = value.Get<float>();
    }

    private void OnCameraForward(InputValue value)
    {
        timeMovingStarted = Time.time;
        buttonCameraForward = value.isPressed;
        Game.Instance.MoveActive = true;
        Game.Instance.LookAroundActive = true;
    }

    private void OnCameraBack(InputValue value)
    {
        timeMovingStarted = Time.time;
        buttonCameraBack = value.isPressed;
        Game.Instance.MoveActive = true;
        Game.Instance.LookAroundActive = true;
    }

    private void OnCameraLeft(InputValue value)
    {
        timeMovingStarted = Time.time;
        buttonCameraLeft = value.isPressed;
        Game.Instance.MoveActive = true;
        Game.Instance.LookAroundActive = true;
    }

    private void OnCameraRight(InputValue value)
    {
        timeMovingStarted = Time.time;
        buttonCameraRight = value.isPressed;
        Game.Instance.MoveActive = true;
        Game.Instance.LookAroundActive = true;
    }

    private void OnCameraUp(InputValue value)
    {
        timeMovingStarted = Time.time;
        buttonCameraUp = value.isPressed;
        Game.Instance.MoveActive = true;
        Game.Instance.LookAroundActive = true;
    }

    private void OnCameraDown(InputValue value)
    {
        timeMovingStarted = Time.time;
        buttonCameraDown = value.isPressed;
        Game.Instance.MoveActive = true;
        Game.Instance.LookAroundActive = true;
    }


    private void OnCameraLookAround(InputValue value)
    {
        buttonCameraLookAround = value.isPressed;
        Game.Instance.LookAroundActive = true;
    }

    private void CameraSetMoveMode()
    {
        if (Game.Instance.CameraPositionObject != null)
        {
            Game.Instance.CameraPositionObject = null;
            Game.Instance.ToggleCameraPosition.enabled = false;
        }
    }

    private void Update()
    {
        float moveSpeed = moveByKeySpeed * Mathf.Pow(2, (Time.time - timeMovingStarted));

        if (buttonCameraForward)
        {
            CameraSetMoveMode();
            transform.position += transform.forward * Time.deltaTime * moveSpeed;
        }
        if (buttonCameraBack)
        {
            CameraSetMoveMode();
            transform.position -= transform.forward * Time.deltaTime * moveSpeed;
        }
        if (buttonCameraRight)
        {
            CameraSetMoveMode();
            transform.position += transform.right * Time.deltaTime * moveSpeed;
        }
        if (buttonCameraLeft)
        {
            CameraSetMoveMode();
            transform.position -= transform.right * Time.deltaTime * moveSpeed;
        }
        if (buttonCameraUp)
        {
            CameraSetMoveMode();
            transform.position += transform.up * Time.deltaTime * moveSpeed;
        }
        if (buttonCameraDown)
        {
            CameraSetMoveMode();
            transform.position -= transform.up * Time.deltaTime * moveSpeed;
        }

        // Look around when Mouse is not pressed
        if (buttonCameraLookAround)
        {
            if (Game.Instance.CameraLookAtObject != null)
            {
                Game.Instance.CameraLookAtObject = null;
                Game.Instance.ToggleCameraLookAt.enabled = false;
            }
            Game.Instance.CameraRotationX += lookSpeed * look.y;
            Game.Instance.CameraRotationY -= lookSpeed * look.x;
        }

        // Zoom in and out with Mouse Wheel
        // TODO : should change camera field of view
        transform.Translate(0, 0, zoom * zoomSpeed, Space.Self);
    }
}