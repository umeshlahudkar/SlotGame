using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SlotMachineController : Singleton<SlotMachineController>
{
    [SerializeField] private Slot[] slots;
    [SerializeField] private Sprite[] symbolSprites;
    [SerializeField] private float slotMovementDalay;

    private WaitForSeconds slotMoveWaitForSeconds;

    private int[] winCombinationSymbols = new int[] { -1, -1, -1, -1, -1 };

    private void Start()
    {
        InitMachine();
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Space))
        {
            StartCoroutine(MoveSlots());
        }
        else if (Input.GetKeyDown(KeyCode.A))
        {
            StartCoroutine(StopSlots());
        }
    }

    private void InitMachine()
    {
        for(int i = 0; i < slots.Length; i++)
        {
            slots[i].InitSlot(this);
        }
    }

    private void ResetMachine()
    {
        for (int i = 0; i < slots.Length; i++)
        {
            slots[i].ResetSlot();
        }
    }

    public void SpinSlotMachine()
    {
        StartCoroutine(SpinMachineCo());
    }

    private IEnumerator SpinMachineCo()
    {
        ResetMachine();

        yield return StartCoroutine(MoveSlots());
        yield return new WaitForSeconds(5f);
        yield return StartCoroutine(StopSlots());

        if(CheckForWinningCombination())
        {
            for(int i = 0; i < slots.Length; i++)
            {
                slots[i].PlaySymbolWinAnimationInRow(winCombinationSymbols[i]);
            }

            yield return new WaitForSeconds(2f);
            UIController.Instance.OpenWinScreen();
        }
        else
        {
            UIController.Instance.ToggleStartButton(true);
        }

    }

    private IEnumerator MoveSlots()
    {
        if(slotMoveWaitForSeconds == null) { slotMoveWaitForSeconds = new WaitForSeconds(slotMovementDalay); }

        for (int i = 0; i < slots.Length; i++)
        {
            slots[i].Move();
            yield return slotMoveWaitForSeconds;
        }
    }

    private IEnumerator StopSlots()
    {
        if (slotMoveWaitForSeconds == null) { slotMoveWaitForSeconds = new WaitForSeconds(slotMovementDalay); }

        for (int i = 0; i < slots.Length; i++)
        {
            slots[i].Stop();
            yield return slotMoveWaitForSeconds;
        }
    }

    private bool CheckForWinningCombination()
    {
        for(int i = 0; i < 3; i++)
        {
            int count = 0;
            int row = i;

            winCombinationSymbols[0] = row;

            for(int j = 0; j < slots.Length - 1; j++)
            {
                if (!IsSymbolMatchedToNextAdjacent(j, ref row))
                {
                    break;
                }
                else
                {
                    count++;
                    winCombinationSymbols[j + 1] = row;
                }
            }

            if (count >= slots.Length - 1)
            {
                return true;
            }

            winCombinationSymbols.Clear();
        }
        return false;
    }

    private bool IsSymbolMatchedToNextAdjacent(int currentSlotIndex, ref int row)
    {
        if(!IsValidSlot(currentSlotIndex) || !IsValidRow(row)) { return false; }

        SymbolType currentSymbolType = slots[currentSlotIndex].GetSymbolTypeInRow(row);

        int nextSlotIndex = currentSlotIndex + 1;

        if (!IsValidSlot(nextSlotIndex)) { return false; }


        if(currentSymbolType == slots[nextSlotIndex].GetSymbolTypeInRow(row))
        {
            return true;
        }


        if ( IsValidRow(row + 1)  && currentSymbolType == slots[nextSlotIndex].GetSymbolTypeInRow(row + 1))
        {
            row = row + 1;
            return true;
        }

        if (IsValidRow(row - 1) && currentSymbolType == slots[nextSlotIndex].GetSymbolTypeInRow(row - 1))
        {
            row = row - 1;
            return true;
        }

        return false;
    }
   
    private bool IsValidRow(int row)
    {
        return (row >= 0 && row <= 2);
    }

    private bool IsValidSlot(int slotIndex)
    {
        return (slotIndex >= 0 && slotIndex < slots.Length);
    }

    public Sprite GetSymbolSprite(SymbolType type)
    {
        int index = (int)type;
        if(index < 0 || index >= symbolSprites.Length)
        {
            return null;
        }
        return symbolSprites[index];
    }
}
