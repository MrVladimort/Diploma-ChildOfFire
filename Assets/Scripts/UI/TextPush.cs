using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TextPush : MonoBehaviour
{
    public string layerPushTo = "UI"; 
    // Start is called before the first frame update
    private void Start()
    {
        GetComponent<MeshRenderer> ().sortingLayerName = layerPushTo;
    }
}
