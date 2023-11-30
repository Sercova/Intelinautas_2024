using System.Collections;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class InventoryController : MonoBehaviour {

    [Tooltip("Automatically retrieved. Items in the inventory are not included.\nOnly items in the scene.")]
    [SerializeField]
    private int ItemsInScene;

    public GameObject InventoryPanel;

    [SerializeField]
    public GameController GCtrller;
    [Tooltip("List of items shown in the inventory.")]
    public List<GameObject> Items = new List<GameObject>();


    ExecFuncsPlayerInfo EFPlayerInfo;

    private double StdDev(IEnumerable<int> arg_values)
    {
        double ret = 0;
        if (arg_values.Count() > 0)
        {
            //Compute the Average      
            double avg = arg_values.Average();
            //Perform the Sum of (value-avg)_2_2      
            double sum = arg_values.Sum(d => System.Math.Pow(d - avg, 2));
            //Put it all together      
            ret = System.Math.Sqrt((sum) / (arg_values.Count() - 1));
        }
        return ret;
    }

    private double CoefVar(ICollection<int> arg_values)
    {
        double ret = 0.0001;

        if (arg_values.Count > 0)
            ret = StdDev(arg_values) / arg_values.Average();

        return ret;
    }

    // Use this for initialization

    int[] itemTypes = new int[5];  // Needed to calculate the Variation coefficient

    void Start() {
        int i = 0;
        float itemsFactor, distFactor, coefV;

        //itemTypes[0] = 0;
        //itemTypes[1] = 0;

        foreach (GameObject obj in Items) {
            Instantiate (obj, InventoryPanel.transform);
            //obj.GetComponent<DraggableItem>().parentToReturnTo = InventoryPanel.transform;
            //obj.GetComponent<DraggableItem>().ItemID = i;
            //obj.GetComponent<DraggableItem>().instanceItem.GetComponent<SnapToAnchor>().ItemID_inst = i;
            i++;
            CountItemTypes(obj.name);
        }

        // Only items in the scene.  Not in the inventory.
        ItemsInScene = GameObject.FindGameObjectsWithTag("InvItem").Length;

        //foreach (GameObject obj in GameObject.FindGameObjectsWithTag("InvItem"))
        //{
        //    Debug.Log("SceneItem: " + obj.name);
        //    ItemsInScene++;
        //    CountItemTypes(obj.name);
        //}

        EFPlayerInfo = GameObject.Find("ExecFuncsPlayerInfo").GetComponent<ExecFuncsPlayerInfo>();

        itemsFactor = (Items.Count + ItemsInScene) / 15.0f;
        Debug.Log("Cant. Items: " + this.TotalItems());

        if (itemTypes.Length > 0)
            coefV = (float)CoefVar(itemTypes);
        else
            coefV = 0.001f;
        Debug.Log("Coef Variacion: " + coefV);

        //Debug.Log("arg_playerage InvCtrller:" + ExecFuncsMngr.Instance.PlayerAge());

        // *** AVERAGE DISTANCE  *** //
        int tokensQ = GameObject.FindGameObjectsWithTag("Token").Length;
        if (tokensQ < 2)
        {
            Debug.Log("Dist. Promedio: " + 1.0f / 100);
            EFPlayerInfo.SetFeatures(this.TotalItems(), 1.0f / 100, coefV);
            return;
        }

        int distCounter = 0;
        float tokenDist = 0.0f;
        Vector2 PosIni = new Vector2(0.0f,0.0f);
        //for (int tokenOrder = 0; tokenOrder < tokensQ; tokenOrder++)
        //{
        //    foreach (GameObject token in GameObject.FindGameObjectsWithTag("CheckPoint" ))
        //    {
        //        if (token.GetComponent<Token>() == null)
        //            continue;

        //        int ancOrd = token.GetComponent<Token>().tokenOrder;
        //        if (ancOrd == tokenOrder)
        //        {
        //            Vector2 PosFin = token.transform.position;

        //            if (PosFin != PosIni)
        //            {
        //                if (PosIni != Vector2.zero)
        //                {
        //                    //Debug.Log("->" + anchor.name + ": " + Vector2.Distance(PosFin, PosIni));
        //                    tokenDist += Vector2.Distance(PosFin, PosIni);
        //                    distCounter++;
        //                }
        //                PosIni = PosFin;
        //            }
        //        }
        //    }
        //}

        EFPlayerInfo.SetFeatures(this.TotalItems(), tokenDist / distCounter, coefV);

        distFactor = (tokenDist / distCounter) / 100;
        Debug.Log("Dist. Promedio: " + distFactor);

        //Debug.Log("Factor Complejidad Nivel: " + coefV * itemsFactor / distFactor);
        //Debug.Log(EFPlayerInfo.GetFeatures());

        //		imgComp = InventoryPanel.GetComponent<Image> ();
    }

    void CountItemTypes(string arg_name)
    {
        string tmp_names;
        if (arg_name.IndexOf("_instance") == -1)
            tmp_names = arg_name;
        else
            tmp_names = arg_name.Substring(0, arg_name.IndexOf("_instance"));

        // *** Add dimensions to int[] itemTypes when adding cases *** //
        switch (tmp_names)
        {
            case "Item_Rock":
                itemTypes[0]++;
                break;
            case "Item_Spring01":
                itemTypes[1]++;
                break;
            case "Item_Spring02":
                itemTypes[1]++;
                break;
            case "Item_RoundRock":
                itemTypes[2]++;
                break;
            case "Item_SquareRock":
                itemTypes[3]++;
                break;
            case "Item_BoxGeneric":
                itemTypes[3]++;
                break;
            case "Quila":
                itemTypes[4]++;  // Temporal Challenge Type
                break;
                //case "":
                //    itemTypes[n]++;
                //    break;
        }
    }

    void Update() {
  //      if (GCtrller.isPlaying)
  //      {
  //          GCtrller.inventoryEnabled = true;
		//}
		//else {
		//	GCtrller.inventoryEnabled = false;
		//}
	}

    public int TotalItems()
    {
        // Items in the inventory + Items in Scene + 
        int ItemsInScene_Aux = 0;

        foreach (string strTAG in EFPlayerInfo.ExecFuncsTags)
        {
            if (strTAG.IndexOf("Challenge_") == 0)
            {
                //Debug.Log("Challenges: " + strTAG + " (" + GameObject.FindGameObjectsWithTag(strTAG).Length + ")");
                // Two times for each challenge ("_start" and "_end")
                ItemsInScene_Aux += GameObject.FindGameObjectsWithTag(strTAG).Length;
            }
        }

        return Items.Count + ItemsInScene + ItemsInScene_Aux;
    }
}
