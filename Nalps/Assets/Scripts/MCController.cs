using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MCController : MonoBehaviour
{
    private float speed = 3.0f;
    Vector3 position;
    public bool inGrass = false;
    private bool checkEncounter = true;
    //Animator animator;
    //Vector2 lookDirection = new Vector2(1, 0);

    
    // Start is called before the first frame update
    void Start()
    {
        //animator = GetComponent<Animator>();
        position = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");

        if (transform.position == position & inGrass & checkEncounter)
        {
            checkEncounter = false;
            float encounter = Random.value;
            print(encounter);
            if (encounter > 0.8f)
            {
                Debug.Log("Fight!");
            }
        }

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
    }

}
