using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum SwitchType
{
    Door,
    Platform,
    Chest
}
public class Switch : InteractableTrigger
{
    public SwitchType switchType;
    
    [SerializeField] public Door door;

    [HideInInspector] public bool active = true;

    private Animator animator;

    private static readonly int ActiveAnimatorMapping = Animator.StringToHash("active");

    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
        
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    protected override void Interact()
    {
        if (active)
        {
            switch (switchType)
            {
                case SwitchType.Door:
                    door.Open();
                    break;
            }
            
            active = false;
        }
        else
        {
            switch (switchType)
            {
                case SwitchType.Door:
                    door.Close();
                    break;
                default:
                    return;
            }
            
            active = true;
        }

        animator.SetBool(ActiveAnimatorMapping, active);
    }
}