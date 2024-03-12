using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Slot : MonoBehaviour
{
    // Symbols - Holds the image icon
    [SerializeField] private Symbol[] symbols;


    [SerializeField] private int symbolCountOnScreen = 3;

    [SerializeField] private float currentMoveSpeed = 0;
    [SerializeField] private float maxMoveSpeed;
    [SerializeField] private float speedIncrementTime;

    private float spaceBetweenSymbols;
    private float symbolHeight;
    private float downMoveMaxThreshold;
    private bool canMove;
    private bool isStoped;

    private SlotMachineController slotMachineController;
    private Coroutine speedIncrementCoroutinue;
   
    public void InitSlot(SlotMachineController _slotMachineController)
    {
        slotMachineController = _slotMachineController;

        SetSymbols();
        CalculateSpaceAndMoveThreshold();
        SetSymbolPosition();
    }

    public void ResetSlot()
    {
        currentMoveSpeed = 0;
        canMove = false;
        isStoped = true;

        SetSymbolPosition();
        SetSymbols();
    }

    private void LateUpdate()
    {
        if (canMove)
        {
            MoveImages();
        }
    }

    private void CalculateSpaceAndMoveThreshold()
    {
        float totalHeight = gameObject.GetComponent<RectTransform>().rect.height;
        symbolHeight = symbols[0].ThisTransform.rect.height;

        float freeSpace = totalHeight - (symbolHeight * symbolCountOnScreen);

        spaceBetweenSymbols = freeSpace / ((symbolCountOnScreen - 1) + 2);
        downMoveMaxThreshold = 2 * (symbolHeight + spaceBetweenSymbols);
    }

    private void SetSymbols()
    {
        int[] randomIndex = new int[symbols.Length];
        for(int i = 0; i < symbols.Length; i++)
        {
            randomIndex[i] = i;
        }
        randomIndex.Shuffle();

        for(int i = 0; i < symbols.Length; i++)
        {
            SymbolType type = (SymbolType)randomIndex[i];
            Sprite sprite = slotMachineController.GetSymbol(type);
            symbols[i].InitSymbol(sprite, type);
        }
    }

    private void SetSymbolPosition(int topSymbolIndex = 0)
    {
        topSymbolIndex = (topSymbolIndex + 1) % symbols.Length;
        symbols[topSymbolIndex].ThisTransform.localPosition = new Vector3(0, -(symbolHeight + spaceBetweenSymbols), 0);

        topSymbolIndex = (topSymbolIndex + 1) % symbols.Length;
        symbols[topSymbolIndex].ThisTransform.localPosition = Vector3.zero;

        for (int i = 2; i < symbols.Length; i++)
        {
            topSymbolIndex = (topSymbolIndex + 1) % symbols.Length;
            symbols[topSymbolIndex].ThisTransform.localPosition = new Vector3(0, (symbolHeight + spaceBetweenSymbols) * (i - 1), 0);
        }
    }

    private void MoveImages()
    {
        bool flag = false;
        int topSymbolIndex = -1;
        for (int i = 0; i < symbols.Length; i++)
        {
            symbols[i].ThisTransform.localPosition += currentMoveSpeed * Time.deltaTime * Vector3.down;

            if (symbols[i].ThisTransform.localPosition.y < -downMoveMaxThreshold)
            {
                if (i == 0)
                {
                    symbols[i].ThisTransform.localPosition = symbols[symbols.Length - 1].ThisTransform.localPosition + new Vector3(0, symbolHeight + spaceBetweenSymbols, 0);
                }
                else
                {
                    symbols[i].ThisTransform.localPosition = symbols[i - 1].ThisTransform.localPosition + new Vector3(0, symbolHeight + spaceBetweenSymbols, 0);
                }
                flag = true;
                topSymbolIndex = i;
            }
        }

        if(isStoped && flag)
        {
            canMove = false;
            SetSymbolPosition(topSymbolIndex);
        }
    }

    public void Move()
    {
        canMove = true;
        isStoped = false;

        if(speedIncrementCoroutinue != null)
        {
            StopCoroutine(speedIncrementCoroutinue);
        }
        speedIncrementCoroutinue = StartCoroutine(SpeedIncrement());
    }

    public void Stop()
    {
        isStoped = true;
    }

    private IEnumerator SpeedIncrement()
    {
        float elapcedTime = 0;

        while(elapcedTime < speedIncrementTime)
        {
            elapcedTime += Time.deltaTime;
            currentMoveSpeed = Mathf.Lerp(0, maxMoveSpeed, elapcedTime / speedIncrementTime);

            yield return null;
        }
        currentMoveSpeed = maxMoveSpeed;
    }
}
