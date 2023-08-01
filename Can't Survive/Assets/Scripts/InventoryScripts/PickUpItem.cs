using UnityEngine;

public class PickUpItem : MonoBehaviour
{
    [SerializeField] Item item;

    private void OnTriggerStay(Collider other)
    {
        if (other.transform.CompareTag("Player"))
        {
            bool result = InventoryManager.Instance.AddItem(item);
            if (result == true)
            {
                Destroy(this.gameObject);
            }
        }
    }
}
