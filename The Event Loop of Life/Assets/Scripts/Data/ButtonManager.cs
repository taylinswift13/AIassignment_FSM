using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ButtonManager : MonoBehaviour
{
    public void OnClick_Start()
    {
        SceneManager.LoadScene("Game", LoadSceneMode.Single);
    }
    public void OnClick_Back()
    {
        SceneManager.LoadScene("Menu", LoadSceneMode.Single);
    }
    public void OnClick_Quit()
    {
        Application.Quit();
    }
}
