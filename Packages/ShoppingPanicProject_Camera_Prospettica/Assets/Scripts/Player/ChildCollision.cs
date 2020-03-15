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
            player = GetComponentInParent<Player>();
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            player.OnChildTriggerEnter(transform, other);
        }
    }

}