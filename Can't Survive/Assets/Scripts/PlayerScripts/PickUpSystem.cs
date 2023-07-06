using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PickUpSystem : MonoBehaviour
{
    [SerializeField] GameObject hand;
    [SerializeField] GameObject pistolPouch;
    [SerializeField] GameObject back;
    [HideInInspector] public bool hasRifle;
    [HideInInspector] public bool hasPistol;
    [HideInInspector] public bool hasItemInHand;

    [SerializeField] KeyCode switchToPistol = KeyCode.Alpha2; 
    [SerializeField] KeyCode switchToRifle = KeyCode.Alpha1;

    
    Animator animator;

    private void Awake()
    {
        animator = GetComponentInChildren<Animator>();
    }

    private void Update()
    {
        DropItem();
        SwitchGun();
    }

    private void OnCollisionEnter(Collision collision)
    {
        PickUpItem(collision);
    }

    void PickUpItem(Collision collision)
    {
        if (collision.gameObject.CompareTag("Rifle") && !hasRifle && !hasItemInHand)
        {
            collision.transform.parent = hand.transform;
            collision.rigidbody.isKinematic = true;
            collision.collider.enabled = false;
            collision.transform.SetLocalPositionAndRotation(Vector3.zero, Quaternion.identity);
            animator.SetBool("hasRifle", true);
            hasRifle = true;
            hasItemInHand = true;
        }
        if (collision.gameObject.CompareTag("Pistol") && !hasPistol && !hasItemInHand)
        {
            collision.transform.parent = hand.transform;
            collision.rigidbody.isKinematic = true;
            collision.collider.enabled = false;
            collision.transform.SetLocalPositionAndRotation(Vector3.zero, Quaternion.identity);
            animator.SetBool("hasPistol", true);
            hasPistol = true;
            hasItemInHand = true;
        }
    }

    void SwitchGun()
    {
        if (Input.GetKeyDown(switchToPistol) && hasPistol && hasItemInHand)
        {
            hand.transform.GetChild(0).transform.parent = pistolPouch.transform;
            pistolPouch.transform.GetChild(0).transform.SetLocalPositionAndRotation(Vector3.zero, Quaternion.identity);
            hasItemInHand = false;
            animator.SetBool("hasPistol", false);
        }
        else if(Input.GetKeyDown(switchToPistol) && hasPistol && !hasItemInHand)
        {
            pistolPouch.transform.GetChild(0).transform.parent = hand.transform;
            hand.transform.GetChild(0).transform.SetLocalPositionAndRotation(Vector3.zero, Quaternion.identity);
            hasItemInHand = true;
            animator.SetBool("hasPistol", true);
        }


        if (Input.GetKeyDown(switchToRifle) && hasRifle && hasItemInHand)
        {
            hand.transform.GetChild(0).transform.parent = back.transform;
            back.transform.GetChild(0).transform.SetLocalPositionAndRotation(Vector3.zero, Quaternion.identity);
            hasItemInHand = false;
            animator.SetBool("hasRifle", false);
        }
        else if (Input.GetKeyDown(switchToRifle) && hasRifle && !hasItemInHand)
        {
            back.transform.GetChild(0).transform.parent = hand.transform;
            hand.transform.GetChild(0).transform.SetLocalPositionAndRotation(Vector3.zero, Quaternion.identity);
            hasItemInHand = true;
            animator.SetBool("hasRifle", true);
        }
    }

    void DropItem()
    {
        if (hasItemInHand && hasRifle && Input.GetKeyDown(KeyCode.Q))
        {
            hand.GetComponentInChildren<Rigidbody>().isKinematic = false;
            hand.GetComponentInChildren<Rigidbody>().AddForce(200f * Time.deltaTime * transform.forward, ForceMode.Impulse);
            hand.GetComponentInChildren<Rigidbody>().AddForce(80f * Time.deltaTime * transform.up, ForceMode.Impulse);
            hand.GetComponentInChildren<Collider>().enabled = true;
            hand.transform.DetachChildren();
            animator.SetBool("hasRifle", false);
            hasRifle = false;
            hasItemInHand = false;
        }

        if (hasItemInHand && hasPistol && Input.GetKeyDown(KeyCode.Q))
        {
            hand.GetComponentInChildren<Rigidbody>().isKinematic = false;
            hand.GetComponentInChildren<Rigidbody>().AddForce(200f * Time.deltaTime * transform.forward, ForceMode.Impulse);
            hand.GetComponentInChildren<Rigidbody>().AddForce(80f * Time.deltaTime * transform.up, ForceMode.Impulse);
            hand.GetComponentInChildren<Collider>().enabled = true;
            hand.transform.DetachChildren();
            animator.SetBool("hasPistol", false);
            hasPistol = false;
            hasItemInHand = false;
        }
    }
}
