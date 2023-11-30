using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public enum EnableButton { Visible, Invisible, Faint };

public class GameController : MonoBehaviour, ISaveableSceneState

{
    [SerializeField]
    public UnityEngine.UI.Button retryButton;

    [SerializeField]
    public UnityEngine.UI.Button menuButton;

    bool oneClick = false;
    float dbClickTimer;
    float dbClickDelay = 0.2f; // seconds

    public GameObject IntelliChar;
    public Transform SpawnLocation;
    public CAMController CAM_Ctrller;
	public bool isPaused = false;
	public float playerDirX = 1.0f;
    public bool isStopped = false;
    public bool isPlaying = false;

    [SerializeField]
    private float maxVelocity = 20.0f;
    [SerializeField]
    private GameObject levelMenu;
    private Rigidbody2D IntelliChar_RB;



    void Start()
    {
        if (ExecFuncsChallengeInfo.GetInstance().ChallengesCompletedIn(gameObject.scene.name)==0)
        {
            // Scene is loading for the first time
            IntelliChar.transform.position = SpawnLocation.position;
            // Initialize destroyed gameobjects list
            ExecFuncsChallengeInfo.GetInstance().DestroyedSceneObjects = new List<SaveSceneState.SceneData>();
        }
        else
        {
            // Scene is comming from a challenge
            if (FileManager.LoadFromFile("CurrentScene.dat", out var json))
            {
                SaveSceneState sceneState = new SaveSceneState();
                sceneState.LoadFromJson(json);
                this.LoadFromSceneState(sceneState);
                //Debug.Log("Load complete!!!");
                
            }

        }

        IntelliChar_RB = IntelliChar.GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!levelMenu.GetComponent<LevelMenu>().ConfirmDialog.activeSelf)
        {
            if (Input.GetKeyDown(KeyCode.Escape) && isPaused == false)
            {
                ShowLevelMenu();
            }
            else if (Input.GetKeyDown(KeyCode.Escape) && isPaused == true)
            {
                HideLevelMenu();
            }
        }
		
        if (IntelliChar_RB.velocity.magnitude > maxVelocity)
            CAM_Ctrller.ZoomOut(IntelliChar);


        if (Input.GetMouseButtonDown(0))
        {
            if (!oneClick) // first click no previous clicks
            {
                oneClick = true;

                dbClickTimer = Time.time; // save the current time
                                          // do one click things;
            }
            else
            {
                oneClick = false; // found a double click, now reset
                CAM_Ctrller.SwitchZoom(IntelliChar);
            }
        }
        if (oneClick)
        {
            // if the time now is delay seconds more than when the first click started. 
            if ((Time.time - dbClickTimer) > dbClickDelay)
            {
                //basically if thats true its been too long and we want to reset so the next click is simply a single click and not a double click.
                oneClick = false;
            }
        }
    }

    SaveFacilitatorState facilitatorState;
    public void LoadChallenge(int arg_CreatureID)
    {
        facilitatorState = new SaveFacilitatorState();

        // Save the scene data to return after the challenge
        SaveSceneState sceneState = new SaveSceneState();
        
        //GameController gameCtrller = new GameController();
        //gameCtrller.PopulateSaveSceneState(sceneState); 
        this.PopulateSceneState(sceneState);

        if (FileManager.WriteToFile("CurrentScene.dat", sceneState.ToJson()))
        {
            Debug.Log("CurrentScene Save successful");
        } else
        {
            Debug.Log("CurrentScene Save fail");
        }

        if (FileManager.WriteToFile("Facilitators.dat", facilitatorState.ToJson()))
        {
            Debug.Log("Facilitator Save successful");
        }
        else
        {
            Debug.Log("Facilitator Save fail");
        }

        // TODO: Load the appropiate challenge
        string strChallengeSceneName = ExecFuncsChallengeInfo.GetInstance().ChallengeSceneName(arg_CreatureID);
        SceneManager.LoadScene(strChallengeSceneName);
    }

    public void PopulateSceneState(SaveSceneState arg_SavedSceneState)
    {
        SaveSceneState.SceneData sceneData = new SaveSceneState.SceneData();

        arg_SavedSceneState.DestroyedObjects = new List<SaveSceneState.SceneData>(ExecFuncsChallengeInfo.GetInstance().DestroyedSceneObjects);

        foreach (ExecFunc_UniqueID UniqueIDComp in FindObjectsOfType<ExecFunc_UniqueID>())
        {
            sceneData.UniqueID = UniqueIDComp.uniqueId;
            sceneData.GObjPos_X = UniqueIDComp.transform.position.x;
            sceneData.GObjPos_Y = UniqueIDComp.transform.position.y;
            sceneData.GObjPos_Z = UniqueIDComp.transform.position.z;

            sceneData.GObjLocalScale_X = UniqueIDComp.transform.localScale.x;
            sceneData.GObjLocalScale_Y = UniqueIDComp.transform.localScale.y;

            arg_SavedSceneState.SceneObjects.Add(sceneData);

            GrowingPlant growingPlant = UniqueIDComp.GetComponent<GrowingPlant>();
            if (growingPlant)
            {
                // It is a plant facilitator
                facilitatorState.facilitatorID = UniqueIDComp.uniqueId;
                growingPlant.PopulateFacilitatorState(ref facilitatorState);
            }

        }
    }

    public void LoadFromSceneState(SaveSceneState arg_SavedSceneState)
    {
        foreach (SaveSceneState.SceneData sceneData in arg_SavedSceneState.SceneObjects)
        {
                foreach (ExecFunc_UniqueID uniqueID in FindObjectsOfType<ExecFunc_UniqueID>())
                {
                    if (uniqueID.uniqueId == sceneData.UniqueID)
                    {
                        uniqueID.gameObject.transform.position = new Vector3(sceneData.GObjPos_X, sceneData.GObjPos_Y, sceneData.GObjPos_Z);
                        uniqueID.gameObject.transform.localScale = new Vector3(sceneData.GObjLocalScale_X, sceneData.GObjLocalScale_Y, 1.0f);
                    }
                }
        }

        foreach (SaveSceneState.SceneData sceneData in arg_SavedSceneState.DestroyedObjects)
        {
            foreach (ExecFunc_UniqueID uniqueID in FindObjectsOfType<ExecFunc_UniqueID>())
            {
                if (uniqueID.uniqueId == sceneData.UniqueID)
                {
                    Destroy(uniqueID.gameObject);
                }
            }
        }
    }
	
	public void HideLevelMenu() {
        levelMenu.SetActive(false);
        Time.timeScale = 1;
        isPaused = false;
        EnableHUDButttons(EnableButton.Visible); 
    }

    public void ShowLevelMenu() {
        ExecFuncsMngr.Instance.DB.AddMeasurement("LM");
        Time.timeScale = 0;
        isPaused = true;

        EnableHUDButttons(EnableButton.Invisible);

        levelMenu.SetActive(true);
    }

    public void EnableHUDButttons(EnableButton arg_enabletype) {
		
		// *** Enable/Disable, Visible/Invisible/Faint buttons in GUI *** //
		
        // float alpha = 1.0f;

        // Color tmpColorVisible = menuButton.image.color;
        // Color tmpColorInvisible = menuButton.image.color;

        // tmpColorVisible.a = alpha;

        // switch (arg_enabletype)
        // {
            // case EnableButton.Visible:
                // alpha = 1.0f;
                // break;
            // case EnableButton.Invisible:
                // alpha = 0.0f;
                // break;
            // case EnableButton.Faint:
                // alpha = 0.5f;
                // break;
            // default:
                // break;
        // }

        // tmpColorVisible.a = alpha;
        // tmpColorInvisible.a = 0.0f;
        // retryButton.image.color = tmpColorVisible;
        // menuButton.image.color = tmpColorVisible;

        // if (arg_enabletype== EnableButton.Invisible)
        // {
            // retryButton.interactable = false;
            // menuButton.interactable = false;
        // } else
        // {
            // retryButton.interactable = true;
            // menuButton.interactable = true;
        // }

        // if (GArea.droppedItems.Count == 0)
        // {
            // retryButton.interactable = false;
            // retryButton.image.color = tmpColorInvisible;
        // }

    }
}
