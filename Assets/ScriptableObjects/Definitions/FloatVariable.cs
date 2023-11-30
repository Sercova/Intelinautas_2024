using UnityEngine;

[CreateAssetMenu]
public class FloatVariable : ScriptableObject
{
    public float Value;

    private void OnEnable()
    {
        Value = 0.0f;
    }
}