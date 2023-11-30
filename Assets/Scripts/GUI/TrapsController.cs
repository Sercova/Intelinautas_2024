using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TrapsController : MonoBehaviour
{
    private bool TrapsTriggered = false;
    private bool AllTrapsTriggered = false;
    private float internalTimer;

    #region Challenge Intro
    [System.Serializable]
    public struct ChallengeItem
    {
        public GameObject GameObj;
        public float ShowTimeLapse;
    }

    [Tooltip("Game controller object in the scene")]
    //[SerializeField]
    public GameController GCtrller;

    [Tooltip("Prefab type Panel_Screen_Msg")]
    [SerializeField]
    private GameObject MsgObj;

    [Tooltip("Cinemachine camera used for watching")]
    [SerializeField]
    private GameObject CM_Watching_1;

    [Tooltip("Cinemachine camera used to switch with CM_Watching_1")]
    [SerializeField]
    private GameObject CM_Watching_2;

    [Tooltip("Dummy used for watching.")]
    [SerializeField]
    private GameObject Dummy;

    [Tooltip("Entities which belong a challenge and time to show each one.  The first one should be the challenge's start checkpoint.")]
    [SerializeField]
    public List<ChallengeItem> ChallengeItems = new List<ChallengeItem>();

    #endregion

    #region Traps Definition
    [System.Serializable]
    public struct Trap
    {
        public GameObject GameObj;
        public float SwitchTime;
    }

    [Tooltip("Traps which are going to be activated and deactivated.")]
    [SerializeField]
    public List<Trap> Traps = new List<Trap>();
    private List<Trap> Traps_wasted = new List<Trap>();

    #endregion
    public bool firstItem;

    private void Start()
    {
        // Necessary to execute this.ShowMessage() from OnTriggerEnter2D of Challenge.cs
        firstItem = true;
    }

    public IEnumerator ShowMessage()
    {
        //Debug.Log("ShowMessage(): INICIO");
        // *** The message always is the first item in TrapsController *** //

        //Traps are triggered at the same time than challenge's showtime starts.
        internalTimer = 0.0f;
        TrapsTriggered = true;

        // Disable HUD Buttons
        GCtrller.isPaused = true;
        GCtrller.retryButton.interactable = false;
        GCtrller.menuButton.interactable = false;

        GCtrller.isStopped = true;
        GCtrller.isPlaying = false;
        //GCtrller.ScrObjSpeedDB.AddSpeedMeasurement(new ExecFuncsSpeedData("IDLE_MSG"));
        GCtrller.CAM_Ctrller.ZoomIn(GCtrller.IntelliChar);
        //GCtrller.watchingFinished = true;  // This activates GCtrller.CM_CamWide camera.

        TextMeshProUGUI Msg;
        Msg = MsgObj.transform.GetChild(0).GetComponent<TextMeshProUGUI>();
        if (Msg.text == "")
            Msg.text = "Prepárate para un desafío...";
        MsgObj.transform.GetChild(0).GetComponent<TextMeshBlink>().animSpeedInSec = 0;

        //// Deactivate CM_CamWide which was activated by GCtrller.watchingFinished = true
        //GCtrller.CM_CamWide.SetActive(false);
        //GCtrller.watchingFinished = false;  // This activates GCtrller.CM_CamWide camera.

        if (ChallengeItems.Count > 0)
        {
            ChallengeItem item = ChallengeItems[0];

            if (CM_Watching_2.activeSelf)
            {
                CM_Watching_2.SetActive(false);
                CM_Watching_1.GetComponent<Cinemachine.CinemachineVirtualCamera>().Follow = item.GameObj.transform;
                CM_Watching_1.SetActive(true);
            }
            else
            {
                CM_Watching_1.SetActive(false);
                CM_Watching_2.GetComponent<Cinemachine.CinemachineVirtualCamera>().Follow = item.GameObj.transform;
                CM_Watching_2.SetActive(true);
            }

            MsgObj.SetActive(true);
            // Message is visible while the first challenge item is focused
            MsgObj.gameObject.GetComponent<ScreenMsg>().timeElapse = item.ShowTimeLapse;
                       
            yield return new WaitForSeconds(item.ShowTimeLapse);

            MsgObj.SetActive(false);  // Message is visible only for the first challenge's item
            
            
        }

        //if (ChallengeItems.Count == 1)
        //{
        //    // Only there is a message to show, not a challenge
        //    GCtrller.watchingFinished = true;
        //    GCtrller.isPaused = false;
        //}

        firstItem = false;

    }


    public IEnumerator ShowChallenge()
    {
        //Debug.Log("ShowChallenge(): Inicio");
        int elements = ChallengeItems.Count;
        if (elements > 1)
        {
            // Start at the second element in ChallengeItems array
            // Because the first item was shown with then ShowMessage() coroutine.
            for (int i = 1; i < elements; i++)
            {


                ChallengeItem item = ChallengeItems[i];
                //Debug.Log("item.name: " + item.GameObj.name);
                if (CM_Watching_2.activeSelf)
                {
                    CM_Watching_2.SetActive(false);
                    CM_Watching_1.GetComponent<Cinemachine.CinemachineVirtualCamera>().Follow = item.GameObj.transform;
                    CM_Watching_1.SetActive(true);
                }
                else
                {
                    CM_Watching_1.SetActive(false);
                    CM_Watching_2.GetComponent<Cinemachine.CinemachineVirtualCamera>().Follow = item.GameObj.transform;
                    CM_Watching_2.SetActive(true);
                }

                yield return new WaitForSeconds(item.ShowTimeLapse);
            }
        }

        // Set back the activation to the GCtrller.CM_CamWide camera
        if (CM_Watching_2.activeSelf)
        {
            CM_Watching_2.SetActive(false);
        }
        else if (CM_Watching_1.activeSelf)
        {
            CM_Watching_1.SetActive(false);
        }

        GCtrller.CAM_Ctrller.ZoomIn(GCtrller.IntelliChar);
        //GCtrller.CM_CamWide.SetActive(true);
        //GCtrller.watchingFinished = true;
        CM_Watching_1.SetActive(false);
        CM_Watching_2.SetActive(false);

        // Enable HUD buttons
        GCtrller.retryButton.interactable = true;
        GCtrller.menuButton.interactable = true;
        GCtrller.isPaused = false;
        //Debug.Log("ShowChallenge(): Sacar Pausa...");
        //Dummy.transform.position = GCtrller.CM_CamWide.GetComponent<Cinemachine.CinemachineVirtualCamera>().Follow.position;
        //CM_Watching_1.GetComponent<Cinemachine.CinemachineVirtualCamera>().Follow = Dummy.transform;
        //CM_Watching_2.GetComponent<Cinemachine.CinemachineVirtualCamera>().Follow = Dummy.transform;

    }


    private void LateUpdate()
    {
        if (!TrapsTriggered || AllTrapsTriggered)
            return;

        internalTimer += Time.deltaTime;

        foreach (Trap trap in Traps)
        {
            if (internalTimer > trap.SwitchTime && !Traps_wasted.Contains(trap))
            {
                Traps_wasted.Add(trap);
                if (Traps.Count == Traps_wasted.Count)
                    AllTrapsTriggered = true;

                trap.GameObj.SetActive(!trap.GameObj.activeSelf);
            }
        }


    }

    public void HideMessage()
    {
        firstItem = false;
        MsgObj.GetComponent<ScreenMsg>().HideMessage();
    }
}