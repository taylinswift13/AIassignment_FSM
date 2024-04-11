using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DataHolder : MonoBehaviour
{
    public int grass = 0;
    public int sheep = 0;
    public int wolf = 0;
    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }
    private void Start()
    {
        var dataHolders = GameObject.FindGameObjectsWithTag("Data");
        if (dataHolders.Length > 1)
        {
            Destroy(dataHolders[0]);
        }
    }

}
