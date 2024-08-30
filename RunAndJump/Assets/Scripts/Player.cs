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

    bool isJump; //üũ �������� ����ȭ �� �ϰ��� ������ߵ�
    bool isJumping ; //���� ���� ���̶�� ��
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

                Debug.Log("�߻� ����" + dir);
                Debug.Log("�߻� �Ŀ�" + power);

                rigid.AddForce(dir * power, ForceMode2D.Impulse);
                JumpArrow.JumpArrowOff();
                JumpPower.JumpPowerOff();

                //���ڸ� ������ or �ؿ� �������� �߻� �ÿ�
                //���� ������ ������ �ʴ� ������
                //�ݸ����� ���ٰ� �ٷ� �Ѵ��������� ����
                collider.enabled = false;
                Invoke("ColliderON", 0.05f);
            }
        }
    }

    private void JumpingInput()
    {
        //���߿��� �ణ�� ���� ���� 
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
    public float targetSize = 15f;  // ��ǥ�� �ϴ� Orthographic Size
    public float zoomSpeed = 5f;   // �� ��/�ƿ� �ӵ�
    bool isZooming = false;

    public float Range = 1.0f;
    Vector3 viewportPos;

    void ObjINCamera()
    {  
        // ������Ʈ�� ���� ��ǥ�� ����Ʈ ��ǥ�� ��ȯ
        viewportPos = Camera.main.WorldToViewportPoint(transform.position);

        // ����Ʈ ��ǥ�� ī�޶��� �þ߿� �ִ��� Ȯ��
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

    // �ڷ�ƾ�� ����Ͽ� ī�޶��� ���� �ε巴�� ����
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
            //������ ���������� üũ�ϴ� ���ǹ�
            if(transform.position.y <= -8.5)
            {
                isdead = true;
                PlayerStop();
                GameManager.Instance.PlayerDie();
            }

            isMove = false;
            anim.SetBool("isRun", false);
            GroundedInPut(); //���� ����
            JumpStartInput(); //���� ���� ����
            JumpingInput(); //���� ����
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
            // ĳ���Ͱ� ���� + ������ �̶�� ���¸� �����ؾ߸� ����
            if (isJump && isJumping)
            {
                PlayerStop();
                anim.SetBool("isJump", false);
            }
        }

        if (collision.gameObject.CompareTag("Spike"))
        {
            Debug.Log("Player ���� ����");
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
            //���� ���϶�
            Block obj = collision.gameObject.GetComponent<Block>();
            if(obj != null)
            {
                transform.position += Vector3.left * obj.Speed * Time.deltaTime;
                return;
            }

            //�� �����϶�
            BlockGroup objGroup = collision.gameObject.GetComponentInParent<BlockGroup>();
            if (objGroup != null)
            {
                transform.position += Vector3.left * objGroup.Speed * Time.deltaTime;
                return;
            }


    
        }
    }

}
