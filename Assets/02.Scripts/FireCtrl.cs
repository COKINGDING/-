using UnityEngine;
using System.Collections;

[RequireComponent(typeof(AudioSource))]
public class FireCtrl : MonoBehaviour
{
    public AudioClip fireSfx;
    private new AudioSource audio;
    public MeshRenderer muzzleFlash;

    public GameObject bullet;
    public Transform firePos;
    public GameObject player;

    void Start()
    {
        player = GameObject.FindWithTag("Player");
        audio = GetComponent<AudioSource>();
    }

    void Update()
    {
        if (!GameManager.instance.isGameStarted) return;

        if (Input.GetMouseButtonDown(0)) // 좌클릭
        {
            Instantiate(bullet, firePos.position, firePos.rotation);
            StartCoroutine(ShowMuzzleFlash());
            audio.PlayOneShot(fireSfx, 1.0f);
        }
    }

    IEnumerator ShowMuzzleFlash()
    {
        Vector2[] Offsets = new Vector2[]
        {
            new Vector2(0.0f, 0.0f), // 기본 위치
            new Vector2(0.5f, 0.0f), // 오른쪽
            new Vector2(0.0f, 0.5f), // 위쪽
            new Vector2(0.5f, 0.5f)  // 오른쪽 위
        };

        Vector2 randomOffset = Offsets[Random.Range(0, Offsets.Length)];

        Material mat = muzzleFlash.material;
        mat.mainTextureOffset = randomOffset;

        float randomZRotation = Random.Range(0f, 360f);
        muzzleFlash.transform.localRotation = Quaternion.Euler(0, 0, randomZRotation);

        float randomScale = Random.Range(0.5f, 1.0f);
        muzzleFlash.transform.localScale = new Vector3(randomScale, randomScale, randomScale);

        muzzleFlash.enabled = true;

        yield return new WaitForSeconds(0.1f);

        muzzleFlash.enabled = false;
    }
}
