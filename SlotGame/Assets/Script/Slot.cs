using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Slot : MonoBehaviour
{
    [SerializeField] private Symbol[] symbols;

    [SerializeField] private int imagesOnScreen = 3;

    [SerializeField] private float currentMoveSpeed = 0;
    [SerializeField] private float maxMoveSpeed;
    [SerializeField] private float speedIncrementTime;

    private float spaceBetweenImages;
    private float imageHeight;
    private float maxThreshold;
    private bool canMove;
    private bool isStoped;

    private SlotMachineController slotMachineController;
   
    public void InitSlot(SlotMachineController _slotMachineController)
    {
        slotMachineController = _slotMachineController;

        SetSymbols();
        CalculateVaribles();
        SetImagesPosition();
    }

    private void CalculateVaribles()
    {
        float totalHeight = gameObject.GetComponent<RectTransform>().rect.height;
        imageHeight = symbols[0].GetComponent<RectTransform>().rect.height;

        float freeHeight = totalHeight - (imageHeight * imagesOnScreen);

        spaceBetweenImages = freeHeight / ((imagesOnScreen - 1) + 2);

        maxThreshold = 2 * (imageHeight + spaceBetweenImages);
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

    private void SetImagesPosition()
    {
        symbols[0].ThisTransform.localPosition = new Vector3(0, -(imageHeight + spaceBetweenImages), 0);
        symbols[1].ThisTransform.localPosition = Vector3.zero;

        for (int i = 2; i < symbols.Length; i++)
        {
            symbols[i].ThisTransform.localPosition = new Vector3(0, (imageHeight + spaceBetweenImages) * (i - 1), 0);
        }
    }

    private void MoveImages()
    {
        bool flag = false;
        int index = -1;
        for (int i = 0; i < symbols.Length; i++)
        {
            symbols[i].ThisTransform.localPosition += currentMoveSpeed * Time.deltaTime * Vector3.down;

            if (symbols[i].ThisTransform.localPosition.y < -maxThreshold)
            {
                if (i == 0)
                {
                    symbols[i].ThisTransform.localPosition = symbols[symbols.Length - 1].ThisTransform.localPosition + new Vector3(0, imageHeight + spaceBetweenImages, 0);
                }
                else
                {
                    symbols[i].ThisTransform.localPosition = symbols[i - 1].ThisTransform.localPosition + new Vector3(0, imageHeight + spaceBetweenImages, 0);
                }
                flag = true;
                index = i;
            }
        }

        if(isStoped && flag)
        {
            canMove = false;
            AllignSymbols(index);
        }
    }

    private void AllignSymbols(int index)
    {
        index = (index + 1) % symbols.Length;

        symbols[index].ThisTransform.localPosition = new Vector3(0, -(imageHeight + spaceBetweenImages), 0);

        index = (index + 1) % symbols.Length;

        symbols[index].ThisTransform.localPosition = Vector3.zero;

        for (int i = 2; i < symbols.Length; i++)
        {
            index = (index + 1) % symbols.Length;
            symbols[index].ThisTransform.localPosition = new Vector3(0, (imageHeight + spaceBetweenImages) * (i - 1), 0);
        }
    }

    public void Move()
    {
        canMove = true;
        isStoped = false;
        StartCoroutine(SpeedIncrement());
    }

    public void Stop()
    {
        isStoped = true;
    }

    private void LateUpdate()
    {
        if(canMove)
        {
            MoveImages();
        }
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
