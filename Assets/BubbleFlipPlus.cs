using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BubbleFlipPlus : MonoBehaviour
{
    public GameObject ValueObj;
    public int totalValues = 50;
    bool flipping = false;
    public bool flipPass = false;
    public int comparisons=0;
    public int swaps = 0;
    public int randomSeed=0;

    int headPos = 0;
    int head2Pos = 0;
    int chainLength = 0;
    int endBuffer;
    Data_Unit[] myList;
    int[] chainLengths;

    bool atStart = true;
    int beginingBuffer =0;

    // Start is called before the first frame update
    void Start()
    {
        chainLengths = new int[totalValues];
        Random.InitState(randomSeed);
        myList = new Data_Unit[totalValues];
        for (int i=0; i<totalValues; i++)
        {
            myList[i] = Instantiate(ValueObj).GetComponent<Data_Unit>();
            myList[i].SetExistence(totalValues - i - 1,i, totalValues, 0, totalValues);
        }
        reshuffle(myList);
        //FlipPass();
        //flipPass = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (flipPass == true)
        { 
            comparisons++;
            if (myList[headPos].GetVal() > myList[headPos + 1].GetVal())
            {
                chainLength++;
            }
            else
            {
                head2Pos = headPos;
                Flip(chainLength);
                chainLength = 0;
            }
            chainLengths[headPos] = chainLength;
            headPos++;



            if (headPos >= totalValues - 1)
            {
                flipPass = false;
                head2Pos = totalValues - 1;
                Flip(chainLength);
                chainLength = 0;
                headPos = 0;
            }
        }
        else
        {

            if (headPos < totalValues - 1 - endBuffer)
            {
                if (myList[headPos].GetVal() > myList[headPos + 1].GetVal())
                {
                    if (atStart)
                    {   
                        atStart = false; beginingBuffer = Mathf.Max(0, beginingBuffer - 1);
                     }
                    swaps++;
                    myList[headPos + 1].ChangePos(headPos);
                    myList[headPos].ChangePos(headPos + 1);
                    Data_Unit temp = myList[headPos];
                    myList[headPos] = myList[headPos + 1];
                    myList[headPos + 1] = temp;
                    chainLength = 0;
                }
                else
                {
                    if (atStart) { beginingBuffer++; }
                    chainLength++;
                }
                comparisons++;
                headPos++;

            }
            else
            {
                atStart=true;
                headPos = beginingBuffer;
                endBuffer+=(chainLength+1);
                chainLength =0;
            }
        }
    }
    void FlipPass()
    {
        
        int chainLength = 0;
        for (int i=0; i< totalValues - 1; i++)
        {
            if (myList[i].GetVal() > myList[i + 1].GetVal())
            {
                chainLength++;
            }
            else
            {
                head2Pos = i;
                Flip(chainLength);
                chainLength = 0;
            }
            

        }
        head2Pos = totalValues - 1;
        Flip(chainLength);
    }
    void Flip(int length)
    {
        if (length > 0)
        {
            swaps++;
            myList[head2Pos].ChangePos(head2Pos - length);
            myList[head2Pos - length].ChangePos(head2Pos);
            Data_Unit temp = myList[head2Pos];
            myList[head2Pos] = myList[head2Pos - length];
            myList[head2Pos - length] = temp;
            head2Pos -= 1;
            Flip(length-2);
        }
    }

    void reshuffle(Data_Unit[] texts)
    {
        // Knuth shuffle algorithm, courtesy of Wikipedia
        for (int t = 0; t < texts.Length; t++)
        {
            Data_Unit tmp = texts[t];
            int r = Random.Range(t, texts.Length);
            texts[t] = texts[r];
            texts[t].ChangePos(t);
            texts[r] = tmp;
        }
    }
}
