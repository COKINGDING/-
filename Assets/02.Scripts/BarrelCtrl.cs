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
                ContactPoint pos = coll.GetContact(0); //�浹���� ���� ����
                Quaternion rot = Quaternion.LookRotation(-pos.normal); //�浹 �Ѿ� �������� ȸ�� ����

                GameObject spark = Instantiate(BigExplosionEffect,pos.point, rot); //����ũ ��ƼŬ ��������
                
                Destroy(spark, 4.0f);
                Destroy(gameObject, 3.0f);
                IndirectDamage(transform.position);

            }




        }
    }



    void IndirectDamage(Vector3 pos)
    {

        // �ֺ��� �ִ� �巳��� ����
        Debug.Log("BOOM");
        Collider[] colls = Physics.OverlapSphere(pos, radius, 1 << 3);

        foreach(var coll in colls)
        {
            Debug.Log("BOOOOOM");
            //���� ������ ���Ե� �巳���� Rigidbody ������Ʈ ����
            rb = coll.GetComponent<Rigidbody>();
            //�巳�� ���� �氨
            rb.mass = 1.0f;

            //freezeRotation ���� ����
            rb.constraints = RigidbodyConstraints.None;

            //���߷� ����
            rb.AddExplosionForce(1500.0f, pos, radius, 1200.0f);
            

        }


    }
}
