using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Quaranteam
{
    [RequireComponent(typeof(Slider))]
    public class GameManager : MonoBehaviour
    {
        /// <summary>
        /// Fill the rules of this level.
        /// </summary>
        public GameRules appliedGameRules;

        /// <summary>
        /// Identifies the current game state: used as index in the game state function array.
        /// </summary>
        private int currentGameState = 0;

        /// <summary>
        /// The game time when the start button is pressed.
        /// </summary>
        private float gamePreparationStartTime = 0f;
        private float gameStartTime = 0f;

        /// <summary>
        /// The update function for each state.
        /// </summary>
        private List<Action> updateFunctions;

        private void Awake()
        {

            //updateFunctions.Add(WaitForGameStart);
            //updateFunctions.Add(ManagePreparationTime);
            //updateFunctions.Add(GameUpdate);

            Debug.Log("Press Enter to start the game");
        }

        //public void Update()
        //{
        //    updateFunctions[currentGameState]();
        //}

        //private void WaitForGameStart()
        //{
        //    // Wait for start button to be pressed.
        //    if (Input.GetKeyDown(KeyCode.Return))
        //    {
        //        Debug.Log("Entered preparation time");
        //        currentGameState += 1;
        //        gamePreparationStartTime = Time.time;
        //    }
        //}

        //private void ManagePreparationTime()
        //{
        //    if (Time.time - gamePreparationStartTime >= appliedGameRules.PreparationTime)
        //    {
        //        Debug.Log("Entered game time");
        //        gameStartTime = Time.time;
        //        currentGameState += 1;
        //    }
        //}

        //private void GameUpdate()
        //{
        //    float gameTime = Time.time - gameStartTime;

        //    if (gameTime >= appliedGameRules.GameTime)
        //    {
        //        Debug.Log("Game end");
        //        currentGameState += 1;
        //    }
        //}
    }
}

