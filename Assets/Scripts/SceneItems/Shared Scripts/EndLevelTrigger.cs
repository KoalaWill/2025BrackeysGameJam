using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndLevelTrigger : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] public BoxCollider2D _col;
    bool activated = false;
    void Start()
    {
        _col = gameObject.GetComponent<BoxCollider2D>();
        activated = false;
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log($"{collision.gameObject.name}");
        if (collision.gameObject.name == "Player Controller" || activated==false)
        {
            activated = true;
            GameManager.instance.ChangeState(GameManager.GameState.EndLevel);
        }
    }
}
