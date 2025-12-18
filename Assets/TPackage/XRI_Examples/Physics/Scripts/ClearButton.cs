using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.XR.Interaction.Toolkit;

public class ClearButton : MonoBehaviour
{
    [Header("이동할 씬 이름")]
    public string mainSceneName = "MainScene";

    public void OnButtonPressed(SelectEnterEventArgs args)
    {
        SceneManager.LoadScene(mainSceneName);
    }
}
