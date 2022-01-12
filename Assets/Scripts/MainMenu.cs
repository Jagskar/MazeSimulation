using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    public Dropdown dropdown;
    public void PlayGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        string cameraOption = dropdown.options[dropdown.value].text;
        PlayerPrefs.SetString("CameraMode", cameraOption);
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
