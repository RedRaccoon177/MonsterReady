using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestHam : MonoBehaviour
{
    [SerializeField] GameObject meatbox;
    GameObject[] meatBoxArr;
    [SerializeField] int currentMeatBox;

    private void Start()
    {
        meatBoxArr = new GameObject[currentMeatBox];

        for (int i=0; i < currentMeatBox; i++)
        {
            var temp = Instantiate(meatbox, transform);
            temp.transform.localPosition = new Vector3(0,i*0.5f,0);
        }
    }
    public int MinusMeatBox(int meatBox)
    {
        currentMeatBox -= meatBox;
        return currentMeatBox;
    }
    public void AddMeatBox(int meatBox)
    {
        currentMeatBox += meatBox;
    }
}
