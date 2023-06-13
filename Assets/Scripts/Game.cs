using Cinemachine;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Game : MonoBehaviour
{
    public static Game Instance;
    
    [SerializeField] TMP_Dropdown dropdownCameraPosition;
    [SerializeField] TMP_Dropdown dropdownCameraLookAt;
    [SerializeField] TMP_Dropdown dropdownTimeScale;
    [SerializeField] Toggle toggleCameraPosition;
    [SerializeField] Toggle toggleCameraLookAt;
    [SerializeField] GameObject[] spaceObjects;
    [SerializeField] CinemachineVirtualCamera vcamMainCamera;
    [SerializeField] Camera camera;
    [SerializeField] float cameraDistance = 20f;
    [SerializeField] Canvas canvas;
    [SerializeField] GameObject pfLabel;
    GameObject cameraPositionObject;
    GameObject cameraLookAtObject;
    List<GameObject> labels = new List<GameObject>();

    //    float cameraRotationX, cameraRotationY;

    public GameObject CameraPositionObject { get => cameraPositionObject; set => cameraPositionObject = value; }
    public GameObject CameraLookAtObject { get => cameraLookAtObject; set => cameraLookAtObject = value; }
    public Toggle ToggleCameraLookAt { get => toggleCameraLookAt; set => toggleCameraLookAt = value; }
    public Toggle ToggleCameraPosition { get => toggleCameraPosition; set => toggleCameraPosition = value; }
//    public float CameraRotationX { get => cameraRotationX; set => cameraRotationX = value; }
//    public float CameraRotationY { get => cameraRotationY; set => cameraRotationY = value; }
    public CinemachineVirtualCamera Camera { get => vcamMainCamera; set => vcamMainCamera = value; }
    public CinemachineVirtualCamera VcamMainCamera { get => vcamMainCamera; set => vcamMainCamera = value; }

    void Awake()
    {
        Instance = this;

        foreach (var spaceObject in spaceObjects)
        {
            dropdownCameraPosition.options.Add(new TMP_Dropdown.OptionData() { text = spaceObject.name });
            dropdownCameraLookAt.options.Add(new TMP_Dropdown.OptionData() { text = spaceObject.name });
            GameObject newLabel = Instantiate(pfLabel);
            newLabel.GetComponent<TextMeshProUGUI>().SetText(spaceObject.name);
            newLabel.transform.SetParent(canvas.transform, false);
            labels.Add(newLabel);
        }
    }

    private void Start()
    {
        dropdownTimeScale.value = dropdownTimeScale.options.FindIndex(option => option.text == "1 day/s");
        dropdownCameraPosition.value = dropdownCameraPosition.options.FindIndex(option => option.text == "Spaceship");
    }

    public void OnDropdownCameraPositionChanged()
    {
        cameraLookAtObject = cameraPositionObject = Array.Find(spaceObjects, o => o.name.Equals(dropdownCameraPosition.options[dropdownCameraPosition.value].text));
        dropdownCameraLookAt.value = dropdownCameraLookAt.options.FindIndex(option => option.text == cameraLookAtObject.name);
    }

    public void OnDropdownCameraLookAtChanged()
    {
        cameraLookAtObject = Array.Find(spaceObjects, o => o.name.Equals(dropdownCameraLookAt.options[dropdownCameraLookAt.value].text));
    }

    public void OnDropdownTimeScale()
    {
        switch(dropdownTimeScale.options[dropdownTimeScale.value].text)
        {
            case "Real-time":
                Settings.TimeScale = 0.000000031709791983764586504312531708333f;
                break;
            case "1 hour/s":
                Settings.TimeScale = 0.00011415525114155251141552511415525f;
                break;
            case "1 day/s":
                Settings.TimeScale = 0.00273972602739726027397260273973f;
                break;
            case "1 month/s":
                Settings.TimeScale = 0.083333f;
                break;
            case "1 year/s":
                Settings.TimeScale = 1;
                break;
            case "500 year/s":
                Settings.TimeScale = 500;
                break;
            case "2000 year/s":
                Settings.TimeScale = 2000;
                break;
            case "10000 year/s":
                Settings.TimeScale = 10000;
                break;
            default:
                Settings.TimeScale = 0.00011415525114155251141552511415525f;
                break;
        }
    }

    public void OnToggleCameraPositionChanged()
    {
        if (!toggleCameraPosition.isOn)
        {
            cameraPositionObject = null;
        }
        else
        {
            OnDropdownCameraPositionChanged();
        }
    }

    public void OnToggleCameraLookAtChanged()
    {
        if (!toggleCameraLookAt.isOn)
        {
            cameraLookAtObject = null;
        }
        else
        {
            OnDropdownCameraLookAtChanged();
        }
    }

    public void Update()
    {
        for (int i=0; i < spaceObjects.Length; i++)
        {
            Vector3 screenPos = camera.WorldToScreenPoint(spaceObjects[i].transform.position);
            Vector2 canvasPos;
            RectTransformUtility.ScreenPointToLocalPointInRectangle(canvas.GetComponent<RectTransform>(), screenPos, null, out canvasPos);

            if (screenPos.z > 10)
            {
                labels[i].SetActive(true);
                labels[i].GetComponent<RectTransform>().anchoredPosition = canvasPos;
            }
            else
            {
                labels[i].SetActive(false);
            }
        }
    }

    public void LateUpdate()
    {
        if (cameraLookAtObject != null)
        {
            vcamMainCamera.transform.rotation = Quaternion.LookRotation(cameraLookAtObject.transform.position - vcamMainCamera.transform.position, Vector3.up);
        }
        if (cameraPositionObject != null)
        {
            if (CameraLookAtObject != null)
            {
                // orbit around object to look at
                vcamMainCamera.transform.position = CameraLookAtObject.transform.position - vcamMainCamera.transform.forward * cameraDistance;
            }
            else
            {
                // stay with object but don't face camera to it
                vcamMainCamera.transform.position = cameraPositionObject.transform.Find("CameraRoot").position;
            }
        }
    }
}
