using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using static UnityEditor.Progress;

public class InventoryManager : MonoBehaviour
{
    [SerializeField] GameObject hand;
    [SerializeField] Animator animator;
    public static InventoryManager Instance;

    public InventorySlot[] inventorySlots;
    public GameObject inventoryItemPrefab;

    int selectedSlot = -1;
    PlayerMovement player;

    private void Awake()
    {
        Instance = this;
    }
    private void Start()
    {
        ChangeSlot(0);
        player = FindObjectOfType<PlayerMovement>();
    }
    void ChangeSlot(int newValue)
    {
        if (selectedSlot >= 0)
        {
            inventorySlots[selectedSlot].DeSelect();
        }

        inventorySlots[newValue].Select();
        selectedSlot = newValue;

        ChangeItem();
    }

    public bool AddItem(Item item)
    {
        for (int i = 0; i < inventorySlots.Length; i++)
        {
            InventorySlot slot = inventorySlots[i];
            InventoryItem itemInSlot = slot.GetComponentInChildren<InventoryItem>();
            if (itemInSlot != null && itemInSlot.item == item && itemInSlot.count < 16 && itemInSlot.item.stackable == true)
            {
                itemInSlot.count++;
                itemInSlot.RefreshCount();
                return true;
            }
        }


        for (int i = 0; i < inventorySlots.Length; i++)
        {
            InventorySlot slot = inventorySlots[i]; 
            InventoryItem itemInSlot = slot.GetComponentInChildren<InventoryItem>();
            if (itemInSlot == null)
            {
                SpawnItem(item, slot);
                return true;
            }
        }

        return false;
    }

    void SpawnItem(Item item, InventorySlot slot)
    {
        GameObject newItemGo = Instantiate(inventoryItemPrefab, slot.transform);
        InventoryItem inventoryItem = newItemGo.GetComponent<InventoryItem>();
        inventoryItem.InitilizeItem(item);

        ChangeItem();
    }

    public Item GetSelectedItem(bool use)
    {
        InventorySlot slot = inventorySlots[selectedSlot];
        InventoryItem itemInSlot = slot.GetComponentInChildren<InventoryItem>();
        if (itemInSlot != null)
        {
            Item item = itemInSlot.item;
            if (use == true)
            {
                itemInSlot.count--;
                if (itemInSlot.count <= 0)
                {
                    Destroy(itemInSlot.gameObject);
                }
                else
                {
                    itemInSlot.RefreshCount();
                }
            }
            return item;
        }
        return null;
    }


    [Header("Open and close inventory")]
    [SerializeField] GameObject mainInventoryUI;
    bool active = true;
    private void Update()
    {
        OpenAndCloseMainInventory();
        ChangeSlot();
        DropItem();
    }

    void OpenAndCloseMainInventory()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            mainInventoryUI.SetActive(active);
            active = !active;
        }
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            mainInventoryUI.SetActive(false);
        }
    }

    public void ChangeItem()
    {
        if (hand.transform.childCount != 0) Destroy(hand.transform.GetChild(0).gameObject);
        Item recivedItem = GetSelectedItem(false);
        if (recivedItem != null)
        {
            GameObject itemInHand = Instantiate(recivedItem.itemPrefab, Vector3.zero, Quaternion.identity);
            itemInHand.transform.parent = hand.transform;
            itemInHand.GetComponent<Rigidbody>().isKinematic = true;
            itemInHand.GetComponent<Collider>().enabled = false;
            itemInHand.transform.SetLocalPositionAndRotation(Vector3.zero, Quaternion.identity);

            ChangeAnimation(recivedItem);
        }
        else
        {
            animator.SetBool("hasRifle", false);
            animator.SetBool("hasPistol", false);
        }
    }

    void ChangeSlot()
    {
        //change active slot
        if (Input.inputString != null)
        {
            //change slot with numbers
            bool isNumber = int.TryParse(Input.inputString, out int number);
            if (isNumber && number > 0 && number < 9)
            {
                ChangeSlot(number - 1);
            }

            //change slots with mouse scroll
            if (Input.GetAxisRaw("Mouse ScrollWheel") < 0) // to right
            {
                ChangeSlot(selectedSlot + 1);
                if (selectedSlot >= 8)
                {
                    selectedSlot = 0;
                    ChangeSlot(selectedSlot);
                }
            }
            if (Input.GetAxisRaw("Mouse ScrollWheel") > 0) // to left
            {
                if (selectedSlot == 0)
                {
                    selectedSlot = 8;
                    inventorySlots[0].DeSelect();
                }
                ChangeSlot(selectedSlot - 1);
            }
        }
    }

    void DropItem()
    {
        if (Input.GetKeyDown(KeyCode.Q) && hand.transform.childCount != 0)
        {
            Item recivedItem = GetSelectedItem(true);
            if (recivedItem != null)
            {
                GameObject itemInHand = hand.transform.GetChild(0).gameObject;
                itemInHand.transform.parent = null;
                itemInHand.GetComponent<Rigidbody>().isKinematic = false;
                itemInHand.GetComponent<Collider>().enabled = true;
                itemInHand.GetComponent<Rigidbody>().AddForce(player.transform.forward * 3f, ForceMode.Impulse);

                InventoryItem itemInSlot = inventorySlots[selectedSlot].GetComponentInChildren<InventoryItem>();
                if (itemInSlot.count <= 0)
                {
                    animator.SetBool("hasRifle", false);
                    animator.SetBool("hasPistol", false);
                }
                else
                {
                    ChangeItem();
                }
            }
        }
    }

    void ChangeAnimation(Item recivedItem)
    {
        switch (recivedItem.animState)
        {
            case AnimState.Default:
                animator.SetBool("hasRifle", false);
                animator.SetBool("hasPistol", false);
                break;
            case AnimState.Pistol:
                animator.SetBool("hasPistol", true);
                animator.SetBool("hasRifle", false);
                break;
            case AnimState.Rifle:
                animator.SetBool("hasRifle", true);
                animator.SetBool("hasPistol", false);
                break;
        }
    }
}
