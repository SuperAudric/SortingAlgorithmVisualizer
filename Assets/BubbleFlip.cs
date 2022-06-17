using System.Collections;
using System.Collections.Generic;
//using System.Diagnostics;
using UnityEngine;

public class BubbleFlip : MonoBehaviour
{
    public GameObject ValueObj;
    public int totalValues =50;
    //bool flipping = false;
    public bool flipPass = false;
    public int comparisons=0;
    public int swaps = 0;
    public int randomSeed=0;
    public GameObject dataHead;

    int headPos = 0;
    int head2Pos = 0;
    int chainLength = 0;
    int antiChainLength = 0;
    int endBuffer = 1;
    Data_Unit[] myList;
    public int[] chainLengths;
    public int[] antiChainLengths;
    bool atStart = true;
    int beginingBuffer =0;

    // Start is called before the first frame update
    void Start()
    {
        antiChainLengths = new int[totalValues];
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
       // flipPass = true;
    }

    // Update is called once per frame
    void Update()
    {
        dataHead.GetComponent<DisplayReaderHead>().Display(headPos, totalValues);
        if (flipPass == true)
        { 
            comparisons++;
            if (myList[headPos].GetVal() > myList[headPos + 1].GetVal())
            {
                if (antiChainLength>0)
                {
                    //DeclareChain(headPos, antiChainLength);
                }
                chainLength++;
            }
            else
            {
                
                antiChainLength++;
                head2Pos = headPos;
                Flip(chainLength);
                if (chainLength > 0)
                {
                    DeclareChain(headPos, chainLength);
                }
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

            if (headPos < totalValues - endBuffer)
            {
                if (myList[headPos].GetVal() > myList[headPos + 1].GetVal())
                {
                    if (atStart)
                    {   
                        atStart = false;
                        beginingBuffer = Mathf.Max(0, chainLengths[0]-1);// this line shouldn't have the -1, but yeilds errors when it is removed. This loses about X comparisons each run
                    }
                    if (headPos > 1) 
                    { 
                        //DeclareChain(headPos - 1, chainLength);
                    }
                    chainLengths[headPos] = 0;
                    antiChainLengths[headPos] = 0;
                    Swap(headPos, headPos + 1);
                    chainLength = 0;
                }
                else
                {
                   
                    //DeclareChain (headPos,(headPos > 0 ? antiChainLengths[headPos - 1]+1 : 0));
                    //if (atStart) { beginingBuffer++; }
                    int chainAfter = chainLengths[headPos +1];
                    int chainBefore = (headPos>0?antiChainLengths[headPos - 1]:-1);
                    headPos += chainAfter;
                    DeclareChain(headPos,chainBefore+chainAfter+1);
                   // chainLengths[headPos+1] = 0;
                    chainLength++;
                }
                comparisons++;
                headPos++;

            }
            else
            {
                atStart=true;
                headPos = beginingBuffer;
                //chainLengths[0] = 0;
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
            Swap(head2Pos - length,head2Pos);
            head2Pos -= 1;
            Flip(length-2);
        }
    }
    void Swap(int pos1, int pos2)
    {
        swaps++;
        myList[pos2].ChangePos(pos1);
        myList[pos1].ChangePos(pos2);
        Data_Unit temp = myList[pos2];
        int temp2 = chainLengths[pos2];
        int temp3 = antiChainLengths[pos2];
        myList[pos2] = myList[pos1];
        chainLengths[pos2] = chainLengths[pos1];
        antiChainLengths[pos2] = antiChainLengths[pos1];
        chainLengths[pos1] = temp2;
        antiChainLengths[pos1] = temp3;
        myList[pos1] = temp;
    }
    void DeclareChain(int position,int length)
    {
        for (int i=0; i <= length; i++)
        {
            if (i <= position)
            {
                chainLengths[position - i] = i;
                antiChainLengths[position - i] = length - i;
            }
            else
                Debug.Log("wouldbeError");
        }
    }
    void TrimChain(int position)
    {
        bool finished = false;
        while (finished==false)
        {
            if (chainLengths[position]<2)
            {
                finished = true;
            }
            chainLengths[position] =Mathf.Max(chainLengths[position]-1,0) ;
            position--;
            
        }
    }
    void reshuffle(Data_Unit[] input)
    {
        // Knuth shuffle algorithm, courtesy of Wikipedia
        for (int t = 0; t < input.Length; t++)
        {
            Data_Unit tmp = input[t];
            int r = Random.Range(t, input.Length);
            input[t] = input[r];
            input[t].ChangePos(t);
            input[r] = tmp;
        }
    }
}