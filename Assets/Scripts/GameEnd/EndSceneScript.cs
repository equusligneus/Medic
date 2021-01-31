using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EndSceneScript : MonoBehaviour
{
    [SerializeField]
    private Ref_Int _GameResult;

    [SerializeField]
    private GameObject _WinScreenGameObject;

    [SerializeField]
    private GameObject _LoseScreenGameObject;


    private void OnEnable()
    {
        if(_GameResult.Get() == 1)
        {
            ToggleWinScreen();
        }

        else if(_GameResult.Get() == -1)
        {
            ToggleLoseScreen();
        }
    }

    private void ToggleWinScreen()
    {
        _WinScreenGameObject.SetActive(true);
        _LoseScreenGameObject.SetActive(false);
    }

    private void ToggleLoseScreen()
    {
        _WinScreenGameObject.SetActive(false);
        _LoseScreenGameObject.SetActive(true);
    }

    public void BackToMainMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }

}
