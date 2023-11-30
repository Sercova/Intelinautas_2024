using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SaveFacilitatorState
{
    [System.Serializable]
    public struct FacilitatorData
    {
        public string UniqueID;
        public int Activated;
    }

    public string facilitatorID;
    public List<FacilitatorData> FacilitatorObjects = new List<FacilitatorData>();

    public string ToJson()
    {
        return JsonUtility.ToJson(this);
    }

    public void LoadFromJson(string arg_Json)
    {
        JsonUtility.FromJsonOverwrite(arg_Json, this);
    }
}

public interface ISaveableFacilitatorState
{
    void PopulateFacilitatorState(ref SaveFacilitatorState arg_SavedFacilitatorState);
    void LoadFromFacilitatorState(SaveFacilitatorState arg_SavedFacilitatorState);
}
