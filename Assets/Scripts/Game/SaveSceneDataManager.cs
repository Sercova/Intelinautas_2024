using System.Collections.Generic;
using UnityEngine;

public static class SaveSceneDataManager
{
    public static void SaveJsonData(IEnumerable<ISaveableSceneState> arg_SceneObjs)
    {
        SaveSceneState sceneState = new SaveSceneState();
        foreach (var sceneObj in arg_SceneObjs)
        {
            sceneObj.PopulateSceneState(sceneState);
        }

        if (FileManager.WriteToFile("CurrentScene.dat", sceneState.ToJson()))
        {
            Debug.Log("Save successful");
        }
    }

    public static void LoadJsonData(IEnumerable<ISaveableSceneState> arg_SceneObjs)
    {
        if (FileManager.LoadFromFile("CurrentScene.dat", out var json))
        {
            SaveSceneState sceneState = new SaveSceneState();
            sceneState.LoadFromJson(json);

            foreach (var sceneObj in arg_SceneObjs)
            {
                sceneObj.LoadFromSceneState(sceneState);
            }

            Debug.Log("Load complete");
        }
    }
}
