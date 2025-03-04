using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class tmpblack : MonoBehaviour
{
    // Start is called before the first frame update

    [SerializeField] GameObject tempBlack;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        tempBlack.SetActive(false);
    }
}
