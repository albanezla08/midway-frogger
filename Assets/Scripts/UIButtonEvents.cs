using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UIButtonEvents : MonoBehaviour
{
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Return))
        {
            OnStart();
        }
    }
    public void OnStart()
    {
        SceneManager.LoadScene(1);
    }

    public void OnQuit()
    {
        Application.Quit();
    }
}
