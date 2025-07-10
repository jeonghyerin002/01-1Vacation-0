using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public float speed;
    float hAxis;
    float vAxis;

    bool wDown; //Shift키 누르면 걷기
    bool jDown; //Space키 누르면 점프
    bool dDown;
    bool isJump; //현재 점프가 한번 실행 되었는지 확인 (무한점프 막기)
    bool isDodge;

    Vector3 moveVec;
    Vector3 dodgeVec;


    Rigidbody rb;
    Animator anim;
    
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        anim = GetComponentInChildren<Animator>();
    }

    
    void Update()
    {
        GetInput();
        Move();
        Turn();
        Jump();
        Dodge();


    }

    void GetInput()
    {
        hAxis = Input.GetAxisRaw("Horizontal");
        vAxis = Input.GetAxisRaw("Vertical");
        wDown = Input.GetButton("Walk");
        jDown = Input.GetButtonDown("Jump");
        dDown = Input.GetButtonDown("Dodge");
    }

    void Move()
    {
        moveVec = new Vector3(hAxis, 0, vAxis).normalized;

        if(isDodge)
        {
            moveVec = dodgeVec;
        }

        //if(wDown)
        //{
        //transform.position += moveVec * speed * 0.3f * Time.deltaTime;
        //}
        //else
        //{
        // transform.position += moveVec * speed * Time.deltaTime;
        //}

        transform.position += moveVec * speed * (wDown ? 0.3f : 1f) * Time.deltaTime; //삼항연산자 (if문 대신 사용)


        anim.SetBool("isRun", moveVec != Vector3.zero);
        anim.SetBool("isWalk", wDown);
    }

    void Turn()
    {
        //캐릭터 이동 방향을 따라 몸을 회전
        transform.LookAt(transform.position + moveVec);
    }

    void Jump()
    {
        if(jDown && !isJump && !isDodge)
        {
            rb.AddForce(Vector3.up * 20, ForceMode.Impulse);
            anim.SetBool("isJump", true);
            anim.SetTrigger("doJump");
            isJump = true;
        }
    }

    void Dodge()
    {
        if (dDown && moveVec != Vector3.zero && !isDodge)
        {
            dodgeVec = moveVec;
            speed *= 2;
            anim.SetTrigger("doDodge");
            isDodge = true;

            Invoke("DodgeOut", 0.4f);
        }
    }

    void DodgeOut()
    {
        speed *= 0.5f;
        isDodge = false;
    }

    void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.tag == "Floor")   //만약 게임 오브젝트 태그가 "Floor"라면
        {
            anim.SetBool("isJump", false);
            isJump = false;                       //isJump가 false가 된다 (점프함수에서 isJump가 계속 true상태면 점프가 안되기 때문에 바닥에 닿으면 다시 false로 바꿔줌)
        }
    }
}
