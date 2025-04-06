using UnityEngine;

public class PlayerCtrl : MonoBehaviour
{

    private Animation anim; // �ִϸ��̼�
    private Transform tr; //��ġ
    private Transform camTr; // ī�޶� ��ġ

    public float moveSpeed = 5.0f; // �̵� �ӵ�
    public float rotSpeed = 300.0f; // ȸ�� �ӵ�

    void Start()
    {

        tr = GetComponent<Transform>();
        anim = GetComponent<Animation>();
        anim.CrossFade("Idle", 0.25f);
        camTr = Camera.main.transform;

    }

    void Update()
    {

        float h = Input.GetAxis("Horizontal"); // A, D �Ǵ� ����/������ ȭ��ǥ Ű
        float v = Input.GetAxis("Vertical");   // W, S �Ǵ� ��/�Ʒ� ȭ��ǥ Ű
        float mouseX = Input.GetAxis("Mouse X"); // ���콺 �¿� �̵�

        Vector3 moveDir = camTr.forward * v + camTr.right * h;
        moveDir.y = 0;
        moveDir = moveDir.normalized;

        tr.position += moveDir * Time.deltaTime * moveSpeed;
        tr.Rotate(Vector3.up * rotSpeed * Time.deltaTime * mouseX);

        if (h == 0 && v == 0) //�̵� �ִϸ��̼� ����
        {
           anim.CrossFade("Idle", 0.25f);
        }
        else if(h > 0) 
        {
            anim.CrossFade("RunR", 0.25f);
        }
        else if(h < 0) 
        {
            anim.CrossFade("RunL", 0.25f);
        }
        else if (v < 0)
        {
            anim.CrossFade("RunB", 0.25f);
        }
        else 
        {
            anim.CrossFade("RunF", 0.25f);
        }


    }


    
}
