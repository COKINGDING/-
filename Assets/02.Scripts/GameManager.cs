using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;
using System.Collections.Generic;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    public bool isGameStarted = false;

    [Header("플레이어 & UI")]
    public PlayerCtrl player;
    public Image hpBar;
    public TextMeshProUGUI scoreText;

    [Header("몬스터 스폰 관련")]
    public GameObject monsterPrefab;
    public int maxMonster = 10;
    public float spawnInterval = 3f;
    public string spawnGroupName = "SpawnPointGroup";

    private List<Transform> points = new List<Transform>();
    private List<GameObject> monsterPool = new List<GameObject>();
    private int totScore = 0;
    private bool isGameOver = false;

    public bool IsGameOver
    {
        get { return isGameOver; }
        set { isGameOver = value; }
    }

    void Awake()
    {
        if (instance == null) instance = this;
        else Destroy(gameObject);

        DontDestroyOnLoad(this.gameObject);
    }

    void Start()
    {
        InitSpawnPoints();
        InitMonsterPool();
        DisplayScore(0);

    }

    void Update()
    {
        if (player != null && hpBar != null)
        {
            float ratio = player.currHp / 100f;
            hpBar.fillAmount = Mathf.Clamp01(ratio);
        }
    }
    public void BeginGame()
    {
        InvokeRepeating(nameof(SpawnMonster), 2f, spawnInterval);
    }
    void InitSpawnPoints()
    {
        Transform group = GameObject.Find(spawnGroupName)?.transform;
        if (group == null) return;

        foreach (Transform point in group.GetComponentsInChildren<Transform>())
        {
            if (point != group)
                points.Add(point);
        }
    }

    void InitMonsterPool()
    {
        for (int i = 0; i < maxMonster; i++)
        {
            GameObject m = Instantiate(monsterPrefab);
            m.name = $"Monster_{i:D2}";
            m.SetActive(false);
            monsterPool.Add(m);
        }
    }

    GameObject GetMonsterFromPool()
    {
        foreach (var m in monsterPool)
        {
            if (!m.activeSelf)
                return m;
        }
        return null;
    }

    void SpawnMonster()
    {
        if (isGameOver) return;

        GameObject monster = GetMonsterFromPool();
        if (monster != null && points.Count > 0)
        {
            int idx = Random.Range(0, points.Count);
            monster.transform.SetPositionAndRotation(points[idx].position, points[idx].rotation);
            monster.SetActive(true);
        }
    }

   

    public void DisplayScore(int score)
    {
        totScore += score;
        scoreText.text = $"<color=#00ff00>SCORE :</color> <color=#ff0000>{totScore:#,##0}</color>";
    }

    public void SaveScore()
    {
        int best = PlayerPrefs.GetInt("HighScore", 0);
        if (totScore > best)
        {
            PlayerPrefs.SetInt("HighScore", totScore);
        }
    }
}
