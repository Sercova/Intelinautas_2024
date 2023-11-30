using UnityEngine;
using UnityEngine.SceneManagement;
using Cinemachine;


public class HUD : MonoBehaviour {

	[SerializeField]
	public GameController GCtrller;

	public void onClickMenu() {
		if (GCtrller.isPaused)
			return;
		
		GCtrller.ShowLevelMenu() ;
	}

    public void onClickResetLastItem()
    {
        if (GCtrller.isPaused)
            return;
    }
}
