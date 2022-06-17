using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Data_Unit : MonoBehaviour
{
    public float myValue=1;
    public int myPosition=0;
    float maxValue =1;
    float minValue=0;
    int totalValues=1;

    bool existent;
    Transform myTransform;
    GameObject mySelf;

    // Start is called before the first frame update
    void Start()
    {
        myTransform = transform;
        mySelf = gameObject;
        Display();
    }
    void Display()
    {
        myTransform.localScale = new Vector3(1f / totalValues, 1, (myValue + 1 - minValue) / (maxValue - minValue));
        myTransform.position = new Vector3(10f / totalValues * (myPosition + 0.5f) - 5f, (myValue + 1 - minValue) / (maxValue - minValue) * 5 - 5, 0);
    }
    public int GetPos()
    {
        return myPosition;
    }
    public float GetVal()
    {
        return myValue;
    }
    public void ChangePos(int newPos)
    {
        myPosition = newPos;
        Display();
    }
    public void SetExistence(float val, int pos, float max, float min, int total)
    {
        if (existent == false)
        {
            if (myTransform == null)
            { myTransform = transform; mySelf = gameObject; }
            myValue = val;
            myPosition = pos;
            maxValue = max;
            minValue = min;
            totalValues = total;
            existent = true;
            mySelf.GetComponent<MeshRenderer>().material.color = (Color.HSVToRGB((myValue-minValue)/(maxValue-minValue)*0.92f, 1, 1));
        }
    }




}
