using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Quaranteam
{
    public class ChildCollision : MonoBehaviour
    {
        Player player;

        public bool risky;

        private void Start()
        {
            GetPlayerReference();
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            GetPlayerReference();

            player.OnChildTriggerEnter(this, other);
        }

        void GetPlayerReference()
        {
            if (player == null)
                player = GetComponentInParent<Player>();
        }
    }

}