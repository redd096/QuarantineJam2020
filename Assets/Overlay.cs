using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine;

public class Overlay : MonoBehaviour
{

    public GameObject restart_Text;
    public GameObject restart_Button;
    // Start is called before the first frame update
    void OnEnable()
    {
#if UNITY_ANDROID
        restart_Button.SetActive(true);
        restart_Text.SetActive(false);
#else
        restart_Button.SetActive(false);
        restart_Text.SetActive(true);
#endif

    }

}
