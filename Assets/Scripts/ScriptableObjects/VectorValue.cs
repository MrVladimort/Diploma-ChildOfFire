using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class VectorValue : ScriptableObject, ISerializationCallbackReceiver
{
    public Vector3 initValue;

    [HideInInspector] public Vector3 RuntimeValue;

    public void OnBeforeSerialize()
    {
        RuntimeValue = initValue;
    }

    public void OnAfterDeserialize() {}
}
