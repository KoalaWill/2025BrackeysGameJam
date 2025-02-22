using UnityEngine;
using System.Collections;

using System.Collections.Generic;   //Allows us to use Lists. 
using UnityEngine.UI;   //Allows us to use UI.

public class GameManager : MonoBehaviour
{
        public static GameManager instance = null;  //Static instance of GameManager which allows it to be accessed by any other script.
        private int level = 1;  //Current level number, expressed in game as "Day 1".
        private bool doingSetup = true; //Boolean to check if we're setting up board, prevent Player from moving during setup.

    #region Gamestates


        public enum GameState
        {
            MainMenu,
            Playing,
            Paused,
            GameOver
        }
        public GameState currentState;

        public void ChangeState(GameState newState)
        {
            currentState = newState;

            switch (currentState)
            {
                case GameState.MainMenu:
                    // Initialize main menu
                    break;
                case GameState.Playing:
                    // Start gameplay
                    break;
                case GameState.Paused:
                    // Pause the game
                    break;
                case GameState.GameOver:
                    // Show game over screen
                    break;
            }
        }

    #endregion

//Awake is always called before any Start functions 
        void Awake()
        {
            //Check if instance already exists 
            if (instance == null) {
            //if not, set instance to this 
                instance = this;
            }
            //If instance already exists and it's not this: 
            else if (instance != this) {
            //Then destroy this. This enforces our singleton pattern, meaning there can only ever be one instance of a GameManager..
                Destroy(gameObject);
            }
            //Sets this to not be destroyed when reloading scene 
            DontDestroyOnLoad(gameObject);

            //Call the InitGame function to initialize the first level 
            InitGame();
                    doingSetup = false;
        }


    //This is called each time a scene is loaded. 
    void OnLevelWasLoaded(int index)
    {
        //Add one to our level number. 
        level++;
        //Call InitGame to initialize our level. 
        InitGame();
    }

    //Initializes the game for each level. 
    void InitGame()
    {
        ChangeState(GameState.Playing);
    }

    //Update is called every frame. 
    void Update()
    {
        // doingSetup are not currently true. 
        if (!doingSetup)
            //If any of these are true, return and do not start MoveEnemies. 
            return;
        if(Input.GetKeyDown(KeyCode.Escape)){
            if(currentState == GameState.Playing){
                ChangeState(GameState.Paused);
                Time.timeScale = 0f;
            }
            else if(currentState == GameState.Paused){
                ChangeState(GameState.Playing);
                Time.timeScale = 1f;
            }
            // Debug.Log($"Current State: {GameManager.instance.currentState}");
        }

    }


    //GameOver is called when the player dies
    public void GameOver()
    {
        //Disable this GameManager. 
        enabled = false;
    }

}

