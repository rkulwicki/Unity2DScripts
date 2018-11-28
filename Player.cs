using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour {

	//standards and changables
	public float jumpVelocity;
	public Vector2 velocity;
	public float gravity;
	public LayerMask wallMask;
	public LayerMask floorMask;
	public float runSpeed;
    public float leapSpeed;
	public float runJumpHeight;
    public bool isFacingRight;

    public bool canDoubleJump;

    public int maxHealth;
    public int currentHealth;

	//player actions and parts
	private bool walk, walk_left, walk_right, jump, run;
	private Animator animator;
	private BoxCollider2D collider2D;
	private Vector2 colliderExtents;

	//need this temp variable to store runSpeed (default at 1)
	private float runSpeedMultiplier;
	private float runJumpHeightMultiplier;

	public enum PlayerState{
		
		jumping,
		idle,
		walking
	}

	//default is set to idle and not grounded until proven otherwise
	private PlayerState playerState = PlayerState.idle;
	public bool grounded = false;
	private bool isJumpAnim = true;

	//buffer so there's not rigid, "vibraty" movement. It helps, trust me.
	private float colliderBuffer = 0.5f;
    //floor buffer so it collides and is grounded properly
    private float floorBuffer = 0.5f;

    private float tempRunSpeed;



    void Start () {

		Fall ();

		animator = GetComponent<Animator> ();

        collider2D = GetComponent<BoxCollider2D> ();
		colliderExtents.x = collider2D.bounds.extents.x;
		colliderExtents.y = collider2D.bounds.extents.y;

        //stores the runSpeedMultiplier in case we change it for fun while double jumping.
        tempRunSpeed = runSpeed;
    }
	
	//This is our main method that is called every frame. We first check the player input, and
	//then we change the player animation and update the player position based on what the
	//input is.
	void Update () {

		CheckPlayerInput ();

        ChangePlayerSound();

        UpdatePlayerPosition ();

		ChangePlayerAnimation ();

	}

	//obviously update the player position based on given bools: 
	//"run", "walk", "walk_left", "walk_right", and "jump".
	void UpdatePlayerPosition(){

		Vector3 pos = transform.localPosition;
		Vector3 scale = transform.localScale;

		//defaults these to 1 unless otherwise specified from "run"
		runSpeedMultiplier = 1f;
		runJumpHeightMultiplier = 1f;

		//walking
		if (walk) {
			
			/*running
			This can allow for different character variability.
			NOTE: It is important to know that if you are running, you must also be walking. You cannot
				run without first walking which prevents jumping high while standing still.
				You must ALSO be in the jump state.*/
			if (run) {
                // && grounded || run && canDoubleJump
                runSpeedMultiplier = runSpeed;
				runJumpHeightMultiplier = runJumpHeight;
			}

			if (walk_left) {

                isFacingRight = false;

				pos.x -= velocity.x * Time.deltaTime * runSpeedMultiplier;

				scale.x = -1;
			}

			if (walk_right) {

                isFacingRight = true;

                pos.x += velocity.x * Time.deltaTime * runSpeedMultiplier;

				scale.x = 1;
			}

			pos = CheckWallRays (pos, scale.x);

		}

        //initial jump
        if (jump)
        {

            isJumpAnim = true;

            grounded = false;

            if (playerState != PlayerState.jumping)
            {

                canDoubleJump = true;

                playerState = PlayerState.jumping;

                velocity = new Vector2(velocity.x, jumpVelocity * runJumpHeightMultiplier);
            }
            //double jumps - if running, then you leap. Else, just jump.
            else {
                if (canDoubleJump) {
                    //second jump while running
                    if (run)
                    {
                       
                        runSpeed = runSpeed * leapSpeed;
                        
                        velocity = new Vector2(velocity.x, (jumpVelocity)/2);
                        canDoubleJump = false;
                    }
                    //second jump while not running
                    else
                    {

                        velocity = new Vector2(velocity.x, jumpVelocity);
                        canDoubleJump = false;
                    }
                }
            }
        }
    

		//in the air
		if (playerState == PlayerState.jumping) {
		
			pos.y += velocity.y * Time.deltaTime;

			velocity.y -= gravity * Time.deltaTime;

		}

		//are we on the ground?
		if (velocity.y <= 0) 
			pos = CheckFloorRays (pos);

		// did we hit our noggin?
		if (velocity.y >= 0)
			pos = CheckCeilingRays (pos);
		


		//our final change to the player
		transform.localPosition = pos;
		transform.localScale = scale;
	}

	void CheckPlayerInput(){

		//keys to use. May need to change
		//Default is arrow keys to move and wasd for actions
		bool input_left = Input.GetKey (KeyCode.LeftArrow);
		bool input_right = Input.GetKey (KeyCode.RightArrow);
		bool input_D = Input.GetKeyDown (KeyCode.D);
		bool input_S = Input.GetKey (KeyCode.S);

		walk = input_left || input_right;
		walk_left = input_left && !input_right;
		walk_right = !input_left && input_right;

		jump = input_D;

		run = input_S;

	}

    #region CheckRays Region

    Vector3 CheckWallRays(Vector3 pos, float direction){
		
		Vector2 originTop = new Vector2 (pos.x + direction * 0.2f, pos.y + colliderExtents.y - colliderBuffer);
		Vector2 originMiddle = new Vector2 (pos.x + direction * 0.2f, pos.y);
		Vector2 originBottom = new Vector2 (pos.x + direction * 0.2f, pos.y - colliderExtents.y + colliderBuffer);

		RaycastHit2D wallTop = Physics2D.Raycast (originTop, new Vector2 (direction, 0), velocity.x * Time.deltaTime, wallMask);
		RaycastHit2D wallMiddle = Physics2D.Raycast (originMiddle, new Vector2 (direction, 0), velocity.x * Time.deltaTime, wallMask);
		RaycastHit2D wallBottom = Physics2D.Raycast (originBottom, new Vector2 (direction, 0), velocity.x * Time.deltaTime, wallMask);


		// If player collides, then move the player back
		if (wallTop.collider != null || wallMiddle.collider != null || wallBottom.collider != null) {
		
			pos.x -= velocity.x * Time.deltaTime * direction;
		}

		return pos;
	}

	Vector3 CheckFloorRays (Vector3 pos){

		//The origins of the ground, based on the position and the edges of the box collider.
		//The 0.3f is to keep the wall from being detected as ground and popping the player up
		//above the wall.
		Vector2 originLeft = new Vector2 (pos.x - colliderExtents.x + colliderBuffer * 2, pos.y - colliderExtents.y );
		Vector2 originMiddle = new Vector2 (pos.x, pos.y - colliderExtents.y);
		Vector2 originRight = new Vector2 (pos.x + colliderExtents.x - colliderBuffer * 2, pos.y -  colliderExtents.y);

		RaycastHit2D floorLeft = Physics2D.Raycast (originLeft, Vector2.down, velocity.y * Time.deltaTime + floorBuffer, floorMask);
		RaycastHit2D floorMiddle = Physics2D.Raycast (originMiddle, Vector2.down, velocity.y * Time.deltaTime + floorBuffer, floorMask);
		RaycastHit2D floorRight = Physics2D.Raycast (originRight, Vector2.down, velocity.y * Time.deltaTime + floorBuffer, floorMask);

        Debug.DrawLine(originMiddle, new Vector2(originMiddle.x, originMiddle.y - 10));

        //If player collides with the floor, then move the player back to the top
        if (floorLeft.collider != null || floorMiddle.collider != null || floorRight.collider != null) {

			//default colliding with the right
			RaycastHit2D hitRay = floorRight;

			if (floorLeft) {
				hitRay = floorLeft;
			} else if (floorMiddle) {
				hitRay = floorMiddle;
			} else if (floorRight) {
				hitRay = floorRight;
			}

			playerState = PlayerState.idle;

			grounded = true;

            runSpeed = tempRunSpeed;

            isJumpAnim = false;

			velocity.y = 0;

			pos.y = hitRay.collider.bounds.center.y + (hitRay.collider.bounds.size.y  / 2 + colliderExtents.y) ;

		} else {
			

			if (playerState != PlayerState.jumping) {

				Fall ();
			}
		}

		return pos;
	}

	Vector3 CheckCeilingRays (Vector3 pos){
		
		Vector2 originLeft = new Vector2 (pos.x - colliderExtents.x + colliderBuffer, pos.y + colliderExtents.y);
		Vector2 originMiddle = new Vector2 (pos.x, pos.y + colliderExtents.y);
		Vector2 originRight = new Vector2 (pos.x + colliderExtents.x - colliderBuffer, pos.y + colliderExtents.y);

		RaycastHit2D ceilLeft = Physics2D.Raycast (originLeft, Vector2.up, velocity.y * Time.deltaTime, floorMask);
		RaycastHit2D ceilMiddle = Physics2D.Raycast (originMiddle, Vector2.up, velocity.y * Time.deltaTime, floorMask);
		RaycastHit2D ceilRight = Physics2D.Raycast (originRight, Vector2.up, velocity.y * Time.deltaTime, floorMask);

		//If player collides, then move the player back
		if (ceilLeft.collider != null || ceilMiddle.collider != null || ceilRight.collider != null) {
		
			//default colliding with the left ceiling
			RaycastHit2D hitRay = ceilLeft;

			if (ceilLeft) {
				hitRay = ceilLeft;
			} else if (ceilMiddle) {
				hitRay = ceilMiddle;
			} else if (ceilRight) {
				hitRay = ceilRight;
			}

            if (hitRay.collider.tag == "woodBlock" || hitRay.collider.tag == "brickBlock" || hitRay.collider.tag == "metalBlock") {

                hitRay.collider.GetComponent<blockScript>().BlockBounce();
            }
				
			pos.y = hitRay.collider.bounds.center.y - (hitRay.collider.bounds.size.y  / 2 + colliderExtents.y) ;

			Fall ();
		}

		return pos;

	}

    #endregion

    void Fall(){
		
		velocity.y = 0;

		playerState = PlayerState.jumping;

		grounded = false;
	}

	void ChangePlayerAnimation(){

        //leaping
        if (!grounded && !canDoubleJump && run) {
            animator.SetInteger("AnimState", 5);
        }

        //jumping 	(activated in UpdatePlayerPos -> jump ...)
        //			(deactivated in CheckFloorRays) 
        else if (isJumpAnim) {
            if (velocity.y > 0) {
                animator.SetInteger("AnimState", 2);
            } else {
                animator.SetInteger("AnimState", 3);
            }

            //walking
        }

        //running
        else if (run && walk){
            animator.SetInteger("AnimState", 4);
        }

        //walking
        else if (walk) {
            animator.SetInteger("AnimState", 1);
        }

		//standing
		else {
			animator.SetInteger ("AnimState", 0);
		}
			
		//stops the jump animation if player hits gound
		//if (velocity.y == 0) ????
	}

    void ChangePlayerSound() {

        //plays sound. This is in the SoundManager script, not game object.
        if (jump && grounded)
        {

            //DOESN'T ALWAYS PLAY. Should probably fix
            SoundManager.PlaySound("sonJump");
        }

        /* THIS DOESN'T WORK. IT REVERBS.
        if (animator.GetInteger("AnimState") == 1) {

            SoundManager.PlaySound("sonWalk");
        }
        */
    }
}
