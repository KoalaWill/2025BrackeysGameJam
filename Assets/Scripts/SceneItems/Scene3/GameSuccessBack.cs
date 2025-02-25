using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameSuccessBack : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if(GameManager.instance.currentState == GameManager.GameState.EndLevel && GameManager.instance.level == 3) gameObject.SetActive(false);
    }
}
