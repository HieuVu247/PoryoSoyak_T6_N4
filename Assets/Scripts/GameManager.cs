using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public Button runButton;
    public FoodController food;

    private void Start()
    {
        if (runButton != null)
        {
            runButton.onClick.AddListener(OnRunButtonClicked);
            Debug.Log("Đã gắn sự kiện cho nút Run.");
        }
        else
        {
            Debug.LogError("RunButton chưa được gán trong Inspector!");
        }
        if (food == null)
        {
            Debug.LogError("FoodController chưa được gán trong Inspector!");
        }
    }

    private void OnRunButtonClicked()
    {
        if (food != null)
        {
            food.MoveToNextConveyor();
            Debug.Log($"Đã gọi MoveToNextConveyor. Tổng số lần di chuyển: {food.MoveCount}");
        }
        else
        {
            Debug.LogError("FoodController là null, không thể gọi MoveToNextConveyor!");
        }
    }

    public void ResetGame()
    {
        Conveyor[] conveyors = Object.FindObjectsByType<Conveyor>(FindObjectsSortMode.None);
        foreach (var conveyor in conveyors)
        {
            conveyor.ResetVisited();
        }
        if (food != null)
        {
            food.ResetMoveCount();
        }
        Debug.Log("Đã reset game: trạng thái băng chuyền và số lần di chuyển.");
    }
}