using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bar : MonoBehaviour
{
    public string playerNumber = "1";
    public float initial = 299f;
    public float max = 300f;
    public float value = -5f;
    private RectTransform bar;
    private float change;
    void Start()
    {
        bar = GameObject.Find("Bar" + playerNumber).GetComponent<RectTransform>();
        bar.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, initial);
    }
    
    public bool isFull()
    {
        if (bar.rect.width >= max)
            return true;
        return false;
    }

    public void setFull()
    {
        SetBar(max);
    }

    void Update()
    {

    }

    void FixedUpdate()
    {
      AddBar(value * Time.deltaTime);
    }

    public void AddBar(float value) {
        change = bar.rect.width + value;
        bar.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, Mathf.Clamp(change, -1f, max));
    }

    public void SetBar(float change) {
        bar.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, Mathf.Clamp(change, -1f, max));
    }

    public float GetBar() {
        return bar.rect.width;
    }
}
