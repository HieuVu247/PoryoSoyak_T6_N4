using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using System.Collections.Generic;

public class FoodController : MonoBehaviour, IPointerDownHandler, IDragHandler, IPointerUpHandler
{
    private Vector3 offset;
    private float zCoord;
    private Conveyor currentConveyor;
    private Conveyor previousConveyor; // Lưu băng chuyền trước đó
    private int moveCount = 0;
    private bool isMoving = false;
    private List<Conveyor> movementHistory = new List<Conveyor>(); // Lưu lịch sử di chuyển
    private int currentHistoryIndex = -1; // Chỉ số trong lịch sử để tái sử dụng

    public int MoveCount => moveCount;

    void Start()
    {
        movementHistory.Clear();
        currentHistoryIndex = -1;
        previousConveyor = null;
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        zCoord = Camera.main.WorldToScreenPoint(transform.position).z;
        Vector2 mousePos = Mouse.current.position.ReadValue();
        Vector3 mouseWorld = Camera.main.ScreenToWorldPoint(new Vector3(mousePos.x, mousePos.y, zCoord));
        offset = transform.position - mouseWorld;
    }

    public void OnDrag(PointerEventData eventData)
    {
        Vector2 mousePos = Mouse.current.position.ReadValue();
        Vector3 mouseWorld = Camera.main.ScreenToWorldPoint(new Vector3(mousePos.x, mousePos.y, zCoord));
        transform.position = mouseWorld + offset;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        Collider2D[] colliders = Physics2D.OverlapPointAll(transform.position);
        bool foundConveyor = false;
        foreach (var collider in colliders)
        {
            Conveyor conveyor = collider.GetComponent<Conveyor>();
            if (conveyor != null)
            {
                currentConveyor = conveyor;
                transform.position = conveyor.CenterPoint;
                Debug.Log($"Đặt thức ăn lên băng chuyền: {currentConveyor.name} tại {conveyor.transform.position}");
                foundConveyor = true;
                break;
            }
        }
        if (!foundConveyor)
        {
            Debug.LogWarning("Thức ăn không được đặt lên băng chuyền nào! Kiểm tra component Conveyor, collider, hoặc layer. Số collider tìm thấy: " + colliders.Length);
            currentConveyor = null;
        }
    }

    public void MoveToNextConveyor()
    {
        if (isMoving || currentConveyor == null)
        {
            Debug.LogWarning($"Không thể di chuyển: isMoving={isMoving}, currentConveyor={(currentConveyor != null ? currentConveyor.name : "null")}");
            return;
        }

        isMoving = true;
        Conveyor nextConveyor = GetNextConveyorFromHistory();
        if (nextConveyor != null)
        {
            previousConveyor = currentConveyor; // Lưu băng chuyền hiện tại
            moveCount++;
            transform.position = nextConveyor.CenterPoint;
            // Cập nhật lịch sử nếu đây là lần đi đầu tiên
            if (currentHistoryIndex < movementHistory.Count - 1)
            {
                movementHistory.Add(nextConveyor);
                currentHistoryIndex++;
            }
            Debug.Log($"Thức ăn di chuyển đến {nextConveyor.name}. Tổng số lần di chuyển: {moveCount}, Trước đó: {(previousConveyor != null ? previousConveyor.name : "null")}");
            Debug.DrawRay(transform.position, (nextConveyor.CenterPoint - transform.position), Color.green, 2f);
            currentConveyor = nextConveyor;
        }
        else
        {
            // Kiểm tra xem đã đi hết vòng chưa
            Conveyor[] conveyors = Object.FindObjectsByType<Conveyor>(FindObjectsSortMode.None);
            if (currentHistoryIndex >= conveyors.Length - 1)
            {
                // Reset và bắt đầu lại từ đầu lịch sử
                previousConveyor = currentConveyor;
                currentHistoryIndex = 0;
                currentConveyor = movementHistory[0]; // Quay lại điểm đầu (a)
                transform.position = currentConveyor.CenterPoint;
                moveCount++;
                Debug.Log($"Bắt đầu vòng mới từ {currentConveyor.name}. Tổng số lần di chuyển: {moveCount}, Trước đó: {(previousConveyor != null ? previousConveyor.name : "null")}");

                // Di chuyển đến bước tiếp theo trong lịch sử
                nextConveyor = GetNextConveyorFromHistory();
                if (nextConveyor != null)
                {
                    moveCount++;
                    transform.position = nextConveyor.CenterPoint;
                    Debug.Log($"Tiếp tục đến {nextConveyor.name}. Tổng số lần di chuyển: {moveCount}");
                    Debug.DrawRay(transform.position, (nextConveyor.CenterPoint - transform.position), Color.green, 2f);
                    currentConveyor = nextConveyor;
                }
                else
                {
                    Debug.LogWarning("Không tìm thấy băng chuyền tiếp theo trong lịch sử!");
                }
            }
            else
            {
                Debug.LogWarning("Không tìm thấy băng chuyền tiếp theo!");
            }
        }
        isMoving = false;
    }

    private Conveyor GetNextConveyorFromHistory()
    {
        Conveyor[] conveyors = Object.FindObjectsByType<Conveyor>(FindObjectsSortMode.None);
        if (currentHistoryIndex < 0 || currentConveyor == null)
        {
            // Lần đầu tiên, tìm băng chuyền gần nhất
            float minDistance = float.MaxValue;
            Conveyor nextConveyor = null;

            foreach (Conveyor conveyor in conveyors)
            {
                if (conveyor == currentConveyor || conveyor == previousConveyor) continue;

                float distance = Vector3.Distance(transform.position, conveyor.CenterPoint);
                if (distance < minDistance && distance > 0)
                {
                    minDistance = distance;
                    nextConveyor = conveyor;
                }
            }
            return nextConveyor;
        }
        else
        {
            // Sau lần đầu, lấy theo lịch sử
            if (currentHistoryIndex + 1 < movementHistory.Count)
            {
                return movementHistory[currentHistoryIndex + 1];
            }
            else
            {
                // Tìm băng chuyền gần nhất chưa trong lịch sử
                float minDistance = float.MaxValue;
                Conveyor nextConveyor = null;

                foreach (Conveyor conveyor in conveyors)
                {
                    if (conveyor == currentConveyor || conveyor == previousConveyor || movementHistory.Contains(conveyor)) continue;

                    float distance = Vector3.Distance(transform.position, conveyor.CenterPoint);
                    if (distance < minDistance && distance > 0)
                    {
                        minDistance = distance;
                        nextConveyor = conveyor;
                    }
                }
                return nextConveyor;
            }
        }
    }

    public void ResetMoveCount()
    {
        moveCount = 0;
        movementHistory.Clear();
        currentHistoryIndex = -1;
        previousConveyor = null;
        currentConveyor = null;
        Debug.Log("Đã reset số lần di chuyển, lịch sử, và trạng thái băng chuyền.");
    }

    private void Update()
    {
        if (Keyboard.current != null && Keyboard.current.spaceKey.wasPressedThisFrame)
        {
            MoveToNextConveyor();
        }
    }

    [ContextMenu("Test Move")]
    public void TestMove()
    {
        MoveToNextConveyor();
    }
}