using UnityEngine;
using UnityEngine.UI;

public class MenuManager : MonoBehaviour
{
    [Header("Canvas Panels")]
    public GameObject menuCanvas;
    public GameObject selectLevelCanvas;
    public GameObject guideCanvas;

    [Header("Buttons")]
    public Button startButton;
    public Button guideButton;
    public Button backFromSelectButton;
    public Button backFromGuideButton;

    private void Awake()
    {
        startButton.onClick.AddListener(OpenSelectLevel);
        guideButton.onClick.AddListener(OpenGuide);

        backFromSelectButton.onClick.AddListener(BackToMenu);
        backFromGuideButton.onClick.AddListener(BackToMenu);

        menuCanvas.SetActive(true);
        selectLevelCanvas.SetActive(false);
        guideCanvas.SetActive(false);
    }

    public void OpenSelectLevel()
    {
        menuCanvas.SetActive(false);
        selectLevelCanvas.SetActive(true);
    }

    public void OpenGuide()
    {
        menuCanvas.SetActive(false);
        guideCanvas.SetActive(true);
    }

    public void BackToMenu()
    {
        selectLevelCanvas.SetActive(false);
        guideCanvas.SetActive(false);
        menuCanvas.SetActive(true);
    }
}
