using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SlotMachineController : MonoBehaviour
{
    [SerializeField] private Slot[] slots;
    [SerializeField] private Sprite[] symbolSprites;
    [SerializeField] private float slotMovementDalay;

    private WaitForSeconds slotMoveWaitForSeconds;

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
        CheckForWinningCombination();
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

    private void CheckForWinningCombination()
    {
        Debug.Log(slots[0].GetSymbolAt(0));
        Debug.Log(slots[0].GetSymbolAt(1));
        Debug.Log(slots[0].GetSymbolAt(2));
    }

    public Sprite GetSymbol(SymbolType type)
    {
        int index = (int)type;
        if(index < 0 || index >= symbolSprites.Length)
        {
            return null;
        }
        return symbolSprites[index];
    }
}
