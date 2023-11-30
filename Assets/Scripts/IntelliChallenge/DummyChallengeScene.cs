using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using UnityEngine.UI;

public class DummyChallengeScene : MonoBehaviour
{
    public TextMeshProUGUI TMP_Text;
    public GameObject Button;

    private void Start()
    {
        //TMP_Text.text = ExecFuncsChallengeInfo.GetInstance().ChallengeInfo(gameObject.scene.name).ChllgTitle + " Presione el bot�n para simular que finaliz� el desaf�o.";
        TMP_Text.text = "";
       // Button.GetComponentInChildren<Text>().text = gameObject.scene.name + "\rFinalizado!!!";
        Button.GetComponentInChildren<Text>().text = "Finalizado";
    }
    public void ChallengeFinished()
    {
        ExecFuncsChallengeInfo.GetInstance().CompleteChallenge(gameObject.scene.name);
        string strSceneName = ExecFuncsChallengeInfo.GetInstance().SceneName();
        SceneManager.LoadScene(strSceneName);
    }
}
