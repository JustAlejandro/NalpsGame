﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MCController : MonoBehaviour
{
    private float speed = 3.0f;
    Vector3 position;
    public bool inGrass = false;
    private bool checkEncounter = true;
    //Animator animator;
    Vector2 lookDirection = new Vector2(1, 0);

    
    // Start is called before the first frame update
    void Start()
    {
        //animator = GetComponent<Animator>();
        transform.position = GameObject.FindGameObjectsWithTag("PlayerData")[0].GetComponent<PlayerScript>().position;
        position = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");
        lookDirection.Set(horizontal, vertical);
        lookDirection.Normalize();

        // This if statement trigger random encounters when the player is inside grass
        if (transform.position == position & inGrass & checkEncounter)
        {
            checkEncounter = false;
            float encounter = Random.value;
            print(encounter);
            if (encounter > 0.8f)
            {
                Debug.Log("Fight!");
                GameObject.FindGameObjectsWithTag("PlayerData")[0].GetComponent<PlayerScript>().position = position;
                SceneManager.LoadScene("BattleScene");
            }
        }

        // The following code moves the MC if a collision is not detected by the raycast
        if (horizontal != 0.0f && transform.position == position)
        {
            RaycastHit2D hit = Physics2D.Raycast(transform.position, new Vector2(horizontal, 0),1.0f, LayerMask.GetMask("Static Objects"));
            if (!hit)
            {
                Vector3 move = new Vector3(horizontal, 0, 0);
                move.Normalize();
                position += move;
                checkEncounter = true;
            }
        }
        else if (vertical != 0.0f && transform.position == position)
        {
            RaycastHit2D hit = Physics2D.Raycast(transform.position, new Vector2(0, vertical),1.0f, LayerMask.GetMask("Static Objects"));
            if (!hit)
            {
                Vector3 move = new Vector3(0, vertical, 0);
                move.Normalize();
                position += move;
                checkEncounter = true;
            }  
        }
        transform.position = Vector3.MoveTowards(transform.position, position, Time.deltaTime * speed);

        // The following is used to pick up items
        if (Input.GetKeyDown(KeyCode.Z))
        {
            RaycastHit2D findItem = Physics2D.Raycast(transform.position, lookDirection, 1.0f);
            if (findItem.collider.GetComponent<Item>() != null)
            {
                Debug.Log("Found Item");
            }
        }
    }

}
