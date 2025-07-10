using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public float speed;
    float hAxis;
    float vAxis;

    bool wDown; //ShiftŰ ������ �ȱ�
    bool jDown; //SpaceŰ ������ ����
    bool dDown;
    bool isJump; //���� ������ �ѹ� ���� �Ǿ����� Ȯ�� (�������� ����)
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

        transform.position += moveVec * speed * (wDown ? 0.3f : 1f) * Time.deltaTime; //���׿����� (if�� ��� ���)


        anim.SetBool("isRun", moveVec != Vector3.zero);
        anim.SetBool("isWalk", wDown);
    }

    void Turn()
    {
        //ĳ���� �̵� ������ ���� ���� ȸ��
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
        if(collision.gameObject.tag == "Floor")   //���� ���� ������Ʈ �±װ� "Floor"���
        {
            anim.SetBool("isJump", false);
            isJump = false;                       //isJump�� false�� �ȴ� (�����Լ����� isJump�� ��� true���¸� ������ �ȵǱ� ������ �ٴڿ� ������ �ٽ� false�� �ٲ���)
        }
    }
}
