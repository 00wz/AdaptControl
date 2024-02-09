using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameRoot : MonoBehaviour
{
    [SerializeField]
    public Canvas Canvas;

    static private GameRoot _instance;
    static public GameRoot Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = UnityEngine.Object.FindObjectOfType<GameRoot>();
            }
            return _instance;
        }
    }
}
