using NUnit.Framework;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class WCAG_1_4_1_UseOfColorTester : MonoBehaviour
{
    [SerializeField]
    List<ColorEntry> colors = new()
    {
        new ColorEntry() { name = "red", color = Color.red },
        new ColorEntry() { name = "green", color = Color.green },
        new ColorEntry() { name = "blue", color = Color.blue }
    };

    [SerializeField] Canvas canvas;
    [SerializeField] GameObject buttonPrefab;




    private void Start()
    {
        foreach(var c in colors)
        {
            var go = GameObject.Instantiate(buttonPrefab, canvas.transform);
            go.GetComponentInChildren<Image>().color = c.color;
            go.GetComponentInChildren<TMP_Text>().text = c.name;
            go.SetActive(true);
        }
    }







    [System.Serializable]
    public class ColorEntry
    {
        public string name;
        public Color color;
    }
}
