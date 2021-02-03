using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class StringValue : ScriptableObject, ISerializationCallbackReceiver
{
    public string initValue;

    [HideInInspector] public string RuntimeValue;

    public void OnBeforeSerialize()
    {
        RuntimeValue = initValue;
    }

    public void OnAfterDeserialize()
    {
    }
}