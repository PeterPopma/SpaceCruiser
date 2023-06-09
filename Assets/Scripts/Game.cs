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
    [SerializeField] Toggle toggleCameraPosition;
    [SerializeField] Toggle toggleCameraLookAt;
    GameObject cameraPositionObject;
    GameObject cameraLookAtObject;

    public GameObject CameraPositionObject { get => cameraPositionObject; set => cameraPositionObject = value; }
    public GameObject CameraLookAtObject { get => cameraLookAtObject; set => cameraLookAtObject = value; }
    public Toggle ToggleCameraLookAt { get => toggleCameraLookAt; set => toggleCameraLookAt = value; }
    public Toggle ToggleCameraPosition { get => toggleCameraPosition; set => toggleCameraPosition = value; }

    void Awake()
    {
        Instance = this;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnDropdownCameraPositionChanged()
    {

    }

    public void OnDropdownCameraLookAtChanged()
    {

    }

    public void OnToggleCameraPositionChanged()
    {

    }

    public void OnToggleCameraLookAtChanged()
    {

    }

    public void LateUpdate()
    {
        
    }
}
