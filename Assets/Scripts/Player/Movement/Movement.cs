using System.Collections;
using UnityEngine;
using DG.Tweening;

public class Movement : MonoBehaviour
{
    [HideInInspector] public Collision coll;
    private Rigidbody2D rb;
    private AnimationScript anim;
    private Player player;
    private GameMaster gameMaster;

    [Space] [Header("Stats")] public float speed = 10;
    public float jumpForce = 50;
    public float slideSpeed = 5;
    public float wallJumpLerp = 10;
    public float dashSpeed = 20;

    [Space] [Header("Booleans")] public bool canMove;
    public bool wallGrab;
    public bool wallJumped;
    public bool wallSlide;
    public bool slide;
    public bool isDashing;

    [Space] [Header("Polish")] public ParticleSystem dashParticle;
    public ParticleSystem jumpParticle;
    public ParticleSystem wallJumpParticle;
    public ParticleSystem slideParticle;

    [Space] private bool groundTouch;
    private bool hasDashed;

    public int side = 1;

    [Space] [Header("TriggersValue")] private static readonly int DashAnimatorMapping = Animator.StringToHash("dash");
    [Space] [Header("TriggersValue")] private static readonly int JumpAnimatorMapping = Animator.StringToHash("jump");

    [Space] [Header("TriggersValue")]
    private static readonly int WallJumpAnimatorMapping = Animator.StringToHash("wallJump");

    // Start is called before the first frame update
    void Start()
    {
        gameMaster = GameObject.FindGameObjectWithTag("GM").GetComponent<GameMaster>();
        player = GetComponent<Player>();
        coll = GetComponent<Collision>();
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponentInChildren<AnimationScript>();
    }

    // Update is called once per frame
    void Update()
    {
        if (player.health.CheckDead())
        {
            float x = Input.GetAxis("Horizontal");
            float y = Input.GetAxis("Vertical");
            float xRaw = Input.GetAxisRaw("Horizontal");
            float yRaw = Input.GetAxisRaw("Vertical");
            Vector2 dir = new Vector2(x, y);

            if (dir.Equals(Vector2.zero))
            {
                player.currentState = PlayerState.Idle;
            }

            Walk(dir);
            anim.SetHorizontalMovement(Mathf.Abs(x), y, rb.velocity.x, rb.velocity.y);
            
            if (xRaw != 0 && coll.onGround)
                gameMaster.GetSoundManager().PlayRun();
            // else if (x == 0 && coll.onGround)
                // gameMaster.GetSoundManager().Stop();
            
            if (coll.onWall && Input.GetButton("Slide") && canMove)
            {
                player.currentState = PlayerState.WallGrab;

                if (side != coll.wallSide)
                    anim.Flip(side);

                wallGrab = true;
                wallSlide = false;
            }

            if (Input.GetButtonUp("Slide") || !coll.onWall || !canMove)
            {
                player.currentState = PlayerState.Jump;

                wallGrab = false;
                wallSlide = false;
            }

            if (Input.GetButtonDown("Slide") && coll.onGround && player.currentState != PlayerState.Slide)
            {
                player.currentState = PlayerState.Slide;
                slide = true;
            }

            if (Input.GetButtonUp("Slide"))
            {
                slide = false;
            }

            if (coll.onGround && !isDashing)
            {
                wallJumped = false;
                GetComponent<BetterJumping>().enabled = true;
            }

            if (wallGrab && !isDashing)
            {
                rb.gravityScale = 0;
                if (x > .2f || x < -.2f)
                    rb.velocity = new Vector2(rb.velocity.x, 0);

                rb.velocity = new Vector2(rb.velocity.x, 0);
            }
            else
            {
                rb.gravityScale = 3;
            }

            if (coll.onWall && !coll.onGround)
            {
                if (x != 0 && !wallGrab)
                {
                    player.currentState = PlayerState.WallSlide;

                    wallSlide = true;
                    WallSlide();
                }
            }

            if (!coll.onWall || coll.onGround)
                wallSlide = false;

            if (Input.GetButtonDown("Jump"))
            {
                player.currentState = PlayerState.Jump;
                gameMaster.GetSoundManager().PlayJump();
                
                if (coll.onGround)
                {
                    anim.SetTrigger(JumpAnimatorMapping);
                    Jump(Vector2.up, false);
                }
                else if (coll.onWall)
                {
                    anim.SetTrigger(WallJumpAnimatorMapping);
                    WallJump();
                }
            }

            if (Input.GetButtonDown("Dash") && !hasDashed)
            {
                player.currentState = PlayerState.Dash;

                if (xRaw != 0 || yRaw != 0)
                    Dash(xRaw, yRaw);
            }

            if (coll.onGround && !groundTouch)
            {
                gameMaster.GetSoundManager().PlayFall();
                GroundTouch();
                groundTouch = true;
            }

            if (!coll.onGround && groundTouch)
            {
                groundTouch = false;
            }

            WallParticle(y);

            if (wallGrab || wallSlide || !canMove)
                return;

            if (x > 0)
            {
                side = 1;
                anim.Flip(side);
            }

            if (x < 0)
            {
                side = -1;
                anim.Flip(side);
            }
        }
    }

    void GroundTouch()
    {
        hasDashed = false;
        isDashing = false;

        side = anim.sr.flipX ? -1 : 1;

        jumpParticle.Play();
    }

    private void Dash(float x, float y)
    {
        Camera.main.transform.DOComplete();
        Camera.main.transform.DOShakePosition(.2f, .5f, 14, 90, false, true);
        FindObjectOfType<RippleEffect>().Emit(Camera.main.WorldToViewportPoint(transform.position));

        hasDashed = true;

        anim.SetTrigger(DashAnimatorMapping);

        rb.velocity = Vector2.zero;
        Vector2 dir = new Vector2(x, y);

        rb.velocity += dir.normalized * dashSpeed;
        StartCoroutine(DashWait());
    }

    IEnumerator DashWait()
    {
        FindObjectOfType<GhostTrail>().ShowGhost();
        StartCoroutine(GroundDash());
        DOVirtual.Float(14, 0, .8f, RigidbodyDrag);

        dashParticle.Play();
        rb.gravityScale = 0;
        GetComponent<BetterJumping>().enabled = false;
        wallJumped = true;
        isDashing = true;

        yield return new WaitForSeconds(.3f);

        dashParticle.Stop();
        rb.gravityScale = 3;
        GetComponent<BetterJumping>().enabled = true;
        wallJumped = false;
        isDashing = false;
    }

    IEnumerator GroundDash()
    {
        yield return new WaitForSeconds(2.5f);
        if (coll.onGround)
            hasDashed = false;
    }

    private void WallJump()
    {
        if (side == 1 && coll.onRightWall || side == -1 && !coll.onRightWall)
        {
            side *= -1;
        }

        StartCoroutine(DisableMovement(.2f));

        Vector2 wallDir = coll.onRightWall ? Vector2.left : Vector2.right;

        Jump((Vector2.up / 1.5f + wallDir / 1.5f), true);

        wallJumped = true;
    }

    private void WallSlide()
    {
        if (coll.wallSide == side)
            anim.Flip(side * -1);

        if (!canMove)
            return;

        bool pushingWall = rb.velocity.x > 0 && coll.onRightWall || rb.velocity.x < 0 && coll.onLeftWall;

        float push = pushingWall ? 0 : rb.velocity.x;

        rb.velocity = new Vector2(push, -slideSpeed);
    }

    private void Walk(Vector2 dir)
    {
        if (!canMove)
            return;

        if (wallGrab)
            return;

        if (!wallJumped)
        {
            rb.velocity = new Vector2(dir.x * (slide ? slideSpeed : speed), rb.velocity.y);
        }
        else
        {
            rb.velocity = Vector2.Lerp(rb.velocity, new Vector2(dir.x * speed, rb.velocity.y),
                wallJumpLerp * Time.deltaTime);
        }
    }

    private void Jump(Vector2 dir, bool wall)
    {
        slideParticle.transform.parent.localScale = new Vector3(ParticleSide(), 1, 1);
        ParticleSystem particle = wall ? wallJumpParticle : jumpParticle;

        rb.velocity = new Vector2(rb.velocity.x, 0);
        rb.velocity += dir * jumpForce;

        particle.Play();
    }

    public IEnumerator DisableMovement(float time)
    {
        canMove = false;
        yield return new WaitForSeconds(time);
        canMove = true;
    }

    void RigidbodyDrag(float x)
    {
        rb.drag = x;
    }

    void WallParticle(float vertical)
    {
        var main = slideParticle.main;

        if (wallSlide)
        {
            slideParticle.transform.parent.localScale = new Vector3(ParticleSide(), 1, 1);
            main.startColor = Color.white;
            slideParticle.Play();
        }
        else
        {
            main.startColor = Color.clear;
        }
    }

    int ParticleSide()
    {
        int particleSide = coll.onRightWall ? -1 : 1;
        return particleSide;
    }
}