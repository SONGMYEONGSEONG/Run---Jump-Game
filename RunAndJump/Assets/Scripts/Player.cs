using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SocialPlatforms;

public class Player : MonoBehaviour
{
    [SerializeField] Rigidbody2D rigid;
    [SerializeField] SpriteRenderer renderer;
    [SerializeField] Animator anim;

    [SerializeField] float speed = 1.0f;
    [SerializeField] float jumpSpeed = 1.5f;
    [SerializeField] UI_JumpArrow JumpArrow;
    [SerializeField] UI_JumpPower JumpPower;

    [SerializeField] Collider2D collider;

    bool isJump; //체크 목적으로 직렬화 함 하고나서 해지헤야됨
    bool isJumping ; //현재 점프 중이라는 뜻
    bool isdead;
    bool isMove;

    
    private void Start()
    {
        PlayerInitalize();
    }

    private void OnDestroy()
    {
        UI_TouchLeftPad.OnTouchLeftPad -= LeftMove;
        UI_TouchRightPad.OnTouchRightPad -= RightMove;
    }

    private void PlayerInitalize()
    {
        UI_TouchLeftPad.OnTouchLeftPad += LeftMove;
        UI_TouchRightPad.OnTouchRightPad += RightMove;

        isMove = false;
        isJump = false;
        isJumping = false;
        isdead = false;
    }

    public void LeftMove()
    {
        
        isMove = true;
        transform.position += Vector3.left * speed * Time.deltaTime;
        renderer.flipX = false;
        anim.SetBool("isRun", true);
    }

    public void RightMove()
    {
       
        isMove = true;
        transform.position += Vector3.right * speed * Time.deltaTime;
        renderer.flipX = true;
        anim.SetBool("isRun", true);
    }

    private void GroundedInPut()
    {
        if (!isJump && !isJumping)
        {
            if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow))
            {
                RightMove();
            }

            if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow))
            {
                LeftMove();
            }
        
            if (Input.GetMouseButton(0) || Input.GetKeyDown(KeyCode.Space))
            {
                if(Input.mousePosition.x >= 170 && Input.mousePosition.x <= 830
                    &&
                    Input.mousePosition.y >=100 && Input.mousePosition.y <=425)
                {
                    return;
                }

                isMove = false;
                isJump = true;
                JumpArrow.JumpArrowPrint(transform.position);
                JumpPower.JumpPowerPrint(transform.position);
                anim.SetBool("isRun", false);
            }

        }
    }

    private void JumpStartInput()
    {
        if (isJump && !isJumping)
        {
            if (Input.GetMouseButtonUp(0) || Input.GetKeyUp(KeyCode.Space))
            {
                isJumping = true;

                anim.SetBool("isJump", true);
                Vector2 PlayerPos = transform.position;
                Vector2 dir = JumpArrow.GetJumpingDir() - PlayerPos;
                dir.Normalize();

                float power = JumpPower.GetJumpingPower();

                Debug.Log("발사 방향" + dir);
                Debug.Log("발사 파워" + power);

                rigid.AddForce(dir * power, ForceMode2D.Impulse);
                JumpArrow.JumpArrowOff();
                JumpPower.JumpPowerOff();

                //제자리 구르기 or 밑에 방향으로 발사 시에
                //블럭이 있으면 멈추지 않는 현상을
                //콜리더를 껏다가 바로 켜는형식으로 변경
                collider.enabled = false;
                Invoke("ColliderON", 0.05f);
            }
        }
    }

    private void JumpingInput()
    {
        //공중에서 약간의 조작 가능 
        if (isJump && isJumping)
        {
            if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow))
            {
                transform.position += Vector3.right * speed * 0.5f * Time.deltaTime;
                renderer.flipX = true;
            }

            if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow))
            {
                transform.position += Vector3.left * speed * 0.5f * Time.deltaTime;
                renderer.flipX = false;
            }

        }
    }

    //Test
    public float originSize = 6.87f;
    public float targetSize = 15f;  // 목표로 하는 Orthographic Size
    public float zoomSpeed = 5f;   // 줌 인/아웃 속도
    bool isZooming = false;

    public float Range = 1.0f;
    Vector3 viewportPos;

    void ObjINCamera()
    {  
        // 오브젝트의 월드 좌표를 뷰포트 좌표로 변환
        viewportPos = Camera.main.WorldToViewportPoint(transform.position);

        // 뷰포트 좌표가 카메라의 시야에 있는지 확인
        if (!(viewportPos.x >= 0 && viewportPos.x <= 1 && viewportPos.y >= 0 && viewportPos.y <= 1 && viewportPos.z > 0))
        {
            //Camera.main.orthographicSize = Mathf.Lerp(Camera.main.orthographicSize, targetSize, Time.deltaTime * zoomSpeed);
            if (!isZooming)
            {
                StartCoroutine(ZoomCamera(targetSize));
            }
        }
        else 
        {
            //Camera.main.orthographicSize = Mathf.Lerp(Camera.main.orthographicSize, originSize, Time.deltaTime * zoomSpeed);
            if (!isZooming)
            {
                StartCoroutine(ZoomCamera(originSize));
            }
        }
    }

    // 코루틴을 사용하여 카메라의 줌을 부드럽게 변경
    IEnumerator ZoomCamera(float targetZoom)
    {
        isZooming = true;
        while (Mathf.Abs(Camera.main.orthographicSize - targetZoom) > 0.01f)
        {
            //Debug.Log(targetZoom + " /// " + Camera.main.orthographicSize + "-" + targetZoom);
            Camera.main.orthographicSize = Mathf.Lerp(Camera.main.orthographicSize, targetZoom, Time.deltaTime * zoomSpeed);
            yield return null;
        }
        Camera.main.orthographicSize = targetZoom;
        isZooming = false;
    }


    // Update is called once per frame
    void Update()
    {
        ObjINCamera();

        if (!isdead)
        {
            //땅으로 떨어졌는지 체크하는 조건문
            if(transform.position.y <= -8.5)
            {
                isdead = true;
                PlayerStop();
                GameManager.Instance.PlayerDie();
            }

            isMove = false;
            anim.SetBool("isRun", false);
            GroundedInPut(); //지상 조작
            JumpStartInput(); //점프 시작 조작
            JumpingInput(); //공중 조작
        }
    }

    private void ColliderON()
    {
        collider.enabled = true;
    }

    private void PlayerStop()
    {
        isJump = false;
        isJumping = false;
        rigid.velocity = Vector2.zero;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Block"))
        {
            // 캐릭터가 점프 + 점프중 이라는 상태를 만족해야만 멈춤
            if (isJump && isJumping)
            {
                PlayerStop();
                anim.SetBool("isJump", false);
            }
        }

        if (collision.gameObject.CompareTag("Spike"))
        {
            Debug.Log("Player 가시 닿음");
            isdead = true;
            anim.SetBool("isDead", true);
            PlayerStop();
            GameManager.Instance.PlayerDie();
        }
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Block"))
        {
            //단일 블럭일때
            Block obj = collision.gameObject.GetComponent<Block>();
            if(obj != null)
            {
                transform.position += Vector3.left * obj.Speed * Time.deltaTime;
                return;
            }

            //블럭 집합일때
            BlockGroup objGroup = collision.gameObject.GetComponentInParent<BlockGroup>();
            if (objGroup != null)
            {
                transform.position += Vector3.left * objGroup.Speed * Time.deltaTime;
                return;
            }


    
        }
    }

}
