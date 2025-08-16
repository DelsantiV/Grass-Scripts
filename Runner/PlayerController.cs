using System.Collections;
using UnityEngine;
using UnityEngine.Events;

public class PlayerController : MonoBehaviour
{
    private Rigidbody playerRb;
    private Animator playerAnim;
    public ParticleSystem explosionParticle;
    public ParticleSystem dirtParticle;
    public AudioClip jumpSound;
    public AudioClip deathSound;
    private AudioSource playerAudio;
    public float jumpForce;
    public float doubleJumpForce;
    private bool canJump = true;
    public float gravityModifier = 1.0f;
    private bool playerOnGround = true;
    private bool inFirstJump = false;
    public bool gameOver = false;
    public bool doubleSpeed = false;
    public UnityEvent OnGameOver;
    private Quaternion baseRotation;
    private Vector3 basePosition;

    // Start is called before the first frame update
    void Awake()
    {
        playerRb = GetComponent<Rigidbody>();
        Physics.gravity *= gravityModifier;
        playerAnim = GetComponent<Animator>();
        playerAudio = GetComponent<AudioSource>();
        OnGameOver = new();
        baseRotation = transform.rotation;
        basePosition = transform.position;
    }

    public void ResetPlayer()
    {
        transform.SetPositionAndRotation(basePosition, baseRotation);
        canJump = true;
        playerOnGround = true;
        playerAnim.SetBool("Death_b", false);
        playerAnim.SetTrigger("BackToIdle");
        playerRb.constraints = RigidbodyConstraints.None;
        playerAnim.SetFloat("Speed_Multiplier", 0);
    }

    // Update is called once per frame
    void Update()
    {
        if (!gameOver)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                Debug.Log(playerOnGround);
                Debug.Log(canJump);
                if (playerOnGround && canJump)
                {
                    playerRb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
                    playerAnim.SetTrigger("Jump_trig");
                    dirtParticle.Stop();
                    playerOnGround = false;
                    inFirstJump = true;
                    playerAudio.PlayOneShot(jumpSound, 0.8f);
                    canJump = false;
                    StartCoroutine(JumpCoolDown());
                }


                else
                {
                    if (inFirstJump)
                    {
                        inFirstJump = false;
                        if (transform.position.y > 2f)
                        {
                            playerRb.AddForce(Vector3.up * doubleJumpForce, ForceMode.Impulse);
                            playerAnim.Play("Running_Jump", 3, 0f);
                            playerAudio.PlayOneShot(jumpSound, 1.2f);
                        }
                    }
                }
            }

            if (Input.GetKey(KeyCode.LeftShift))
            {
                doubleSpeed = true;
                playerAnim.SetFloat("Speed_Multiplier", 2.0f);
            }
            else if (doubleSpeed)
            {
                doubleSpeed = false;
                playerAnim.SetFloat("Speed_Multiplier", 1.0f);
            }
            if (transform.rotation != baseRotation) transform.rotation = baseRotation;
            if (Mathf.Abs(transform.position.z - basePosition.z) > 0.1f) transform.position = new(basePosition.x, transform.position.y, basePosition.z);
        }
    }
    private IEnumerator JumpCoolDown()
    {
        yield return new WaitForSeconds(0.2f);
        canJump = true;
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ground") && !gameOver)
        {
            playerOnGround = true;
            dirtParticle.Play();
            inFirstJump = false;
        }

        else if (collision.gameObject.CompareTag("Obstacle") && !gameOver)
        {   
            gameOver = true;
            doubleSpeed = false;
            dirtParticle.Stop();
            playerAnim.SetBool("Death_b", true);
            playerAnim.SetInteger("DeathType_int", 1);
            playerAnim.SetFloat("Speed_Multiplier", 0f);
            explosionParticle.Play();
            playerAudio.PlayOneShot(deathSound, 0.8f);
            playerRb.constraints = RigidbodyConstraints.FreezeRotation;
            OnGameOver.Invoke();
            playerRb.linearVelocity = Vector3.zero;
        }
    }

}
