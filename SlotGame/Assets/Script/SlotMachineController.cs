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
    }

    private void InitMachine()
    {
        for(int i = 0; i < slots.Length; i++)
        {
            slots[i].InitSlot(this);
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
