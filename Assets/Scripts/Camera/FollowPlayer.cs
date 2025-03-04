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
    [SerializeField] public float CamUpBound;
    [SerializeField] public float CamLowBound;
    public float deltaX;

    public GameObject target;
    public Camera camera;

    public static FollowPlayer instance = null;


    private void Awake()
    {
        if (instance == null)
        { 
            instance = this;
        }
        
        else if (instance != this)
        {
            Destroy(gameObject);
        }
    }


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
            Mathf.Clamp(targetTRANS.position.y + yOffset, CamLowBound, CamUpBound),
            -10f

        );

        deltaX = newPos.x - transform.position.x;

        transform.position = Vector3.Slerp(transform.position, newPos, FollowSpeed * Time.deltaTime);    
        
    }
}

