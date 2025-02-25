using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class FollowPlayer : MonoBehaviour
{
    public float FollowSpeed = 2f;
    public float yOffset = 1f;
    public Transform targetTRANS;
    [SerializeField] public float CamLeftBound;
    [SerializeField] public float CamRightBound;

    public GameObject target;
    public Camera camera;


    
    // Start is called before the first frame update
    void Start()
    {
        camera = GetComponent<Camera>();
        target = GameObject.FindGameObjectWithTag("Player");
        targetTRANS = target.transform;
    }

    // Update is called once per frame
    void Update()
    {
        if(GameManager.instance.currentState != GameManager.GameState.Playing) return;
        

        Vector3 newPos = new Vector3(
            Mathf.Clamp(targetTRANS.position.x, CamLeftBound, CamRightBound),
            Mathf.Clamp(targetTRANS.position.y + yOffset, -2.5f, 999999999),
            -10f

        );

        

        transform.position = Vector3.Slerp(transform.position, newPos, FollowSpeed * Time.deltaTime);    
        
    }
}

