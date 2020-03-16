using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CreditsButton : MonoBehaviour
{
    [SerializeField] GameObject credits;

    private void Start()
    {
        GetComponent<Button>().onClick.AddListener(ShowHideCredits);
    }

    public void ShowHideCredits()
    {
        credits.SetActive(!credits.activeSelf);
    }
}
