using System;

[Serializable]
public class InteractuableReference
{
    public bool UseConstant = true;
    public string ConstantValue;
    public InteractuableVariable Variable;

    public string Value
    {
        get { return UseConstant ? ConstantValue : Variable.Value; }
        set { Variable.Value = value; }
    }
}
