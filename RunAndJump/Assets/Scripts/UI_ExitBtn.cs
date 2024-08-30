using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UI_ExitBtn : MonoBehaviour
{
    public void Exit()
    {
        SceneManager.LoadScene("TitleScene");
        Time.timeScale = 1.0f;

    }
}
