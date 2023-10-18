using Cinemachine;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

public class GameInput : MonoBehaviour
{
    CinemachineVirtualCamera vcamMainCamera;

    [SerializeField] TextMeshProUGUI textMessage;
    [SerializeField] private float moveByKeySpeed = 4f; 
    [SerializeField] private float lookSpeed = 2f;
    [SerializeField] private float zoomSpeed = 2f;
    GameObject cameraFollowObject;
    GameObject cameraLookAtObject;
    Vector3 cameraPosition;

    private float cameraYaw = 0f;
    private float cameraPitch = 0f;
    private float cameraDistance = 3f;
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

    public GameObject CameraFollowObject { get => cameraFollowObject; set => cameraFollowObject = value; }
    public GameObject CameraLookAtObject { get => cameraLookAtObject; set => cameraLookAtObject = value; }
    public Vector3 CameraPosition { get => cameraPosition; set => cameraPosition = value; }

    public void Awake()
    {
        vcamMainCamera = GetComponent<CinemachineVirtualCamera>();
    }

    public void Start()
    {
    }

    private void OnLook(InputValue value)
    {
        look = value.Get<Vector2>();
    }

    private void OnZoom(InputValue value)
    {
        zoom = value.Get<float>();
    }

    public void ChangeLookAtObject(GameObject lookAtObject)
    {
        CameraLookAtObject = lookAtObject;
        if (lookAtObject != null)
        {
            cameraDistance = (CameraLookAtObject.GetComponent<SpaceObject>().MinimumViewDistance * Settings.planetSize) * 1.5f;
        }
    }

    public void ChangeFollowObject(GameObject followObject)
    {
        if (followObject==null)
        {
            cameraPosition += cameraFollowObject.transform.position;
        }
        else
        {
            cameraPosition = followObject.transform.Find("CameraRoot").position - followObject.transform.position;
        }
        CameraFollowObject = followObject;
    }

    private void OnCameraForward(InputValue value)
    {
        timeMovingStarted = Time.time;
        buttonCameraForward = value.isPressed;
//        CameraSetMoveMode();
    }

    private void OnCameraBack(InputValue value)
    {
        timeMovingStarted = Time.time;
        buttonCameraBack = value.isPressed;
    }

    private void OnCameraLeft(InputValue value)
    {
        timeMovingStarted = Time.time;
        buttonCameraLeft = value.isPressed;
    }

    private void OnCameraRight(InputValue value)
    {
        timeMovingStarted = Time.time;
        buttonCameraRight = value.isPressed;
    }

    private void OnCameraUp(InputValue value)
    {
        timeMovingStarted = Time.time;
        buttonCameraUp = value.isPressed;
    }

    private void OnCameraDown(InputValue value)
    {
        timeMovingStarted = Time.time;
        buttonCameraDown = value.isPressed;
    }

    private void OnCameraLookAround(InputValue value)
    {
        buttonCameraLookAround = value.isPressed;
    }

    /*private void CameraSetMoveMode()
    {
        if (Game.Instance.CameraPositionObject != null)
        {
            Game.Instance.CameraPositionObject = null;
            Game.Instance.ToggleCameraPosition.isOn = false;
        }
    }*/

    // forward, back, left, right, down, or up set the camera position.
    //
    // If CameraFollowObject is set:
    // - Initially the camera moves close to the object to follow.
    // - CameraPosition is Relative to the object to follow
    //
    // If CameraFollowObject is unset:
    // - CameraPosition is Absolute: the position of the object it was following is added.
    // - CameraPosition is Relative to the object to follow

    // If CameraLookAtObject is null: Moving the mouse (with right mouse button down) changes the camera rotation.
    // If CameraLookAtObject is set:
    // IF CAMERA IS NOT FOLLOWING ANOTHER OBJECT:
    // - Moving the mouse (with right mouse button down) makes the camera orbit around the object at the current distance.
    // - Forward and back change the distance to the object to follow

    public void LateUpdate()
    {
        vcamMainCamera.transform.eulerAngles = new Vector3(cameraPitch, cameraYaw, 0f);

        if (CameraFollowObject == null)
        {
            vcamMainCamera.transform.position = cameraPosition;
        }
        else
        {
            vcamMainCamera.transform.position = cameraFollowObject.transform.position + cameraPosition;
        }

        if (CameraLookAtObject != null)
        {
            vcamMainCamera.transform.position = CameraLookAtObject.transform.position - vcamMainCamera.transform.forward * cameraDistance;
        }
    }

    private void Update()
    {
        textMessage.text = "pitch: " + cameraPitch + " yaw: " + cameraYaw + " camera distance: " + cameraDistance;
        float moveSpeed = moveByKeySpeed * Mathf.Pow(2, (Time.time - timeMovingStarted));

        if (buttonCameraForward)
        {
            if (CameraLookAtObject != null)
            {
                if (cameraDistance > (CameraLookAtObject.GetComponent<SpaceObject>().MinimumViewDistance * Settings.planetSize))
                {
                    cameraDistance -= Time.deltaTime * moveSpeed;
                }
            }
            else
            {
                cameraPosition += transform.forward * Time.deltaTime * moveSpeed;
            }
        }
        if (buttonCameraBack)
        {
            if (CameraLookAtObject != null)
            {
                cameraDistance += Time.deltaTime * moveSpeed;
            }
            else
            {
                cameraPosition -= transform.forward * Time.deltaTime * moveSpeed;
            }
        }
        if (buttonCameraRight)
        {
            cameraPosition += transform.right * Time.deltaTime * moveSpeed;
        }
        if (buttonCameraLeft)
        {
            cameraPosition -= transform.right * Time.deltaTime * moveSpeed;
        }
        if (buttonCameraUp)
        {
            cameraPosition += transform.up * Time.deltaTime * moveSpeed;
        }
        if (buttonCameraDown)
        {
            cameraPosition -= transform.up * Time.deltaTime * moveSpeed;
        }

        // Look around when right mouse is pressed
        if (buttonCameraLookAround)
        {
            cameraYaw += lookSpeed * look.x;
            cameraPitch -= lookSpeed * look.y;

            //            Game.Instance.CameraRotationX += lookSpeed * look.y;
            //            Game.Instance.CameraRotationY -= lookSpeed * look.x;
        }

        // Zoom in and out with Mouse Wheel
        // TODO : should change camera field of view
   //     transform.Translate(0, 0, zoom * zoomSpeed, Space.Self);
    }
}