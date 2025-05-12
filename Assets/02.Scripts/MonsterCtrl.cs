using UnityEngine;
using UnityEngine.AI;
using System.Collections;
using static UnityEditor.PlayerSettings;

public class MonsterCtrl : MonoBehaviour
{

    public enum State
    {
        IDLE,
        TRACE,
        ATTACK,
        HIT,
        DIE
    }


    private GameObject bloodEffect;

    public float traceDist = 20.0f;
    public float attackDist = 2.0f;

    public int monsterHp = 3;


    private Transform playerTr;
    private NavMeshAgent agent;
    private Animator anim;
    private State state = State.IDLE;
    private bool isDie = false;


    void Start()
    {
        playerTr = GameObject.FindWithTag("Player").GetComponent<Transform>();
        agent = GetComponent<NavMeshAgent>();
        anim = GetComponent<Animator>();
        bloodEffect = Resources.Load<GameObject>("BloodSprayEffect");

        StartCoroutine(CheckMonsterState());
        StartCoroutine(MonsterAction());
    }

    IEnumerator CheckMonsterState()
    {
        while (!isDie)
        {
            // 게임 시작 전이면 대기
            if (!GameManager.instance.isGameStarted)
            {
                yield return null;
                continue;
            }

            float distance = Vector3.Distance(playerTr.position, agent.transform.position);

            if (distance <= attackDist)
            {
                state = State.ATTACK;
                yield return new WaitForSeconds(0.5f);
            }
            else if (distance <= traceDist)
                state = State.TRACE;
            else
                state = State.IDLE;

            yield return new WaitForSeconds(0.3f);
        }
    }


    IEnumerator MonsterAction()
    {
        while (!isDie)
        {
            switch (state)
            {
                case State.IDLE:
                    agent.isStopped = true;
                    anim.SetBool("IsTrace", false);

                    break;

                case State.TRACE:
                    agent.SetDestination(playerTr.position);
                    agent.isStopped = false;
                    anim.SetBool("IsTrace", true);
                    anim.SetBool("IsAttack", false);

                    break;

                case State.ATTACK:
                    anim.SetTrigger("IsAttack");

                    break;
                case State.HIT:
                    agent.isStopped = true;
                    anim.SetTrigger("Hit"); 
                    yield return new WaitForSeconds(0.3f);
                    state = State.TRACE; 
                    break;

                case State.DIE:
                    isDie = true;
                    anim.SetBool("IsTrace", false);    
                    anim.SetBool("IsAttack", false);  
                    anim.SetTrigger("Die");
                    agent.isStopped = true;

                    yield return new WaitForSeconds(3f);

                    break;
            }

            yield return null;
        }
    }

    private void OnCollisionEnter(Collision coll)
    {
        if (coll.collider.CompareTag("BULLET"))
        {
            monsterHp--;
            ContactPoint pos = coll.GetContact(0);
            Quaternion rot = Quaternion.LookRotation(-pos.normal);
            GameObject blood = Instantiate(bloodEffect, pos.point, rot);
            Destroy(blood, 1.0f);
            Destroy(coll.gameObject); // 총알 제거

            if (monsterHp <= 0)
            {
                state = State.DIE;
                agent.GetComponent<Collider>().enabled = false;
                StartCoroutine(DieAndDeactivate());
                GameManager.instance.DisplayScore(50);
            }
            else
            {
                state = State.HIT; // 죽지 않았으면 Hit 상태로 전환
            }
        }
    }


    void OnEnable()
    {
        PlayerCtrl.OnPlayerDie += this.OnPlayerDie;

        if (agent == null) agent = GetComponent<NavMeshAgent>();
        if (anim == null) anim = GetComponent<Animator>();
        if (playerTr == null) playerTr = GameObject.FindWithTag("Player")?.transform;

        

        // 리스폰 상태 초기화
        state = State.IDLE;
        monsterHp = 3;
        isDie = false;
        agent.enabled = true;
        GetComponent<Collider>().enabled = true;

        StartCoroutine(CheckMonsterState());
        StartCoroutine(MonsterAction());
    }


    void OnDisable()
    {
        PlayerCtrl.OnPlayerDie -= this.OnPlayerDie;
    }

    void OnPlayerDie()
    {
        StopAllCoroutines();
        state = State.IDLE; 
        agent.isStopped = true;
        anim.SetBool("IsTrace", false);
        anim.SetBool("IsAttack", false);
        anim.SetFloat("Speed", Random.Range(1,5));
        anim.Play("Gangnam Style"); 
    }


    IEnumerator DieAndDeactivate()
    {
        isDie = true;
        anim.SetBool("IsTrace", false);
        anim.SetBool("IsAttack", false);
        anim.SetTrigger("Die");
        agent.isStopped = true;

        // 3초 동안 애니메이션 보여줌
        yield return new WaitForSeconds(3.0f);

        gameObject.SetActive(false);
        monsterHp = 3;
        isDie = false;
        agent.enabled = true;
        GetComponent<Collider>().enabled = true;
    }

}
