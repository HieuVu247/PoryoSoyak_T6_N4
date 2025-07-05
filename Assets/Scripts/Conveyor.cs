using UnityEngine;

public class Conveyor : MonoBehaviour
{
    public Vector3 CenterPoint => transform.position;
    private bool hasBeenVisited = false;
    private static Conveyor[] conveyorCircle; // Danh sách băng chuyền theo thứ tự vòng tròn
    private static int conveyorCount; // Tổng số băng chuyền
    private int circleIndex; // Chỉ số của băng chuyền này trong vòng tròn

    void Awake()
    {
        // Khởi tạo danh sách băng chuyền khi game bắt đầu
        if (conveyorCircle == null)
        {
            conveyorCircle = Object.FindObjectsByType<Conveyor>(FindObjectsSortMode.None);
            conveyorCount = conveyorCircle.Length;
            // Gán chỉ số cho từng băng chuyền
            for (int i = 0; i < conveyorCount; i++)
            {
                conveyorCircle[i].circleIndex = i;
            }
            Debug.Log($"Khởi tạo vòng tròn với {conveyorCount} băng chuyền.");
        }
    }

    public void ResetVisited()
    {
        hasBeenVisited = false;
    }

    public void MarkAsVisited()
    {
        hasBeenVisited = true;
    }

    public bool HasBeenVisited()
    {
        return hasBeenVisited;
    }

    public Conveyor GetNextConveyor()
    {
        // Kiểm tra xem tất cả băng chuyền đã được ghé thăm chưa
        bool allVisited = true;
        foreach (var conveyor in conveyorCircle)
        {
            if (!conveyor.HasBeenVisited())
            {
                allVisited = false;
                break;
            }
        }

        // Nếu tất cả đã được ghé thăm, reset để bắt đầu vòng mới
        if (allVisited)
        {
            foreach (var conveyor in conveyorCircle)
            {
                conveyor.ResetVisited();
            }
            Debug.Log("Đã reset trạng thái tất cả băng chuyền để bắt đầu vòng mới.");
        }

        // Lấy băng chuyền tiếp theo trong vòng tròn
        int nextIndex = (circleIndex + 1) % conveyorCount; // Vòng tròn vô hạn
        Conveyor nextConveyor = conveyorCircle[nextIndex];
        Debug.Log($"Băng chuyền hiện tại: {name}, Chuyển đến: {nextConveyor.name}, Index: {nextIndex}");
        return nextConveyor;
    }
}