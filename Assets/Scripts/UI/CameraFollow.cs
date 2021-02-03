using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{

    public float smoothing = 0.1f;

    public float offset = 0.5f;

    public Vector2 maxPosition;

    public Vector2 minPosition;
    
    private Transform target;
    private Animator animator;
    private GameMaster gameMaster;

    private static readonly int Kick = Animator.StringToHash("kick");

    // Start is called before the first frame update
    void Start()
    {
        gameMaster = GameObject.FindGameObjectWithTag("GM").GetComponent<GameMaster>();
        animator = GetComponent<Animator>();
        target = gameMaster.GetPlayer().gameObject.transform;
        transform.position = new Vector3(target.position.x, target.position.y, transform.position.z);
    }

    // Update is called once per frame
    void LateUpdate()
    {
        if (transform.position != target.position)
        {
            var position = transform.position;
            var targetPosition = new Vector3(target.position.x, target.position.y + offset, position.z);

            targetPosition.x = Mathf.Clamp(targetPosition.x, minPosition.x, maxPosition.x);
            targetPosition.y = Mathf.Clamp(targetPosition.y, minPosition.y, maxPosition.y);
            
            position = Vector3.Lerp(position, targetPosition, smoothing);
            transform.position = position;
        }
    }

    public void ScreenKick()
    {
        animator.SetTrigger(Kick);
    }
}