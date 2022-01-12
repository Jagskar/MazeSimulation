using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OptionsMenu : MonoBehaviour
{
    static string optionSelected;
    public Dropdown dropdown;

    public void ConfirmOptions()
    {
        string cameraOption = dropdown.options[dropdown.value].text;
        PlayerPrefs.SetString("CameraMode", cameraOption);
        SetOption(cameraOption);
    }
    public void SetOption(string option)
    {
        optionSelected = option;
    }
}
