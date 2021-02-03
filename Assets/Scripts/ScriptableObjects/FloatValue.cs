using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class FloatValue : ScriptableObject, ISerializationCallbackReceiver
{
    public float initValue;

    [HideInInspector] public float RuntimeValue;

    public void OnBeforeSerialize()
    {
        RuntimeValue = initValue;
    }

    public void OnAfterDeserialize() {}
}
