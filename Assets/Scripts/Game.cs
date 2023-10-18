using Cinemachine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Game : MonoBehaviour
{
    public static Game Instance;
    
    [SerializeField] TMP_Dropdown dropdownCameraFollow;
    [SerializeField] TMP_Dropdown dropdownCameraLookAt;
    [SerializeField] TMP_Dropdown dropdownTimeScale;
    [SerializeField] TMP_Dropdown dropdownSpaceshipSpeed;
    [SerializeField] TMP_Dropdown dropdownSpaceshipDestination;
    [SerializeField] TMP_Dropdown dropdownPlanetSize;
    [SerializeField] Toggle toggleCameraPosition;
    [SerializeField] Toggle toggleCameraLookAt;
    [SerializeField] Toggle toggleTrails;
    [SerializeField] GameObject[] spaceObjects;
    [SerializeField] Canvas canvas;
    [SerializeField] GameObject pfLabel;
    [SerializeField] Spaceship spaceShip;
    [SerializeField] TextMeshProUGUI textTime;
    [SerializeField] GameInput gameInput;

    List<GameObject> labels = new List<GameObject>();

    public Toggle ToggleCameraLookAt { get => toggleCameraLookAt; set => toggleCameraLookAt = value; }
    public Toggle ToggleCameraPosition { get => toggleCameraPosition; set => toggleCameraPosition = value; }
    private float simulationTime;    // in years

    void Awake()
    {
        Instance = this;

        foreach (var spaceObject in spaceObjects)
        {
            dropdownCameraFollow.options.Add(new TMP_Dropdown.OptionData() { text = spaceObject.name });
            dropdownCameraLookAt.options.Add(new TMP_Dropdown.OptionData() { text = spaceObject.name });
            dropdownSpaceshipDestination.options.Add(new TMP_Dropdown.OptionData() { text = spaceObject.name });
            GameObject newLabel = Instantiate(pfLabel);
            newLabel.GetComponent<TextMeshProUGUI>().SetText(spaceObject.name);
            newLabel.transform.SetParent(canvas.transform, false);
            labels.Add(newLabel);
        }
    }

    private void Start()
    {
        dropdownTimeScale.value = dropdownTimeScale.options.FindIndex(option => option.text == "1 day/s");
        dropdownCameraFollow.value = dropdownCameraFollow.options.FindIndex(option => option.text == "Earth");
        dropdownCameraLookAt.value = dropdownCameraLookAt.options.FindIndex(option => option.text == "Earth");
        simulationTime = 0;
    }

    public void OnDropdownDestinationChanged()
    {
        spaceShip.Destination = Array.Find(spaceObjects, o => o.name.Equals(dropdownSpaceshipDestination.options[dropdownSpaceshipDestination.value].text));
    }

    public void OnDropdownSpaceshipSpeedChanged()
    {
        switch(dropdownSpaceshipSpeed.value)
        {
            case 0:
                spaceShip.Speed = 16900;
                break;
            case 1:
                spaceShip.Speed = 299792458;
                break;
        }

    }

    public void OnDropdownCameraFollowChanged()
    {
        /* cameraLookAtObject = */
        gameInput.ChangeFollowObject(Array.Find(spaceObjects, o => o.name.Equals(dropdownCameraFollow.options[dropdownCameraFollow.value].text)));
//        dropdownCameraLookAt.value = dropdownCameraLookAt.options.FindIndex(option => option.text == cameraLookAtObject.name);
//        toggleCameraLookAt.isOn = true;
        toggleCameraPosition.isOn = true;
    }

    public void OnDropdownCameraLookAtChanged()
    {
        gameInput.ChangeLookAtObject(Array.Find(spaceObjects, o => o.name.Equals(dropdownCameraLookAt.options[dropdownCameraLookAt.value].text)));
        toggleCameraLookAt.isOn = true;
    }

    public void OnDropdownPlanetSizeChanged()
    {
        switch (dropdownPlanetSize.options[dropdownPlanetSize.value].text)
        {
            case "100x":
                UpdatePlanetSize(100);
                break;
            case "1x":
                UpdatePlanetSize(1);
                break;
        }
    }

    private void UpdatePlanetSize(int newSize)
    {
        if (newSize == 1 && Settings.planetSize == 100)
        {
            foreach (var planet in spaceObjects)
            {
                planet.transform.localScale *= 0.01f;
            }
            Settings.planetSize = 1;
        }
        if (newSize == 100 && Settings.planetSize == 1)
        {
            foreach (var planet in spaceObjects)
            {
                planet.transform.localScale *= 100f;
            }
            Settings.planetSize = 100;
        }
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
            case "1 week/s":
                Settings.TimeScale = 0.01917808219178082191780821917811f;
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

    public void OnToggleCameraFollowChanged()
    {
        if (!toggleCameraPosition.isOn)
        {
            gameInput.ChangeFollowObject(null);
        }
        else
        {
            OnDropdownCameraFollowChanged();
        }
    }

    public void OnToggleCameraLookAtChanged()
    {
        if (!toggleCameraLookAt.isOn)
        {
            gameInput.ChangeLookAtObject(null);
        }
        else
        {
            OnDropdownCameraLookAtChanged();
        }
    }
    public void OnToggleTrailsChanged()
    {
        if (toggleTrails.isOn)
        {
            SetTrailsActive(true);
        }
        else
        {
            SetTrailsActive(false);
        }
    }

    private void SetTrailsActive(bool active)
    {
        foreach(GameObject spaceObject in spaceObjects)
        {
            if (spaceObject.GetComponent<TrailRenderer>() != null)
            {
                spaceObject.GetComponent<TrailRenderer>().enabled = active;
            }
        }
    }

    private string CalculateTime()
    {
        DateTime time = DateTime.Now;
        int yearsPassed = (int)simulationTime;
        time = time.AddYears(yearsPassed);
        time = time.AddSeconds((simulationTime-yearsPassed)*365*24*60*60);
        return time.ToString(CultureInfo.InvariantCulture);
    }

    public void Update()
    {
        simulationTime += Time.deltaTime * Settings.TimeScale;
        textTime.text = CalculateTime();

        for (int i=0; i < spaceObjects.Length; i++)
        {
            Vector3 screenPos = Camera.main.WorldToScreenPoint(spaceObjects[i].transform.position);
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

}
