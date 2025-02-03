using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System;

public class ARUI : MonoBehaviour
{
    public List<string> infoText = new List<string>();
    public List<Texture2D> imageList = new List<Texture2D>();

    public Canvas canvas;
    public TMP_Text infoBox;
    public RawImage rawImage;

    bool cardScaled = false;
    Vector3 cardScaleOg;

    int infoPointer = -1;

    void Start()
    {
        canvas.enabled = false;
    }

    void Update()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, 50))
        {
            if (hit.transform.tag == "Card")
            {
                if (Input.GetMouseButtonDown(0) && !canvas.enabled)
                {
                    // Do something
                    DisplayCanvas();
                    infoPointer = 0;
                    DisplayAndPlayInfo();

                    hit.transform.GetComponent<Animator>().SetBool("Interacted", true);
                } 
            }
        }
    }

    void DisplayAndPlayInfo()
    {
        if (infoPointer < infoText.Count)
        {
            infoBox.text = infoText[infoPointer];
            rawImage.texture = imageList[infoPointer];
        }  
        else
        {
            canvas.enabled = false;
            GameObject temp = GameObject.FindGameObjectWithTag("Card");
            temp.transform.GetComponent<Animator>().SetBool("Interacted", false);
        }  
    }

    public void NextInfo()
    {
        infoPointer++;
        DisplayAndPlayInfo();
    }

    public void DisplayCanvas()
    {
        canvas.enabled = true;
    }

    public void HideCanvas()
    {
        canvas.enabled = false;
    }
}
