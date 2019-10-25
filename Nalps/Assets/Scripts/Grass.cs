using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grass : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        MCController mc = collision.gameObject.GetComponent<MCController>();
        if (mc != null)
        {
            mc.inGrass = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        MCController mc = collision.gameObject.GetComponent<MCController>();
        if (mc != null)
        {
            mc.inGrass = false;
        }
    }
}
