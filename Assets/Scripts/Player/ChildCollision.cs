using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Quaranteam
{
    public class ChildCollision : MonoBehaviour
    {
        Player player;

        private void Start()
        {
            PlayerReference();
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            PlayerReference();

            player.OnChildTriggerEnter(transform, other);
        }

        void PlayerReference()
        {
            if (player == null)
                player = GetComponentInParent<Player>();
        }
    }

}