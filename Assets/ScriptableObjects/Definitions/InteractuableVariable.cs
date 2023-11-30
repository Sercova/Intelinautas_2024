using UnityEngine;

[CreateAssetMenu]
public class InteractuableVariable : ScriptableObject
{
    public string Value;

    private void OnEnable()
    {
        Value = "-";
    }
}

