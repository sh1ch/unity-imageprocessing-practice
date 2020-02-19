using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Button : MonoBehaviour
{
    public Image Image1;
    public Image Image2;
    public Image Image3;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Click()
    {
        Debug.Log("Click Called.");

        Image1.GetComponent<DefeatEffect>().IsEnabled = true;
        Image2.GetComponent<DefeatEffect>().IsEnabled = true;
        Image3.GetComponent<DefeatEffect>().IsEnabled = true;
    }
}
