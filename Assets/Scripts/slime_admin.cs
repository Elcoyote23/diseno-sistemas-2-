using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class slime_admin : MonoBehaviour
{

    public Transform objetivo;
    public float speed;

    public bool debePerseguir;
    public float distancia;
    // Start is called before the first frame update
    void Start()
    {
        objetivo = GameObject.Find("player").GetComponent<Transform>();
    }

    // Update is called once per frame
    void Update()
    {
        
        
            transform.position = Vector2.MoveTowards(transform.position, objetivo.position, speed * Time.deltaTime);
        


    }
}
