using UnityEngine;


public class removeBullet : MonoBehaviour
{

    public GameObject sparkEffect;



    private void OnCollisionEnter(Collision coll)
    {
        if (coll.collider.CompareTag("BULLET"))
        {
            ContactPoint cp = coll.GetContact(0); //�浹���� ���� ����
            Quaternion rot = Quaternion.LookRotation(-cp.normal); //�浹 �Ѿ� �������� ȸ�� ����
            GameObject spark = Instantiate(sparkEffect, cp.point, rot); //����ũ ��ƼŬ ��������



            Destroy(spark, 0.3f);
            Destroy(coll.gameObject); //����
        }
        else if (this.CompareTag("MONSTER") && coll.collider.CompareTag("BULLET"))
        {
            Destroy(coll.gameObject);
        }
    }
}
