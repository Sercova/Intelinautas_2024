using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IntChllge_Giant : MonoBehaviour
{
    public bool vulnerable;
    public GameObject Alarm;
    public ContactFilter2D ContactFilter;
    public int CreatureID;

    private GameController GCtrllr;

    private void Start()
    {
        GCtrllr = GameObject.Find("GameController").GetComponent<GameController>();
        if (ExecFuncsChallengeInfo.GetInstance().ChallengeIsCompleted(this.CreatureID))
        {
            int lastChallenge = ExecFuncsChallengeInfo.GetInstance().LastChallengeId();
            foreach (CreatureAI ai in FindObjectsOfType<CreatureAI>())
            {
                if (ai.CreatureID == lastChallenge)
                {
                    ai.enabled = true;
                    GCtrllr.CAM_Ctrller.ZoomOut(ai.gameObject, 5);
                }
            }

            SaveSceneState.SceneData sceneData = new SaveSceneState.SceneData();

            sceneData.UniqueID  = this.gameObject.GetComponent<ExecFunc_UniqueID>().uniqueId;
            sceneData.GObjPos_X = this.gameObject.transform.position.x;
            sceneData.GObjPos_Y = this.gameObject.transform.position.y;
            sceneData.GObjPos_Z = this.gameObject.transform.position.z;

            sceneData.GObjLocalScale_X = this.gameObject.transform.localScale.x;
            sceneData.GObjLocalScale_Y = this.gameObject.transform.localScale.y;

            ExecFuncsChallengeInfo.GetInstance().DestroyedSceneObjects.Add(sceneData);

            Destroy(this.gameObject);
            return;
        }
        vulnerable = false;
    }

    private void OnTriggerExit2D(Collider2D arg_collision)
    {
        if (arg_collision.gameObject.tag == "Player")
            vulnerable = false;
    }

    private void OnTriggerEnter2D(Collider2D arg_collision)
    {
        if (arg_collision.gameObject.tag == "Player")
            vulnerable = true;

        if (arg_collision.gameObject.tag == "Abbys")
        {
            vulnerable = true;
            Disappear(gameObject);
        }

        //else
        //    vulnerable = false;
    }

    private void OnTriggerStay2D(Collider2D arg_collision)
    {
        if (arg_collision.gameObject.tag == "Player")
            vulnerable = true;
        //else
        //    vulnerable = false;
    }
    private void Update()
    {
        if (vulnerable)
            Alarm.SetActive(true);
        else
            Alarm.SetActive(false);



        if (Input.GetMouseButtonDown(0) && vulnerable)
        {
            Vector2 touchPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            //RaycastHit2D hit_collider = Physics2D.Raycast(touchPos, Vector2.zero, Mathf.Infinity, LayerMask.GetMask("Intelli_Motivator"));
            Collider2D[] Colliders = new Collider2D[10];
            bool isTouched = Physics2D.OverlapCircle(touchPos, 0.5f, ContactFilter, Colliders) > 0;

            //if (hit_collider.collider != null)
            if (isTouched)
            {
                // Completed challenges are deleted from the scene
                GCtrllr.LoadChallenge(this.CreatureID);   
            }

        }
    }

    private void Disappear(GameObject arg_gameObj)
    {
        if (vulnerable)
            Destroy(arg_gameObj);
    }

}