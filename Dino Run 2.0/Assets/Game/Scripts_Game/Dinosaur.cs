using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dinosaur : MonoBehaviour
{
    private Rigidbody2D dinoRb;
    private Animator animator;

    [SerializeField] private float upForce;
    [SerializeField] private Transform groundCheck;
    [SerializeField] private LayerMask ground;
    [SerializeField] private float radius;

    void Start()
    {
        dinoRb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
    }
    void Update()
    {
        bool isGrounded = Physics2D.OverlapCircle(groundCheck.position, radius, ground);
        animator.SetBool("isGrounded", isGrounded);
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (isGrounded)
            {
                dinoRb.AddForce(Vector2.up * upForce);
            }

        }
    }
    void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(groundCheck.position, radius);
    }
    void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Obstacle"))
        {
            Manager.Instance.ShowGameOverScreen();
            animator.SetTrigger("Die");
            Time.timeScale = 0f;
        }
    }
}
