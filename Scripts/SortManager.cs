using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public enum SortType
{
    None = 0,

    Bubble,
    Selection,
    Insertion,
    Marge,
    Quick,
    RandomQuick,
    Counting,
    Radix,
}

public class SortManager : MonoBehaviour
{
    public Box TargetBox;
    public List<Box> BoxArray;
    public GameObject ListParent;

    public int DefaultCount = 15;
    public int Count;
    public bool Result = true;

    int MaxValue = 0;

    public void SwapValue(Box A, Box B)
    {
        int Temp = A.Value;
        A.Value = B.Value;
        B.Value = Temp;

        A.Refresh();
        B.Refresh();
    }

    public void ShuffleStart()
    {
        StartCoroutine(Shuffle());
    }

    IEnumerator Shuffle()
    {
        if(Result)
        {
            Result = false;

            for (int i = BoxArray.Count - 1; i > 0; i--)
            {
                int j = Random.Range(0, BoxArray.Count - 1);

                SwapValue(BoxArray[i], BoxArray[j]);

                yield return new WaitForSeconds(0.05f);
            }

            Result = true;

            Debug.Log("셔플 종료");
        }
    }

    public void StartSort(int Value)
    {
        switch (Value)
        {
            case 1: StartCoroutine(CoBubbleSort()); break;
            case 2: StartCoroutine(CoSelectionSort()); break;
            case 3: StartCoroutine(CoInsertionSort()); break;
            case 4: StartCoroutine(CoMargeSort(0, Count - 1)); break;
            case 5: StartCoroutine(CoQuickSort(0, Count - 1)); break;
            case 6: RandomQuickSort(0, Count - 1); break;
            case 7: StartCoroutine(CoCountingSort(0, Count - 1)); break;
            case 8: StartCoroutine(CoRadixSort(MaxValue, Count - 1)); break;
        }
    } 

    private IEnumerator CoBubbleSort()
    {
        if(Result)
        {
            Result = false;

            int Size = BoxArray.Count;

            bool Check = false;

            do
            {
                Check = false;

                for (int i = 0; i < Size - 1; i++)
                {
                    BoxArray[i].TargetBox.color = Color.red;

                    if (BoxArray[i].Value > BoxArray[i + 1].Value)
                    {
                        BoxArray[i+1].TargetBox.color = Color.blue;

                        SwapValue(BoxArray[i], BoxArray[i + 1]);
                        Check = true;

                        yield return new WaitForSeconds(0.2f);
                    }

                    BoxArray[i].TargetBox.color = Color.white;
                    BoxArray[i + 1].TargetBox.color = Color.white;
                }
            } while (Check);

            Result = true;

            Debug.Log("버블 정렬 끝");
        }
    }

    private IEnumerator CoSelectionSort()
    {
        if(Result)
        {
            Result = false;

            int Least = 0;

            for (int i = 0; i < BoxArray.Count - 1; i++)
            {
                Least = i;
                BoxArray[Least].TargetBox.color = Color.red;

                for (int j = i + 1; j < BoxArray.Count; j++)
                {
                    if (BoxArray[j].Value < BoxArray[Least].Value)
                    {
                        Least = j;
                    }
                }

                if (i != Least)
                {
                    SwapValue(BoxArray[i], BoxArray[Least]);

                    yield return new WaitForSeconds(0.2f);
                }

                BoxArray[i].TargetBox.color = Color.white;
            }

            Result = true;

            Debug.Log("선택 정렬 끝");
        }  
    }

    private IEnumerator CoInsertionSort()
    {
        int j = 0;

        if (Result)
        {
            Result = false;

            for (int i = 1; i < BoxArray.Count; i++)
            {
                int InsertionTarget = BoxArray[i].Value;
                BoxArray[i].TargetBox.color = Color.red;

                for(j = i - 1; j >= 0 && BoxArray[j].Value > InsertionTarget; j--)
                {
                    SwapValue(BoxArray[j + 1], BoxArray[j]);

                    yield return new WaitForSeconds(0.2f);
                }

                BoxArray[i].TargetBox.color = Color.white;
                BoxArray[j + 1].Value = InsertionTarget;
            }

            Result = true;

            Debug.Log("삽입 정렬 끝");
        }
    }

    private IEnumerator CoMargeSort(int Begin, int End)
    {
        if(Result)
        {
            Result = false;

            if (Begin < End)
            {
                int LeftPivot = (Begin + End) / 2;
                int RightPivot = LeftPivot + 1;

                if (Begin != LeftPivot)
                {
                    CoMargeSort(Begin, LeftPivot);
                    CoMargeSort(RightPivot, End);
                }

                List<Box> TempArray = new List<Box>();

                int FirstDivision = Begin;
                int SecondDivision = RightPivot;
                int i = 0;

                while (FirstDivision <= LeftPivot && SecondDivision <= End)
                {
                    if (BoxArray[FirstDivision].Value <= BoxArray[SecondDivision].Value)
                    {
                        TempArray.Add(BoxArray[FirstDivision++]);
                    }
                    else
                    {
                        TempArray.Add(BoxArray[SecondDivision++]);
                    }
                }

                if (FirstDivision > LeftPivot)
                {
                    while (SecondDivision <= End)
                    {
                        TempArray.Add(BoxArray[SecondDivision++]);
                    }
                }
                else
                {
                    while (FirstDivision <= LeftPivot)
                    {
                        TempArray.Add(BoxArray[FirstDivision++]);
                    }
                }

                for (i = Begin; i <= End; i++)
                {
                    BoxArray[i] = TempArray[i - Begin];
                    BoxArray[i].SetPosition();

                    yield return new WaitForSeconds(0.2f);
                }
            }

            Result = true;

            Debug.Log("병합 정렬 끝");
        }
    }

    private IEnumerator CoQuickSort(int Left, int Right)
    {
        int Pivot = Left;
        int LeftPivot = Left;
        int RightPivot = Right;

        Debug.Log(LeftPivot);
        Debug.Log(RightPivot);

        while (LeftPivot < RightPivot)
        {
            while (BoxArray[LeftPivot].Value <= BoxArray[Pivot].Value && LeftPivot < Right)
            {
                BoxArray[LeftPivot].TargetBox.color = Color.white;

                LeftPivot++;

                BoxArray[LeftPivot].TargetBox.color = Color.red;
            }

            while (BoxArray[RightPivot].Value >= BoxArray[Pivot].Value && RightPivot > Left)
            {
                BoxArray[RightPivot].TargetBox.color = Color.white;

                RightPivot--;

                BoxArray[RightPivot].TargetBox.color = Color.red;
            }

            if (LeftPivot < RightPivot)
            {
                SwapValue(BoxArray[LeftPivot], BoxArray[RightPivot]);

                yield return new WaitForSeconds(0.2f);

                continue;
            }

            SwapValue(BoxArray[Pivot], BoxArray[RightPivot]);

            yield return new WaitForSeconds(0.2f);

            StartCoroutine(CoQuickSort(Left, RightPivot - 1));
            StartCoroutine(CoQuickSort(RightPivot + 1, Right));
        }

        if ((LeftPivot > RightPivot))
        {
            yield return new WaitForSeconds(0.5f);

            for (int i = 0; i < Count; i++)
            {
                BoxArray[i].TargetBox.color = Color.white;
            }

            Debug.Log("퀵 정렬 끝");
        }
    }

    private IEnumerator Partition(int Left, int Right)
    {
        int RandomCount = Random.Range(Left, Right);

        SwapValue(BoxArray[RandomCount], BoxArray[Right]);

        // Random Partition

        int PivotValue = BoxArray[Right].Value;

        int TempIndex = Left - 1;

        for (int j = Left; j <= Right - 1; j++)
        {
            

            if (BoxArray[j].Value <= PivotValue)
            {
                TempIndex++;
                SwapValue(BoxArray[TempIndex], BoxArray[j]);

                BoxArray[TempIndex].TargetBox.color = Color.red;
                BoxArray[j].TargetBox.color = Color.red;

                yield return new WaitForSeconds(0.3f);


                BoxArray[TempIndex].TargetBox.color = Color.white;
                BoxArray[j].TargetBox.color = Color.white;

            }
        }

        SwapValue(BoxArray[TempIndex + 1], BoxArray[Right]);

        TempIndex++;

        // Partition

        RandomQuickSort(Left, TempIndex - 1);
        RandomQuickSort(TempIndex + 1, Right);

        // Random Quick Sort
    }

    private void RandomQuickSort(int Left, int Right)
    {
        if(Left < Right)
        {
            StartCoroutine(Partition(Left, Right));
        }
    }

    private IEnumerator CoCountingSort(int Begin, int End)
    {
        int[] CountSum = new int[100001];
        int[] Temp = new int[100001];

        for(int i = Begin; i <= End; i++)
        {
            Temp[BoxArray[i].Value] += 1;
        }

        for(int i = 1; i <= 100000; i++)
        {
            Temp[i] += Temp[i - 1];
        }

        for(int i = 0; i <= End; i++)
        {
            CountSum[--Temp[BoxArray[i].Value]] = BoxArray[i].Value;
        }

        for(int i = 0; i <= End; i++)
        {
            BoxArray[i].Value = CountSum[i - 0];
            BoxArray[i].SetPosition();

            yield return new WaitForSeconds(0.2f);
        }
    }

    private IEnumerator CoRadixSort(int MaxSize, int End)
    {
        int Radix = 1;
        Queue<Box> Q = new Queue<Box>();

        while(true)
        {
            if (Radix >= MaxSize) break;
            Radix = Radix *= 10;
        }

        for(int i = 1; i < Radix; i = i * 10)
        {
            for(int j = 0; j < End; j++)
            {
                int k;

                if(BoxArray[j].Value < i)
                {
                    k = 0;
                }
                else
                {
                    k = (BoxArray[j].Value / i) % 10;
                    Q.Enqueue(BoxArray[j]);
                }
            }
        }

        int Index = 0;

        for(int j = 0; j < Q.Count; j++)
        {
            while(Q.Count == 0)
            {
                BoxArray[Index] = Q.Dequeue();
                BoxArray[Index].SetPosition();
                Index++;

                yield return new WaitForSeconds(0.2f);
            }
        }
    }

    void Start()
    {
        for(int i = 0; i < Count; i++)
        {
            Box TempBox = Instantiate(TargetBox) as Box;
            TempBox.gameObject.transform.SetParent(ListParent.transform);

            if(TempBox)
            {
                int TempSize = Random.Range(0, 100);

                if(TempSize > MaxValue)
                {
                    MaxValue = TempSize;
                }

                TempBox.Set(TempSize, i+1);
                BoxArray.Add(TempBox);
            }
        }

        Result = true;
    }
}
