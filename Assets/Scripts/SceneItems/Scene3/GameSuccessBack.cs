using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameSuccessBack : MonoBehaviour
{

    private SpriteRenderer spriteRenderer;
    private Color color;

    // Start is called before the first frame update
    void Start()
    {
        spriteRenderer = gameObject.GetComponent<SpriteRenderer>();
        color = spriteRenderer.color;
        // the alpha value 
        color.a = 0f;
        spriteRenderer.color = color;
    }

    // Update is called once per frame
    void Update()
    {
        if (GameManager.instance.currentState == GameManager.GameState.EndLevel && GameManager.instance.level == 3) {
            color.a = 0.5f;
            spriteRenderer.color = color;
        }
    }
}
