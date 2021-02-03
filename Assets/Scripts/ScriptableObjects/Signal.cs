using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class Signal : ScriptableObject
{
    public List<SignalListener> listeners = new List<SignalListener>();

    public void Raise()
    {
        listeners.ForEach(listener => listener.OnSignalRaised());
    }
    
    public void RegisterListener(SignalListener listener)
    {
        listeners.Add(listener);
    }
    
    public void DegisterListener(SignalListener listener)
    {
        listeners.Remove(listener);
    }
}
