using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class parallaxMovement : MonoBehaviour
{
    // Start is called before the first frame update

    [SerializeField] public GameObject camera;

    public float factor = 0.3f;
    public float delta;
    void Start()
    {
        camera = GameObject.Find("Main Camera");
    }

    // Update is called once per frame
    void Update()
    {
        delta = FollowPlayer.instance.deltaX;
        if(delta == 0) return;
        else{
            transform.position = new Vector3((transform.position.x - delta*factor), transform.position.y, transform.position.z);
        }
        
    }
}
