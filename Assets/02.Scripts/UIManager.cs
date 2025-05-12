using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public class UIManager : MonoBehaviour
{
    public Button startButton;
    public Button optionButton;
    public Button shopButton;

    public GameObject rawImage;     // Canvas 자식 요소들
    public GameObject panel;
    public GameObject hpBar;        // Image - hpbar
    public GameObject score;

    private UnityAction action;

    void Start()
    {
        InitUI(); // 초기 비활성화
        action = () => OnStartButtonClick();
        startButton.onClick.AddListener(action);
        optionButton.onClick.AddListener(delegate { OnButtonClick(optionButton.name); });
        shopButton.onClick.AddListener(() => OnButtonClick(shopButton.name));
    }

    void InitUI()
    {
        // 게임 시작 전 UI 초기 상태
        hpBar.SetActive(false);
        score.SetActive(false);
    }

    void OnStartButtonClick()
    {
        // UI 정리
        rawImage.SetActive(false);
        panel.SetActive(false);
        startButton.gameObject.SetActive(false);
        optionButton.gameObject.SetActive(false);
        shopButton.gameObject.SetActive(false);

        hpBar.SetActive(true);
        score.SetActive(true);

        GameManager.instance.isGameStarted = true;
        GameManager.instance.BeginGame(); 
    }

    public void OnButtonClick(string msg)
    {
       // Debug.Log($"Click Button");
    }
}
