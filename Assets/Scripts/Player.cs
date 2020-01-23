using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;

namespace TileVania.Player
{
    public class Player : MonoBehaviour
    {
        [SerializeField] private float playerHorizontalSpeed = 1.0f;
        [SerializeField] private float climbSpeed = 5.0f;
        [SerializeField] private float jumpForce = 5.0f;
        private Rigidbody2D myRigitbody;
        private Animator myAnimator;
        private Collider2D myCollider;

        float initialGravityScale;
        



        // Start is called before the first frame update
        void Start()
        {
            myRigitbody = GetComponent<Rigidbody2D>();
            myAnimator = GetComponent<Animator>();
            myCollider = GetComponent<Collider2D>();

            initialGravityScale = myRigitbody.gravityScale;
        }

        // Update is called once per frame
        void Update()
        {
            Run();
            FlipSpite();
            Jump();
            Climb();
        }

        private void Jump()
        {
            if (!myCollider.IsTouchingLayers(LayerMask.GetMask("Ground")))
            {
                return;
            }
            if (CrossPlatformInputManager.GetButtonDown("Jump"))
            {
                Vector2 jumpVelocityToAdd = new Vector2(0, jumpForce);
                myRigitbody.velocity += jumpVelocityToAdd;
            }
        }

        private void Climb()
        {
            if (!myCollider.IsTouchingLayers(LayerMask.GetMask("Climb")))
            {
                myAnimator.SetBool("isClimbing", false);
                myRigitbody.gravityScale = initialGravityScale;
                return;
            }

            float controlThrow = CrossPlatformInputManager.GetAxis("Vertical");
            Vector2 climbVelocity = new Vector2(myRigitbody.velocity.x, controlThrow * climbSpeed);
            myRigitbody.velocity = climbVelocity;
            myRigitbody.gravityScale = 0.0f;


            bool playerHasVerticalSpeed = Mathf.Abs(myRigitbody.velocity.y) > Mathf.Epsilon;
            myAnimator.SetBool("isClimbing", playerHasVerticalSpeed);
        }

        private void Run()
        {
            float controlThrow = CrossPlatformInputManager.GetAxis("Horizontal"); // -1 to +1
            Vector2 playerVelocity = new Vector2(controlThrow * playerHorizontalSpeed, myRigitbody.velocity.y);
            myRigitbody.velocity = playerVelocity;
        }

        private void FlipSpite()
        {
            if (Mathf.Abs(myRigitbody.velocity.x) > Mathf.Epsilon)
            {
                myAnimator.SetBool("isRunning", true);
                transform.localScale = new Vector2(Mathf.Sign(myRigitbody.velocity.x), transform.localScale.y);
            }
            else
            {
                myAnimator.SetBool("isRunning", false);
            }
        }
    }
}

