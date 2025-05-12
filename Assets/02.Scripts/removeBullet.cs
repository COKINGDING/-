using UnityEngine;


public class removeBullet : MonoBehaviour
{

    public GameObject sparkEffect;



    private void OnCollisionEnter(Collision coll)
    {
        if (coll.collider.CompareTag("BULLET"))
        {
            ContactPoint cp = coll.GetContact(0); //충돌지점 정보 추출
            Quaternion rot = Quaternion.LookRotation(-cp.normal); //충돌 총알 법선방향 회전 생성
            GameObject spark = Instantiate(sparkEffect, cp.point, rot); //스파크 파티클 동적생성



            Destroy(spark, 0.3f);
            Destroy(coll.gameObject); //삭제
        }
        else if (this.CompareTag("MONSTER") && coll.collider.CompareTag("BULLET"))
        {
            Destroy(coll.gameObject);
        }
    }
}
