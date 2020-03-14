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

        LevelTimer levelTimer;

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

            StartCoroutine(WaitForEnterButton());
        }

        private IEnumerator WaitForEnterButton()
        {
            if (Input.GetKeyDown(KeyCode.Return))
            {
                levelTimer.gameObject.SetActive(true);
            }

            yield return null;
        }
    }
}

