using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Quaranteam
{
    public class ModifiersPanelUI : MonoBehaviour
    {
        [SerializeField]
        private GameObject modifierUiPrefab;

        private Dictionary<ModifiersId, ModifierTextUI> modifierDescriptors =
            new Dictionary<ModifiersId, ModifierTextUI>();

        private GameManager gameManager;

        private void Start()
        {
            gameManager = FindObjectOfType<GameManager>();
            gameManager.onModifierRemoved += OnModifierRemoved;
        }

        public void AddModifier(ModifierRule modifier)
        {
            Debug.Log("AddModifier");
            if (!modifierDescriptors.ContainsKey(modifier.Id))
            {
                ModifierTextUI newDescriptor = 
                    Instantiate(modifierUiPrefab, transform).GetComponent<ModifierTextUI>();
                Debug.Log("Instantiated new modifier");
                modifierDescriptors[modifier.Id] = newDescriptor;
                gameManager.onModifierUpdated += newDescriptor.OnModifierUpdated;
                gameManager.onModifierRemoved += newDescriptor.OnModifierRemoved;
                newDescriptor.BindToId(modifier.Id);
            }

            modifierDescriptors[modifier.Id].modifierDescription.text = modifier.Description;
            modifierDescriptors[modifier.Id].remainingTimeText.text = ((int)modifier.Duration).ToString() + "s";
        }

        public void OnModifierRemoved(ModifiersId id)
        {
            ModifierTextUI descriptor = modifierDescriptors[id];
            gameManager.onModifierRemoved -= descriptor.OnModifierRemoved;
            gameManager.onModifierUpdated -= descriptor.OnModifierUpdated;
            modifierDescriptors.Remove(id);
        }
    }
}

