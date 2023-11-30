using UnityEngine;


public class CheckPoint : MonoBehaviour
{
    [Tooltip("The first one should be true")]
    public bool ChP_Reached;  // MUST BE false in the inspector

    [Tooltip("The first one should be 1")]
    [SerializeField]
    private int ChP_Order;



    private void OnTriggerEnter2D(Collider2D arg_Collider)
    {
        // Turn off all checkpoints except the collided one.
        if (arg_Collider.tag == "Player") 
        {
            GameObject[] CheckPoints = GameObject.FindGameObjectsWithTag("CheckPoint");
            foreach (GameObject gamObj in CheckPoints)
            {
                CheckPoint chkpnt = gamObj.gameObject.GetComponent<CheckPoint>();
                chkpnt.ChP_Reached = false;
            }
            this.ChP_Reached = true;
        }

    }

    public static Vector3 ChP_LastReachedPosition()
    {
        Vector3 posResult = Vector3.zero;

        GameObject[] CheckPoints = GameObject.FindGameObjectsWithTag("CheckPoint");
        foreach (GameObject gamObj in CheckPoints)
        {
            CheckPoint chkpnt = gamObj.gameObject.GetComponent<CheckPoint>();
            if (chkpnt.ChP_Reached)
            {
                posResult = chkpnt.transform.position;
                return posResult;
            }
        }
        return posResult;
    }

}
