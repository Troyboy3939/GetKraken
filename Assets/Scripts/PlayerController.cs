﻿using System.Collections;
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

    [SerializeField] GameObject m_Grid;
    public bool m_bIsFalling = false;
    
    [HideInInspector] public bool m_bHasCoin;
    [HideInInspector] public bool m_bIsDead = false;

    private bool m_bStunned = false;

    Rigidbody m_Controller;
    private UIController m_UiController;

    // This will be used to store colliders that need to be accessed from multiple methods
    private Collider m_tempCol;

    // Start is called before the first frame update
    void Start()
    {
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
                        hitController.AddForce(transform.forward * m_fShovePower, ForceMode.VelocityChange);
                        p.Stun();
                        p.SetHasCoin(false);
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

        // If the player is holding a coin when they die, the coin will be pushed away
        if (m_bHasCoin)
        {
            Rigidbody coinRb = gameObject.GetComponentInChildren<Rigidbody>();
            DetachCoin();
        }
    }

    // Respawns the player at their chest
    private void Respawn()
    {
        gameObject.GetComponent<MeshRenderer>().enabled = true;
        gameObject.GetComponent<CapsuleCollider>().enabled = true;
        gameObject.GetComponent<Rigidbody>().useGravity = true;

        GameObject chest = Blackboard.GetInstance().GetChestWithID(m_nPlayerID);

        FloorGrid grid = m_Grid.GetComponent<FloorGrid>();
        grid.DropObjectAtNode(grid.GetNodeByPosition(chest.transform.position), transform);

        m_bIsDead = false;
    }

    private void DetachCoin()
    {
        CoinController[] coins = GetComponentsInChildren<CoinController>();
        Transform[] ChildrenTransforms = GetComponentsInChildren<Transform>();

        int nChildCount = transform.childCount;
        transform.DetachChildren();

        foreach (CoinController coin in coins)
        {
            coin.m_bHeld = false;
            coin.transform.Translate(new Vector3(0, -1, 0));

            Rigidbody rb = coin.GetComponent<Rigidbody>();
            rb.isKinematic = false;
            rb.velocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;

            BoxCollider bc = rb.gameObject.GetComponent<BoxCollider>();
            Physics.IgnoreCollision(bc, gameObject.GetComponent<CapsuleCollider>(), false);
        }

        for (int i = 0; i < nChildCount + 1; i++)
        {
            if (ChildrenTransforms[i].tag == "Nose")
            {
                ChildrenTransforms[i].SetParent(transform);
            }
            else if (ChildrenTransforms[i].tag == "Coin")
            {
                CoinController CC = ChildrenTransforms[i].GetComponentInParent<CoinController>();
                m_tempCol = CC.GetComponentInParent<BoxCollider>();
                m_tempCol.enabled = true;
                CC.SetHeld(false);
            }
        }

        m_bHasCoin = false;
    }

    // TODO: This is currently not working
    //private void DropCoin()
    //{
    //    Collider thisCol = GetComponent<Collider>();
    //    Collider otherCol = GetComponentInParent<Collider>();

    //    // Temporarily disable collision between the coin and the player that touched it
    //    Physics.IgnoreCollision(thisCol, otherCol);
    //    StartCoroutine(DropCoinCooldown(thisCol, otherCol));

    //    DetachCoin();
    //}

    //private IEnumerator DropCoinCooldown(Collider col1, Collider col2)
    //{
    //    bool b = true;
    //    while (b)
    //    {
    //        b = false;
    //        yield return new WaitForSeconds(2);
    //    }
    //    Physics.IgnoreCollision(col1, col2, false);
    //}

    // Update is called once per frame
    void Update()
    {
        m_Controller.AddForce(m_fGravityMultiplier * Physics.gravity);

        if (!m_UiController.m_bGameEnded)
        {
            // Disable everything but the respawn timer while the player is dead
            if (!m_bIsDead)
            {
                Rigidbody rb = GetComponent<Rigidbody>();

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
                        }
                    }

                    //-------------------------------------------------------------------------------------------------------------------------------------------------------------
                    //Shoving and Dropping
                    //-------------------------------------------------------------------------------------------------------------------------------------------------------------

                    RaycastHit hit = new RaycastHit();

                    if (XCI.IsPluggedIn(m_nPlayerID))
                    {
                        //If button being pressed
                        if (XCI.GetButtonDown(XboxButton.B, (XboxController)m_nPlayerID))
                        {
                            Shove(ref hit);
                        }

                        // Currently not working
                        //if (XCI.GetButtonDown(XboxButton.X, (XboxController)m_nPlayerID))
                        //{
                        //    DropCoin();
                        //}
                    }
                    else
                    {
                        if (Input.GetKeyDown(KeyCode.Space))
                        {
                            Shove(ref hit);
                        }

                        // Currently not working
                        //if (Input.GetKeyDown(KeyCode.LeftShift))
                        //{
                        //    DropCoin();
                        //}
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

                // when getting shoved
                if ((!m_bHasCoin) && (transform.childCount != 0))
                {
                    DetachCoin();
                }

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