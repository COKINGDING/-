using UnityEngine;

public class BarrelCtrl : MonoBehaviour
{

    private float radius;
    private Rigidbody rb;

    public Texture[] textures;
    private new MeshRenderer renderer;

    private int cnt;
    private int idx;


    void Start()
    {
        idx = Random.Range(0, textures.Length);
        radius = 5.0f;
        cnt = 3;
        renderer = GetComponentInChildren<MeshRenderer>();

        if (textures.Length > 0) 
        {
            renderer.material.mainTexture = textures[idx];
        }

    }

    // Update is called once per frame
    void Update()
    {
        


    }

    public GameObject BigExplosionEffect; 
 
    private void OnCollisionEnter(Collision coll)
    {
        if (coll.collider.CompareTag("BULLET"))
        {
            cnt--;

            if (cnt == 0)
            {
                ContactPoint pos = coll.GetContact(0); //충돌지점 정보 추출
                Quaternion rot = Quaternion.LookRotation(-pos.normal); //충돌 총알 법선방향 회전 생성

                GameObject spark = Instantiate(BigExplosionEffect,pos.point, rot); //스파크 파티클 동적생성
                
                Destroy(spark, 4.0f);
                Destroy(gameObject, 3.0f);
                IndirectDamage(transform.position);

            }




        }
    }



    void IndirectDamage(Vector3 pos)
    {

        // 주변에 있는 드럼모두 추출
        Debug.Log("BOOM");
        Collider[] colls = Physics.OverlapSphere(pos, radius, 1 << 3);

        foreach(var coll in colls)
        {
            Debug.Log("BOOOOOM");
            //폭발 범위에 포함된 드럼통의 Rigidbody 컴포넌트 추출
            rb = coll.GetComponent<Rigidbody>();
            //드럼통 무게 경감
            rb.mass = 1.0f;

            //freezeRotation 제한 해제
            rb.constraints = RigidbodyConstraints.None;

            //폭발력 전달
            rb.AddExplosionForce(1500.0f, pos, radius, 1200.0f);
            

        }


    }
}
