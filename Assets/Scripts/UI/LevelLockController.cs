using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class LevelLockController : MonoBehaviour
{
    [System.Serializable]
    public class LevelButtonData
    {
        public Button button;        
        public Image image;          
        public Sprite normalSprite;  
        public Sprite lockedSprite;
        public TextMeshProUGUI label;
    }

    public LevelButtonData[] levels;


    public int highestUnlockedLevel = 1;

    void Start()
    {
        UpdateAllButtons();
    }

    public void UpdateAllButtons()
    {
        for (int i = 0; i < levels.Length; i++)
        {
            bool unlocked = (i + 1) <= highestUnlockedLevel;

            levels[i].button.interactable = unlocked;

            levels[i].image.sprite = unlocked
                ? levels[i].normalSprite
                : levels[i].lockedSprite;
            if (levels[i].label != null)
                levels[i].label.gameObject.SetActive(unlocked);
        }
    }


    public void OnLevelComplete(int levelNumber)
    {
        highestUnlockedLevel = Mathf.Max(highestUnlockedLevel, levelNumber + 1);
        UpdateAllButtons();

        PlayerPrefs.SetInt("UnlockedLevel", highestUnlockedLevel);
    }

    public void LoadProgress()
    {
        highestUnlockedLevel = PlayerPrefs.GetInt("UnlockedLevel", highestUnlockedLevel);
        UpdateAllButtons();
    }
}
