using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Attractor : MonoBehaviour
{
    private bool isAttracting;
    // ally is the other player
    [SerializeField] private float range;
    [SerializeField] private float attractionIntensity;
    [SerializeField] private Transform ally;
    private Rigidbody2D allyRb;
    private Transform player;
    private Animator allyAnim;

    private void Awake()
    {
        player = transform;
        allyRb = ally.GetComponent<Rigidbody2D>();
        allyAnim = ally.GetComponent<Animator>();
    }

    private void FixedUpdate()
    {
        if (Input.GetKey(KeyCode.Space))
        {
            float distance = Vector2.Distance(ally.position, player.position);
            if (distance < range && distance > 0.6) 
            {
                Vector2 dir = -(ally.position - player.position) / distance;
                dir *= 1 / distance;
                allyRb.AddForce(dir * attractionIntensity, ForceMode2D.Impulse);
                allyAnim.SetBool("is_tumbling",true);
                allyAnim.SetBool("is_running",false);
                allyAnim.SetBool("is_idle",false);
                allyAnim.SetBool("is_jumping",false);
                allyAnim.SetBool("is_falling",false);
                allyAnim.SetBool("isGrounded",false);
            }
        }
    }
}
