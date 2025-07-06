using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem; // Quan trọng: dùng Input System mới

public class DragUI2D : MonoBehaviour, IPointerDownHandler, IDragHandler
{
    private Vector3 offset;
    private float zCoord;

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
}
