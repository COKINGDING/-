using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public class UIManager : MonoBehaviour
{
    public Button startButton;
    public Button optionButton;
    public Button shopButton;

    public GameObject rawImage;     // Canvas �ڽ� ��ҵ�
    public GameObject panel;
    public GameObject hpBar;        // Image - hpbar
    public GameObject score;

    private UnityAction action;

    void Start()
    {
        InitUI(); // �ʱ� ��Ȱ��ȭ
        action = () => OnStartButtonClick();
        startButton.onClick.AddListener(action);
        optionButton.onClick.AddListener(delegate { OnButtonClick(optionButton.name); });
        shopButton.onClick.AddListener(() => OnButtonClick(shopButton.name));
    }

    void InitUI()
    {
        // ���� ���� �� UI �ʱ� ����
        hpBar.SetActive(false);
        score.SetActive(false);
    }

    void OnStartButtonClick()
    {
        // UI ����
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
