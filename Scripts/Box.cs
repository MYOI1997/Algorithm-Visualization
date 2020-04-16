using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Box : MonoBehaviour
{
    public Image TargetBox;
    public Text TargetText;

    public int Value;
    public int Location;

    void Start()
    {
        TargetBox = gameObject.GetComponent<Image>();
        TargetText = gameObject.transform.GetComponentInChildren<Text>();
    }

    public void Refresh(int NewVal = -1)
    {
        if(NewVal >= 0)
        {
            Value = NewVal;
        }

        SetSize(Value);
    }

    public void SetPosition()
    {
        TargetText.text = Value.ToString();

        TargetBox.rectTransform.sizeDelta = new Vector2(50, 50 * Value / 10);
        TargetBox.rectTransform.position = new Vector3(Location * 50, 0, 0);
    }

    public void Set(int Size, int i)
    {
        Value = Size;
        TargetText.text = Value.ToString();
        Location = i;

        TargetBox.rectTransform.sizeDelta = new Vector2(50, 50 * Size / 10);
        TargetBox.rectTransform.position = new Vector3(Location * 50, 0, 0);
    }

    public void SetSize(int Size)
    {
        Value = Size;
        TargetText.text = Value.ToString();

        TargetBox.rectTransform.sizeDelta = new Vector2(50, 50 * Value / 10);
    }
}
