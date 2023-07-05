using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickUpSystem : MonoBehaviour
{
    [SerializeField] GameObject hand;
    [HideInInspector] public bool hasItem;
    Animator animator;

    private void Awake()
    {
        animator = GetComponentInChildren<Animator>();
    }

    private void Update()
    {
        DropItem();
    }

    private void OnCollisionEnter(Collision collision)
    {
        PickUpItem(collision);
    }

    void PickUpItem(Collision collision)
    {
        if (collision.gameObject.CompareTag("Item") && !hasItem)
        {
            collision.transform.parent = hand.transform;
            collision.rigidbody.isKinematic = true;
            collision.collider.enabled = false;
            collision.transform.SetLocalPositionAndRotation(Vector3.zero, Quaternion.identity);
            animator.SetBool("hasRifle", true);
            hasItem = true;
        }
    }

    void DropItem()
    {
        if (hasItem && Input.GetKeyDown(KeyCode.Q))
        {
            hand.GetComponentInChildren<Rigidbody>().isKinematic = false;
            hand.GetComponentInChildren<Rigidbody>().AddForce(200f * Time.deltaTime * transform.forward, ForceMode.Impulse);
            hand.GetComponentInChildren<Rigidbody>().AddForce(80f * Time.deltaTime * transform.up, ForceMode.Impulse);
            hand.GetComponentInChildren<Collider>().enabled = true;
            hand.transform.DetachChildren();
            animator.SetBool("hasRifle", false);
            hasItem = false;
        }
    }
}
