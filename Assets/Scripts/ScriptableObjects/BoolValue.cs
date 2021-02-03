using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class BoolValue : ScriptableObject, ISerializationCallbackReceiver
{
    public bool initValue;

    [HideInInspector] public bool RuntimeValue;

    public void OnBeforeSerialize()
    {
        RuntimeValue = initValue;
    }

    public void OnAfterDeserialize() {}
}
