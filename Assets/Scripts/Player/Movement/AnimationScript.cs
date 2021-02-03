using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationScript : MonoBehaviour
{
    public GameObject contextClue;
    public GameObject fx;
    
    [HideInInspector] public Movement move;
    [HideInInspector] public Player player;
    [HideInInspector] public SpriteRenderer sr;

    private Animator anim;
    private Collision coll;

    private Animator fxAnimator;
    private Animator contextClueAnimator;
    private SpriteRenderer contextCluesSprite;

    [Space] [Header("TriggersValue")] private static readonly int Slide = Animator.StringToHash("sliding");
    private static readonly int Combo = Animator.StringToHash("combo");
    private static readonly int IsDashing = Animator.StringToHash("isDashing");
    private static readonly int CanMove = Animator.StringToHash("canMove");
    private static readonly int WallSlide = Animator.StringToHash("wallSlide");
    private static readonly int WallGrab = Animator.StringToHash("wallGrab");
    private static readonly int OnWall = Animator.StringToHash("onWall");
    private static readonly int OnGround = Animator.StringToHash("onGround");
    private static readonly int HorizontalAxis = Animator.StringToHash("HorizontalAxis");
    private static readonly int VerticalAxis = Animator.StringToHash("VerticalAxis");
    private static readonly int HorizontalVelocity = Animator.StringToHash("HorizontalVelocity");
    private static readonly int VerticalVelocity = Animator.StringToHash("VerticalVelocity");

    private static readonly int CastAnimatorMapping = Animator.StringToHash("cast");
    private static readonly int EndCastAnimatorMapping = Animator.StringToHash("endCast");
    private static readonly int HealAnimatorMapping = Animator.StringToHash("heal");
    private static readonly int EndHealAnimatorMapping = Animator.StringToHash("endHeal");

    public Vector3 direction = new Vector3(1, 0, 0);
    private bool isFacingRight;

    void Start()
    {
        anim = GetComponent<Animator>();
        sr = GetComponent<SpriteRenderer>();

        fxAnimator = fx.GetComponent<Animator>();

        contextClueAnimator = contextClue.GetComponent<Animator>();
        contextCluesSprite = contextClue.GetComponent<SpriteRenderer>();
        
        coll = GetComponentInParent<Collision>();
        move = GetComponentInParent<Movement>();
        player = GetComponentInParent<Player>();
    }

    void Update()
    {
        anim.SetBool(OnGround, coll.onGround);
        anim.SetBool(OnWall, coll.onWall);
        anim.SetBool(WallGrab, move.wallGrab);
        anim.SetBool(WallSlide, move.wallSlide);
        anim.SetBool(CanMove, move.canMove);
        anim.SetBool(IsDashing, move.isDashing);
        anim.SetBool(Slide, move.slide);
        anim.SetBool(Combo, player.combo);

        fxAnimator.SetBool(OnGround, coll.onGround);
    }

    public void SetHorizontalMovement(float x, float y, float xVel, float yVel)
    {
        anim.SetFloat(HorizontalAxis, x);
        anim.SetFloat(VerticalAxis, y);
        anim.SetFloat(HorizontalVelocity, Mathf.Abs(xVel));
        anim.SetFloat(VerticalVelocity, yVel);
    }

    public void SetTrigger(int trigger)
    {
        anim.SetTrigger(trigger);

        if (trigger == CastAnimatorMapping || trigger == EndCastAnimatorMapping || trigger == HealAnimatorMapping ||
            trigger == EndHealAnimatorMapping)
        {
            fxAnimator.SetTrigger(trigger);
        }
    }
    
    public void EnableContextClue()
    {
        contextClue.SetActive(true);
    }

    public void DisableContextClue()
    {
        contextClue.gameObject.SetActive(false);
    }

    public void LockMove()
    {
        player.LockMove();
    }

    public void UnlockMove()
    {
        player.UnlockMove();
    }

    public void UnlockCombo()
    {
        player.UnlockCombo();
    }
    
    public void Flip(int flipSide)
    {
        if (flipSide == -1 && !isFacingRight)
        {
            FlipAnimationObject();
        }
        else if (flipSide == 1 && isFacingRight)
        {
            FlipAnimationObject();
        }
    }

    public void FlipAnimationObject()
    {
        direction *= -1;
        contextCluesSprite.flipX = !contextCluesSprite.flipX;
        isFacingRight = !isFacingRight;
        Vector3 mirrorScale = gameObject.transform.localScale;
        mirrorScale.x *= -1;
        gameObject.transform.localScale = mirrorScale;
    }
}