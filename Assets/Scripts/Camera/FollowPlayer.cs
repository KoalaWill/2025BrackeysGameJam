using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class FollowPlayer : MonoBehaviour
{
    public float FollowSpeed = 2f;
    public float yOffset = 1f;
    public Transform targetTRANS;
    //public Transform topTriggerTRANS;
    //public Collider2D endTriggerCOLL;
    public GameObject target;
    public Camera camera;


    
    // Start is called before the first frame update
    void Start()
    {
        camera = GetComponent<Camera>();
        //topTriggerTRANS = GameObject.FindGameObjectWithTag("TOP").GetComponent<Transform>();
        //endTriggerCOLL = GameObject.FindGameObjectWithTag("END").GetComponent<Collider2D>();
        target = GameObject.FindGameObjectWithTag("Player");
        targetTRANS = target.transform;
    }

    // Update is called once per frame
    void Update()
    {
        //Mathf.Min(targetTRANS.position.y, (topTriggerTRANS.position.y - camera.orthographicSize))
        Vector3 newPos = new Vector3(targetTRANS.position.x, targetTRANS.position.y + yOffset, -10f);
        transform.position = Vector3.Slerp(transform.position, newPos, FollowSpeed * Time.deltaTime);    
        
    }
}

