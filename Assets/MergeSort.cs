using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MergeSort : MonoBehaviour
{
    int loops = 0;
    public GameObject ValueObj;
    public int totalValues = 50;
    public bool flipping = false;
    public bool flipPass = false;
    public int comparisons=0;
    public int swaps = 0;
    public int randomSeed=0;

    int headPos = 0;
    int head2Pos = 0;
    int chainLength = 0;
    int endBuffer;
    Data_Unit[] myList;

    bool sorted = false;

    // Start is called before the first frame update
    void Start()
    {
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

            if (sorted== false)
            {
                sorted = true;
                myList=MergeAndSort(myList);
                RecheckValues();
                print("Hello?");
            }
        }
    }


     public Data_Unit[] MergeAndSort(Data_Unit[] CurrentList)
    {
        if (loops < 1000)
        {
            //print("this... is going to happen a lot, isn't it?");
            Data_Unit[] left;
            Data_Unit[] right;
            Data_Unit[] result = new Data_Unit[CurrentList.Length];
            //As this is a recursive algorithm, we need to have a base case to 
            //avoid an infinite recursion and therfore a stackoverflow
            if (CurrentList.Length <= 1)
                return CurrentList;
            // The exact midpoint of our array  
            int midPoint = CurrentList.Length / 2;
            //Will represent our 'left' array
            left = new Data_Unit[midPoint];

            //if array has an even number of elements, the left and right array will have the same number of 
            //elements
            if (CurrentList.Length % 2 == 0)
                right = new Data_Unit[midPoint];
            //if array has an odd number of elements, the right array will have one more element than left
            else
                right = new Data_Unit[midPoint + 1];
            //populate left CurrentList
            for (int i = 0; i < midPoint; i++)
                left[i] = CurrentList[i];
            //populate right array   
            int x = 0;
            //We start our index from the midpoint, as we have already populated the left array from 0 to midpoint

            for (int i = midPoint; i < CurrentList.Length; i++)
            {
                right[x] = CurrentList[i];
                x++;
            }
            //Recursively sort the left array
            left = MergeAndSort(left);
            //Recursively sort the right array
            right = MergeAndSort(right);
            //Merge our two sorted arrays
            result = merge(left, right);
            return result;
        }
        else
        {
            print("recursion safeguard activated.");
            return CurrentList;
        }
    }
    //This method will be responsible for combining our two sorted arrays into one giant array
    public Data_Unit[] merge(Data_Unit[] left, Data_Unit[] right)
    {
        int resultLength = right.Length + left.Length;
        Data_Unit[] result = new Data_Unit[resultLength];
        //
        int indexLeft = 0, indexRight = 0, indexResult = 0;
        //while either array still has an element
        while (indexLeft < left.Length || indexRight < right.Length)
        {
            //if both arrays have elements  
            if (indexLeft < left.Length && indexRight < right.Length)
            {
                //If item on left array is less than item on right array, add that item to the result array 
                comparisons++;
                if (left[indexLeft].GetVal() <= right[indexRight].GetVal())
                {
                    result[indexResult] = left[indexLeft];
                    indexLeft++;
                    indexResult++;
                }
                // else the item in the right array wll be added to the results array
                else
                {
                    result[indexResult] = right[indexRight];
                    indexRight++;
                    indexResult++;
                }
            }
            //if only the left array still has elements, add all its items to the results array
            else if (indexLeft < left.Length)
            {
                result[indexResult] = left[indexLeft];
                indexLeft++;
                indexResult++;
            }
            //if only the right array still has elements, add all its items to the results array
            else if (indexRight < right.Length)
            {
                result[indexResult] = right[indexRight];
                indexRight++;
                indexResult++;
            }
        }
        return result;
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



    public void RecheckValues()
    {
        for (int i = 0; i < myList.Length; i++)
        {
            myList[i].ChangePos(i);
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
