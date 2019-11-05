using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XboxCtrlrInput;
public class PlayerController : MonoBehaviour
{
    public int m_nPlayerID = 4;

    float m_fSpeed;
    float m_fTimeWhenStunned = 0.0f;
    float m_fTimeWhenKilled = 0.0f;

    [SerializeField] float m_fMaxSpeed = 14;
    [SerializeField] float m_fCoinSpeed = 7;
    [SerializeField] float m_fSphereCastDist = 10;
    [SerializeField] float m_fSphereCastRadius = 1;
    [SerializeField] float m_fShovePower = 10;
    [SerializeField] float m_fStunTime = 10;
    [SerializeField] float m_fGravityMultiplier = 3;
    [SerializeField] float m_fRespawnTime = 2;
    [SerializeField] float m_fDropCoinCooldown = 1;
    [SerializeField] string m_szColour = "";
    [SerializeField] float m_fRespawnHeight = 30;
    [SerializeField] float m_fRespawnOffset = 2;
    [SerializeField] GameObject m_Grid;
    public bool m_bIsFalling = false;
    
    [HideInInspector] public bool m_bHasCoin;
    [HideInInspector] public bool m_bCanPickUpCoin = true;
    [HideInInspector] public bool m_bIsDead = false;

    private bool m_bStunned = false;

    Rigidbody m_Controller;
    private UIController m_UiController;

    // This will be used to store colliders that need to be accessed from multiple methods
    private Collider m_tempCol;

    public ParticleSystem dustRun;

    // Start is called before the first frame update
    void Start()
    {
        GameObject calOb = GameObject.FindGameObjectWithTag("Calibration");

        Calibration c = null;
        if (calOb != null)
        {
             c = calOb.GetComponent<Calibration>();
        }

        MeshRenderer m = GetComponent<MeshRenderer>();
        if (c != null && m !=  null)
        {
            int nBlueID = c.GetBlueID();
            int nGreenID = c.GetGreenID();
            int nYellowID = c.GetYellowID();
            int nOrangeID = c.GetOrangeID();

            if (m_szColour == "Blue")
            {
                m_nPlayerID = nBlueID;
                m.material = Blackboard.GetInstance().GetBlueMat();
            }
            else if (m_szColour == "Green")
            {
                m_nPlayerID = nGreenID;
                m.material = Blackboard.GetInstance().GetGreenMat();
            }
            else if (m_szColour == "Yellow")
            {
                m_nPlayerID = nYellowID;
                m.material = Blackboard.GetInstance().GetYellowMat();
            }
            else if (m_szColour == "Orange")
            {
                m_nPlayerID = nOrangeID;
                m.material = Blackboard.GetInstance().GetOrangeMat();
            }
        }

        ParticleSystem.MainModule ma = GetComponent<ParticleSystem>().main;
        ma.startColor = GetComponent<Renderer>().material.color;

        m_Controller = GetComponent<Rigidbody>();
        m_fSpeed = m_fMaxSpeed;

        m_UiController = GameObject.Find("Canvas").GetComponent<UIController>();
        Debug.Assert(m_UiController != null, "Cannot find the UIController script on Canvas.");

        Respawn();
    }

    public void Stun()
    {
        if (!m_bStunned)
        {
            m_bStunned = true;
            m_fTimeWhenStunned = Time.time;
        }
    }

    public bool GetStunned()
    {
        return m_bStunned;
    }

    public void SetHasCoin(bool hasCoin)
    {
        m_bHasCoin = hasCoin;
    }

    private void Shove(ref RaycastHit hit)
    {
        if (!m_bHasCoin)
        {
            if (Physics.SphereCast(transform.position - (transform.forward * 2), m_fSphereCastRadius, transform.forward, out hit, m_fSphereCastDist))
            {
                Rigidbody hitController = hit.transform.GetComponent<Rigidbody>();
                PlayerController p = hit.transform.GetComponentInParent<PlayerController>();
                
                
                if (p != null)
                {
                    if (!p.GetStunned())
                    {
                        if (hitController.tag == "Player")
                        {
                            p.GetComponent<ParticleSystem>().Play();
                            hitController.AddForce(transform.forward * m_fShovePower, ForceMode.VelocityChange);
                            p.Stun();
                            p.DetachCoin();
                        }
                    }
                }
            }
        }
    }

    // Kill the player and start a respawn timer
    public void Kill()
    {
        // Can't deactivate the object itself because then this script stops running
        // Deactivating the mesh, collider and gravity will do what we need
        gameObject.GetComponent<MeshRenderer>().enabled = false;
        gameObject.GetComponent<CapsuleCollider>().enabled = false;
        gameObject.GetComponent<Rigidbody>().useGravity = false;

        m_fTimeWhenKilled = Time.time;
        m_bIsDead = true;

        // If the player is holding a coin when they die, the coin will be dropped
        if (m_bHasCoin) DetachCoin();
    }

    // Respawns the player at their chest
    private void Respawn()
    {
        gameObject.GetComponent<MeshRenderer>().enabled = true;
        gameObject.GetComponent<CapsuleCollider>().enabled = true;
        gameObject.GetComponent<Rigidbody>().useGravity = true;

        GameObject chest = Blackboard.GetInstance().GetChestWithID(m_nPlayerID);

        transform.position = chest.transform.position;
        transform.Translate(Vector3.up * m_fRespawnHeight, Space.World);
        transform.Translate(chest.transform.TransformDirection(0, 0, 1) * m_fRespawnOffset, Space.World);

        m_bIsDead = false;
    }

    private void DetachCoin()
    {
        StartCoroutine(DropHeldCoinCooldown());
        CoinController[] coins = GetComponentsInChildren<CoinController>();

        foreach (CoinController coin in coins)
        {
            coin.SetHeld(false);
            coin.transform.parent = null;
            coin.transform.Translate(new Vector3(0, -1, 0));

            Rigidbody rb = coin.GetComponent<Rigidbody>();
            rb.isKinematic = false;
            rb.velocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;

            Collider col = coin.GetComponent<Collider>();
            col.enabled = true;
        }

        SetHasCoin(false);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.tag == "Tentacle")
        {
            Animator a = collision.gameObject.GetComponentInParent<Animator>();
            if (!a.GetCurrentAnimatorStateInfo(0).IsName("Rise Up") && !a.GetCurrentAnimatorStateInfo(0).IsName("Exit Horizontal") && !a.GetCurrentAnimatorStateInfo(0).IsName("Exit Vertical"))
            {
                TentacleController tc = collision.gameObject.GetComponentInParent<TentacleController>();
                StateMachine stateMachine = null;
                if (tc != null)
                {
                    stateMachine = tc.GetStateMachine();
                    if (!a.GetCurrentAnimatorStateInfo(0).IsName("Lay Down Idle"))
                    {
                        if(stateMachine.GetState() != StateMachine.ESTATE.ATTACKING)
                        {
                           stateMachine.GetTentacleState().Attack();
                        }
                       
                    }
                }
              
                    
                   
                }
                else
                {

                }
                Kill();
            }
        }



    private void DropHeldCoin()
    {
        DetachCoin();
    }

    private IEnumerator DropHeldCoinCooldown()
    {
        bool b = true;
        while (b)
        {
            b = false;
            yield return new WaitForSeconds(m_fDropCoinCooldown);
        }
        m_bCanPickUpCoin = true;
    }

    void FixedUpdate()
    {
        m_Controller.AddForce(m_fGravityMultiplier * Physics.gravity);
    }

    // Update is called once per frame
    void Update()
    {
        if (!m_UiController.m_bGameEnded)
        {
            // Disable everything but the respawn timer while the player is dead
            if (!m_bIsDead)
            {
                Rigidbody rb = GetComponent<Rigidbody>();

                if(transform.position.y < -2)
                {
                    Kill();
                }

                if (rb.velocity.y < -0.1)
                {
                    m_bIsFalling = true;
                }
                else
                {
                    m_bIsFalling = false;
                }

                rb.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationZ | RigidbodyConstraints.FreezeRotationY;
                if (!(m_bStunned) && !(m_bIsFalling))
                {
                    //-------------------------------------------------------------------------------------------------------------------------------------------------------------
                    //Movement
                    //-------------------------------------------------------------------------------------------------------------------------------------------------------------
                    
                    //If your controller is plugged in
                    if (XCI.IsPluggedIn(m_nPlayerID))
                    {
                        //And if you are using the left or right stick
                        if (XCI.GetAxis(XboxAxis.LeftStickX, (XboxController)m_nPlayerID) != 0 || XCI.GetAxis(XboxAxis.LeftStickY, (XboxController)m_nPlayerID) != 0)
                        {
                            Vector3 v3InputDir = new Vector3(XCI.GetAxis(XboxAxis.LeftStickX, (XboxController)m_nPlayerID), 0, XCI.GetAxis(XboxAxis.LeftStickY, (XboxController)m_nPlayerID));
                            v3InputDir.Normalize();

                            m_Controller.transform.localRotation = Quaternion.LookRotation(v3InputDir, Vector3.up);
                            m_Controller.velocity = transform.forward * m_fSpeed;

                            dustRun.Play();
                        }
                        // Or if you're using the dpad
                        else if (XCI.GetDPad(XboxDPad.Left, (XboxController)m_nPlayerID) || XCI.GetDPad(XboxDPad.Right, (XboxController)m_nPlayerID) || XCI.GetDPad(XboxDPad.Up, (XboxController)m_nPlayerID) || XCI.GetDPad(XboxDPad.Down, (XboxController)m_nPlayerID))
                        {
                            Vector3 v3InputDir = Vector3.zero;
                            if (XCI.GetDPad(XboxDPad.Left, (XboxController)m_nPlayerID))
                            {
                                v3InputDir.x = -1;
                            }
                            else if (XCI.GetDPad(XboxDPad.Right, (XboxController)m_nPlayerID))
                            {
                                v3InputDir.x = 1;
                            }
                            else if (XCI.GetDPad(XboxDPad.Up, (XboxController)m_nPlayerID))
                            {
                                v3InputDir.z = 1;
                            }
                            else if (XCI.GetDPad(XboxDPad.Down, (XboxController)m_nPlayerID))
                            {
                                v3InputDir.z = -1;
                            }

                            v3InputDir.Normalize();

                            m_Controller.transform.localRotation = Quaternion.LookRotation(v3InputDir, Vector3.up);
                            m_Controller.velocity = transform.forward * m_fSpeed;

                            dustRun.Play();
                        }
                    }
                    else //else if controller not connected, use WASD instead
                    {
                        if (Input.GetAxis("Horizontal") != 0 || Input.GetAxis("Vertical") != 0)
                        {
                            Vector3 v3InputDir = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
                            v3InputDir.Normalize();

                            m_Controller.transform.localRotation = Quaternion.LookRotation(v3InputDir, Vector3.up);
                            m_Controller.velocity = transform.forward * m_fSpeed;

                            dustRun.Play();
                        }
                    }

                    //-------------------------------------------------------------------------------------------------------------------------------------------------------------
                    //Shoving and Dropping
                    //-------------------------------------------------------------------------------------------------------------------------------------------------------------

                    RaycastHit hit = new RaycastHit();

                    if (XCI.IsPluggedIn(m_nPlayerID))
                    {
                        //If button being pressed
                        if (XCI.GetButtonDown(XboxButton.B, (XboxController)m_nPlayerID) || XCI.GetButtonDown(XboxButton.A, (XboxController)m_nPlayerID))
                        {
                            Shove(ref hit);
                        }

                        if (XCI.GetButtonDown(XboxButton.X, (XboxController)m_nPlayerID) || XCI.GetButtonDown(XboxButton.Y, (XboxController)m_nPlayerID))
                        {
                            DropHeldCoin();
                        }
                    }
                    else
                    {
                        if (Input.GetKeyDown(KeyCode.Space))
                        {
                            Shove(ref hit);
                        }

                        if (Input.GetKeyDown(KeyCode.LeftShift))
                        {
                            DropHeldCoin();
                        }
                    }
                }
                else if(m_bStunned)//If stunned
                {
                    // End the stun
                    if (Time.time - m_fTimeWhenStunned > m_fStunTime)
                    {
                        m_bStunned = false;
                        m_fTimeWhenStunned = 0;
                        GetComponent<Rigidbody>().freezeRotation = false;

                        // Undo the IgnoreCollision call
                        if (m_tempCol != null && m_tempCol.gameObject.tag == "Coin")
                        {
                            Physics.IgnoreCollision(m_tempCol, gameObject.GetComponent<CapsuleCollider>(), false);
                        }
                    }
                }

                //-------------------------------------------------------------------------------------------------------------------------------------------------------------
                //Other
                //-------------------------------------------------------------------------------------------------------------------------------------------------------------

                // while holding coin, the player should be slowed
                if (m_bHasCoin)
                {
                    m_fSpeed = m_fCoinSpeed;
                }
                else
                {
                    m_fSpeed = m_fMaxSpeed;
                }
            }
            else if (Time.time - m_fTimeWhenKilled > m_fRespawnTime)
            {
                m_fTimeWhenKilled = 0;
                Respawn();
            }
        }
    }
}