using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrowingPlant : MonoBehaviour, ISaveableFacilitatorState
{
    public bool isRoot;
    public int plantSize;
    public GameObject plant;

    private int exit = 0;
    private string uniqueID;

    private void Start()
    {
        uniqueID = this.gameObject.GetComponent<ExecFunc_UniqueID>().uniqueId;

        if (ExecFuncsChallengeInfo.GetInstance().ChallengesCompletedIn(gameObject.scene.name) > 0)
        {
            // It is returning from a challenge
            if (FileManager.LoadFromFile("Facilitators.dat", out var json))
            {
                SaveFacilitatorState facilitatorState = new SaveFacilitatorState();
                facilitatorState.LoadFromJson(json);
                this.LoadFromFacilitatorState(facilitatorState);
                //Debug.Log("Load complete!!!");
            }

        } else
        {
            // Scene is loading for the first time
        }
    }

    private void OnTriggerEnter2D(Collider2D arg_collision)
    {
        if (exit > 30)
            return;

        if (arg_collision.gameObject.layer == LayerMask.NameToLayer("Intelli_Player_Ground_Detect")
                && isRoot)
        {
            Grow();
        }
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Vector2 touchPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            RaycastHit2D hit_collider = Physics2D.Raycast(touchPos, Vector2.zero, Mathf.Infinity, LayerMask.GetMask("Intelli_Facilitator"));

            bool isTouched = hit_collider.collider != null && hit_collider.collider.gameObject==this.gameObject;
            if (isTouched)
            {
                Grow() ;
            }

        }
    }

    private void Grow()
    {
        gameObject.GetComponent<BoxCollider2D>().isTrigger = false;
        GetComponent<GrowingPlant>().isRoot = false;
        for (int i = 1; i < plantSize; i++)
        {
            GameObject instPlant = Instantiate(plant, this.gameObject.transform);
            SpriteRenderer sprtRender = instPlant.GetComponent<SpriteRenderer>();
            instPlant.transform.position = new Vector3(gameObject.transform.position.x, gameObject.transform.position.y + i * sprtRender.size.y);
            exit++;
        }
    }


    public void PopulateFacilitatorState(ref SaveFacilitatorState arg_SavedFacilitatorState)
    {

        //facilitatorData.UniqueID = arg_SavedFacilitatorState.FacilitatorObjects[arg_SavedFacilitatorState.FacilitatorObjects.Count].UniqueID;
        if (this.uniqueID == arg_SavedFacilitatorState.facilitatorID)
        {
            SaveFacilitatorState.FacilitatorData facilitatorData = new SaveFacilitatorState.FacilitatorData();
            facilitatorData.UniqueID = this.uniqueID;
            // Activated plants are not triggers
            facilitatorData.Activated = !gameObject.GetComponent<BoxCollider2D>().isTrigger ? 1 : 0;

            arg_SavedFacilitatorState.FacilitatorObjects.Add(facilitatorData);
        }
        
    }

    public void LoadFromFacilitatorState(SaveFacilitatorState arg_SavedFacilitatorState)
    {
        foreach (SaveFacilitatorState.FacilitatorData facilitatorData in arg_SavedFacilitatorState.FacilitatorObjects)
        {
            if (uniqueID == facilitatorData.UniqueID && facilitatorData.Activated == 1)
            {
                Grow();
            }
        }
    }
}
