using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Quaranteam
{
    public class ModifierTextUI : MonoBehaviour
    {
        [SerializeField]
        protected internal Text modifierDescription;

        [SerializeField]
        protected internal Text remainingTimeText;

        private ModifiersId boundId;

        private float remainingTime = 5f;

        private string description;

        private void Awake()
        {
        }

        public void BindToId(ModifiersId id)
        {
            boundId = id;
        }

        public void OnModifierRemoved(ModifiersId id)
        {
            if (id == boundId)
            {
                Destroy(gameObject);
            }
        }

        public void OnModifierUpdated(ModifiersId id, string newDescription, float newTimer)
        {
            if (id == boundId)
            {
                description = newDescription;
                remainingTime = newTimer;
                modifierDescription.text = description;
                remainingTimeText.text = remainingTime.ToString("0.0") + "s";
            }
           
        }

    }
}

