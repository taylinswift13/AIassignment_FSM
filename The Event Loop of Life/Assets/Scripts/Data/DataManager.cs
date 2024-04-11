using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DataManager : MonoBehaviour
{
    public List<Slider> sliders = new List<Slider>();
    public List<Text> text = new List<Text>();
    public DataHolder data;
    private void Update()
    {
        for (int i = 0; i < 3; i++)
        { text[i].text = sliders[i].value.ToString(); }
        DataSender();
    }
    public void DataSender()
    {
        data.grass = (int)sliders[0].value;
        data.sheep = (int)sliders[1].value;
        data.wolf = (int)sliders[2].value;
    }
}
