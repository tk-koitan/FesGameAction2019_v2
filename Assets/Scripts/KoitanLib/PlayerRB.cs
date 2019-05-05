using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerRB : MonoBehaviour
{
    private Rigidbody2D rb;
    [SerializeField]
    private BoxCollider2D box;
    public float maxVx = 5;
    public float accelVx = 1;
    public float gravity = -0.2f;
    public float maxVy = 10;
    public bool isGround;
    public bool isSquat;
    public int direction = 1;//右1,左-1
    private float defaultScaleX;

    private Vector2 v = Vector2.zero;
    private Vector2 rv = Vector2.zero;
    public Vector2 power = Vector2.zero;
    public Vector2 externalPower = Vector2.zero;
    public Vector2 groundPoint;
    private Vector2 groundNormal;
    private GameObject groundObj;
    public float slopeMaxDeg = 50;

    private bool isContactRight;
    private bool isContactLeft;
    private bool isContactUp;
    private Vector2 riderBeforePos;

    private ActionInput actionInput;

    public bool jumpEnabled = false;

    //ジャンプ猶予
    private float jumpCooltime = 0;
    private float jumpDefaultCooltime = 0.07f;

    //ジャンプ回数
    public int defaultAirJumpTimes = 1;
    private int airJumpTimes = 0;
    public float defaultJumpFrames = 0.15f;
    private float jumpFrames = 0;
    private bool isJumping;

    [SerializeField]
    private float sceneWaitTime = 5.0f;


    // キャッシュ by tada
    Animator animator;
    GroundChecker groundChecker;

    //音
    public AudioClip jumpSE;
    public AudioClip deathSE;
    private AudioSource audioSource;

    //IK
    public GameObject ik;
    public Transform handPos;

    // Start is called before the first frame update
    void Start()
    {
        direction = (transform.localScale.x > 0.0f) ? 1 : -1; // tada
        defaultScaleX = transform.localScale.x * direction;
        transform.localScale = new Vector3(
            defaultScaleX, transform.localScale.y, transform.localScale.z);

        rb = GetComponent<Rigidbody2D>();
        box = GetComponent<BoxCollider2D>();
        groundChecker = GetComponentInChildren<GroundChecker>();
        animator = GetComponent<Animator>(); // tada
        actionInput = ActionInput.Instatnce;
        audioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        AnimatorStateInfo stateInfo = animator.GetCurrentAnimatorStateInfo(0);

        if (ActionInput.GetButton(ButtonCode.RightArrow))
        {
            power.x += accelVx * Time.deltaTime * 60;
            power.x = Mathf.Min(power.x, maxVx);
            direction = 1;
        }
        else if (ActionInput.GetButton(ButtonCode.LeftArrow))
        {
            power.x -= accelVx * Time.deltaTime * 60;
            power.x = Mathf.Max(power.x, -maxVx);
            direction = -1;
        }
        else
        {
            if (isGround)
            {
                if (power.x > 0)
                {
                    power.x -= accelVx * 2 * Time.deltaTime * 60;
                    power.x = Mathf.Max(0, power.x);
                }
                else if (power.x < 0)
                {
                    power.x += accelVx * 2 * Time.deltaTime * 60;
                    power.x = Mathf.Min(0, power.x);
                }
            }
            else
            {
                //スピードが早すぎるときは減速しない
                if (Mathf.Abs(power.x) < maxVx * 2)
                {
                    if (power.x > 0)
                    {
                        power.x -= accelVx * Time.deltaTime * 60;
                        power.x = Mathf.Max(0, power.x);
                    }
                    else if (power.x < 0)
                    {
                        power.x += accelVx * Time.deltaTime * 60;
                        power.x = Mathf.Min(0, power.x);
                    }
                }
            }
        }

        //スピードが早いときは減速しない
        if (Mathf.Abs(power.x) < maxVx)
        {
            if (power.x > maxVx)
            {
                power.x -= accelVx * Time.deltaTime * 60;
                power.x = Mathf.Max(power.x, maxVx);
            }
            else if (power.x < -maxVx)
            {
                power.x += accelVx * Time.deltaTime * 60;
                power.x = Mathf.Min(power.x, -maxVx);
            }
        }



        transform.localScale = new Vector3(
            defaultScaleX * direction, transform.localScale.y, transform.localScale.x);

        isSquat = stateInfo.IsName("squat");

        if (ActionInput.GetButton(ButtonCode.DownArrow))
        {
            animator.SetBool("squat", true);
        }
        else
        {
            animator.SetBool("squat", false);
        }

        if (isContactRight)
        {
            power.x = Mathf.Min(power.x, 0);
        }

        if (isContactLeft)
        {
            power.x = Mathf.Max(power.x, 0);
        }

        if (ActionInput.GetButtonDown(ButtonCode.Jump) && jumpEnabled)
        {
            if (!isGround)
            {
                airJumpTimes--;
                if (airJumpTimes <= 0)
                {
                    jumpEnabled = false;
                }
            }
            isJumping = true;
            jumpFrames = defaultJumpFrames;
            power.y = maxVy;
            isGround = false;
            animator.SetBool("Jump", true); // tada
            //音
            audioSource.PlayOneShot(jumpSE);
        }

        if (isGround)
        {
            airJumpTimes = defaultAirJumpTimes;
            jumpEnabled = true;
            jumpCooltime = jumpDefaultCooltime;
            isJumping = false;
            jumpFrames = defaultJumpFrames;
            power.y = 0;
            animator.SetBool("Jump", false); // tada
            if (power.magnitude < 0.01f)
            {
                v = Vector2.zero;
            }
            v = Vector3.ProjectOnPlane(power, groundNormal);
            v.x = Mathf.Clamp(v.x, -maxVx * 2, maxVx * 2);
            Debug.DrawRay(groundPoint, groundNormal, Color.red);
            //Debug.DrawRay(groundPoint, v, Color.red);
        }
        else
        {
            //ジャンプ
            if (jumpCooltime > 0)
            {
                jumpCooltime -= Time.deltaTime;
            }
            else
            {
                if (airJumpTimes == 0)
                {
                    jumpEnabled = false;
                }
            }

            if (isJumping && jumpFrames > 0)
            {
                if (ActionInput.GetButtonDown(ButtonCode.Jump))
                {
                    animator.Play("PlayerJump", 0, 0.0f); // tada
                    //音
                    audioSource.PlayOneShot(jumpSE);
                }
                if (ActionInput.GetButton(ButtonCode.Jump))
                {
                    jumpFrames -= Time.deltaTime;
                    power.y = maxVy;
                    isGround = false;
                }
                else
                {
                    isJumping = false;
                }
            }
            else
            {
                power.y += gravity * Time.deltaTime * 60;
            }
            power.y = Mathf.Clamp(power.y, -maxVy, maxVy * 2);
            v = power;
            v.x = Mathf.Clamp(v.x, -maxVx * 2, maxVx * 2);
        }

        //rb.velocity = v + rv;
        rb.MovePosition(transform.position + (Vector3)v * Time.deltaTime * 60);
        //rv = Vector2.zero;
        //rv = Vector2.zero;
        //transform.position += (Vector3)v;
        Debug.DrawRay(transform.position, v * Time.deltaTime * 60, Color.white);
        //Debug.Log("速さ:"+v.magnitude);

        // tada
        animator.SetFloat("MoveSpeed", Mathf.Abs(v.x));
    }


    public void PositionSet(Vector2 velocity)
    {
        //rb.MovePosition(transform.position + (Vector3)velocity);
        transform.position += (Vector3)velocity;
        //v += velocity;
        //rv = velocity;
        //rb.velocity += velocity;
        //Debug.Log(velocity);
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (v.y > 0 && !isGround) return;
        isContactRight = false;
        isContactLeft = false;

        groundPoint = collision.contacts[0].point;
        groundNormal = collision.contacts[0].normal;
        groundObj = collision.contacts[0].collider.gameObject;
        Vector2 center = transform.position;
        float dir = (v + center - collision.contacts[0].point).magnitude;

        float slopeMinY = Mathf.Sin((90 - slopeMaxDeg) * Mathf.Deg2Rad);
        for (int i = 1; i < collision.contacts.Length; i++)
        {
            float tmpDir = (v + center - collision.contacts[i].point).magnitude;
            if (tmpDir < dir)
            {
                groundPoint = collision.contacts[i].point;
                groundNormal = collision.contacts[i].normal;
                groundObj = collision.contacts[i].collider.gameObject;
            }
            Debug.DrawRay(collision.contacts[i].point, collision.contacts[i].normal);
        }

        if (isGround)
        {

            RaycastHit2D hit = Physics2D.Raycast(center, Vector2.down * (v.magnitude + box.bounds.size.y));
            Debug.DrawRay(center, Vector2.down * (v.magnitude + box.bounds.size.y));
            if (hit)
            {
                float tmpDir = (v + center - hit.point).magnitude;
                if (tmpDir < dir)
                {
                    groundPoint = hit.point;
                    groundNormal = hit.normal;
                    groundObj = hit.collider.gameObject;
                }
            }
        }


        if (groundNormal.y >= slopeMinY)
        {
            isGround = true;
            if (groundObj.layer == (int)LayerName.MovingPlatform)
            {
                Mover tmpM = groundObj.GetComponent<Mover>();
                //tmpM.ridingPlayers.Add(this);
                Vector2 fromPos = groundPoint - (Vector2)tmpM.transform.position;
                Vector2 toPos = Quaternion.Euler(0, 0, tmpM.angleSpeed) * fromPos;
                Vector2 tmpV = toPos - fromPos;
                //Debug.Log(gameObject.name + ":" + tmpV.magnitude);
                rv = tmpM.v + tmpV;
                //Debug.Log(groundObj);
            }
            else
            {
                rv = Vector2.zero;
            }
            Debug.DrawRay(groundPoint, groundNormal);
            //Debug.Log(groundNormal + ">=" + slopeMinY);
        }
        else
        {
            isGround = false;
            if (groundNormal.x < 0)
            {
                isContactRight = true;
            }
            else if (groundNormal.x > 0)
            {
                isContactLeft = true;
            }
            groundNormal = Vector2.up;
        }
    }


    private void OnCollisionStay2D(Collision2D collision)
    {
        if (v.y > 0 && !isGround) return;
        isContactRight = false;
        isContactLeft = false;

        groundPoint = collision.contacts[0].point;
        groundNormal = collision.contacts[0].normal;
        groundObj = collision.contacts[0].collider.gameObject;
        Vector2 center = transform.position;
        float dir = (v + center - collision.contacts[0].point).magnitude;

        float slopeMinY = Mathf.Sin((90 - slopeMaxDeg) * Mathf.Deg2Rad);
        for (int i = 1; i < collision.contacts.Length; i++)
        {
            float tmpDir = (v + center - collision.contacts[i].point).magnitude;
            if (tmpDir < dir)
            {
                groundPoint = collision.contacts[i].point;
                groundNormal = collision.contacts[i].normal;
                groundObj = collision.contacts[i].collider.gameObject;
            }
            Debug.DrawRay(collision.contacts[i].point, collision.contacts[i].normal);
        }

        if (isGround)
        {

            RaycastHit2D hit = Physics2D.Raycast(center, Vector2.down * (v.magnitude + box.bounds.size.y));
            Debug.DrawRay(center, Vector2.down * (v.magnitude + box.bounds.size.y));
            if (hit)
            {
                float tmpDir = (v + center - hit.point).magnitude;
                if (tmpDir < dir)
                {
                    groundPoint = hit.point;
                    groundNormal = hit.normal;
                    groundObj = hit.collider.gameObject;
                }
            }
        }


        if (groundNormal.y >= slopeMinY)
        {
            isGround = true;
            if (groundObj.layer == (int)LayerName.MovingPlatform)
            {
                Mover tmpM = groundObj.GetComponent<Mover>();
                //tmpM.ridingPlayers.Add(this);
                Vector2 fromPos = groundPoint - (Vector2)tmpM.transform.position;
                Vector2 toPos = Quaternion.Euler(0, 0, tmpM.angleSpeed) * fromPos;
                Vector2 tmpV = toPos - fromPos;
                //Debug.Log(gameObject.name + ":" + tmpV.magnitude);
                rv = tmpM.v + tmpV;
                //Debug.Log(groundObj);
            }
            else
            {
                rv = Vector2.zero;
            }
            Debug.DrawRay(groundPoint, groundNormal);
            //Debug.Log(groundNormal + ">=" + slopeMinY);
        }
        else
        {
            isGround = false;
            if (groundNormal.x < 0)
            {
                isContactRight = true;
            }
            else if (groundNormal.x > 0)
            {
                isContactLeft = true;
            }
            groundNormal = Vector2.up;
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        isGround = isContactLeft = isContactRight = false;
        groundNormal = Vector2.up;
        rv = Vector2.zero;
    }

    public void StartDeadAnimtion()
    {
        animator.SetTrigger("Death");
        Invoke("Retry", 2.0f);
        ActionInput.actionEnabled = false;
    }

    public void Retry()
    {
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        SceneManager.LoadScene(currentSceneIndex);
    }

    // 以下 tada 
    public void Goaled() // ゴールした時に実行
    {
        animator.SetTrigger("Clear");
        // 動けなくする処理
        ActionInput.actionEnabled = false; // koitan
        ScoreManager.Instatnce.TimerStop(true);
        ScoreManager.Instatnce.canvasAnimator.Play("GoalText");

        //BGM変更
        MusicManager.Play(MusicManager.Instance.bgm3);

        // n秒後にシーン遷移
        StartCoroutine(GoStageState());
    }

    private IEnumerator GoStageState()
    {
        yield return new WaitForSeconds(sceneWaitTime);
        ActionInput.actionEnabled = true;
        SceneManager.LoadScene("KawazStageSelect");
        MusicManager.Play(MusicManager.Instance.bgm2);
    }
}
