using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyRPG : MonoBehaviour
{
    public int health = 3;

    Color c = Color.red;
    public void Damage()
    {
        health -= 1;
        //Blink();
    }
    void Blink()
    {
        c = new Color(c.r - health*0.1f, c.g, c.b, c.a);
        GetComponent<SpriteRenderer>().color = c;
    }
    void Update()
    {
        
        if(health < 0)
        {
            Destroy(this.gameObject);
        }
    }
}
