using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DialogPanel : MonoBehaviour {

    public GameObject lvlMenu;

    public void YesResponse()
    {
        ExecFuncsMngr.Instance.isRestarted = false;

        PlayerPrefs.SetInt("qRestart", 0);
        PlayerPrefs.SetInt("qPause", 0);
        PlayerPrefs.SetInt("qChallengeTempFails", 0);

        SceneManager.LoadScene("LevelSelector");
    }

    public void NoResponse()
    {
        gameObject.SetActive(false);
        lvlMenu.SetActive(true);
    }

}
