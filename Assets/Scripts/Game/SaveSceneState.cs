using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SaveSceneState
{
    [System.Serializable]
    public struct SceneData
    {
        public string UniqueID;
        public float GObjPos_X;
        public float GObjPos_Y;
        public float GObjPos_Z;
        public float GObjLocalScale_X;
        public float GObjLocalScale_Y;
    }

    public List<SceneData> SceneObjects = new List<SceneData>();
    public List<SceneData> DestroyedObjects = new List<SceneData>();

    public string ToJson()
    {
        return JsonUtility.ToJson(this);
    }

    public void LoadFromJson(string arg_Json)
    {
        JsonUtility.FromJsonOverwrite(arg_Json, this);
    }
}

public interface ISaveableSceneState
{
    void PopulateSceneState(SaveSceneState arg_SavedSceneState);
    void LoadFromSceneState(SaveSceneState arg_SavedSceneState);
}
