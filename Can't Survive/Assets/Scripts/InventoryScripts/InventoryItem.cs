
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class InventoryItem : MonoBehaviour, IBeginDragHandler, IEndDragHandler, IDragHandler
{

    public Image image;
    public TextMeshProUGUI countText;

    [HideInInspector] public Transform ParentAfterDrag;
    [HideInInspector] public Item item;
    [HideInInspector] public int count = 1;


    public void InitilizeItem(Item newItem)
    {
        item = newItem;
        image.sprite = newItem.image;
        RefreshCount();
    }
    public void RefreshCount()
    {
        countText.text = count.ToString();
        bool textActive = count > 1;
        countText.gameObject.SetActive(textActive);
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        image.raycastTarget = false;
        ParentAfterDrag = transform.parent;
        transform.SetParent(transform.root);
        InventoryManager.Instance.ChangeItem();
    }

    public void OnDrag(PointerEventData eventData)
    {
        transform.position = Input.mousePosition;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        image.raycastTarget = true;
        transform.SetParent(ParentAfterDrag);
        InventoryManager.Instance.ChangeItem();
    }

}
