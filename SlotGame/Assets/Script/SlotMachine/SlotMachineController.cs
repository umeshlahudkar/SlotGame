using System.Collections;
using UnityEngine;
using SlotGame.Util;
using SlotGame.UI;

namespace SlotGame.SlotMachine
{
    /// <summary>
    /// Handle the slot machine Spinning, Stopping, Checks for any win combinations
    /// </summary>
    public class SlotMachineController : Singleton<SlotMachineController>
    {
        // references of slots
        [SerializeField] private Slot[] slots;

        // sprites of Symbols(icons), stored in the same order as SymbolType enum is created
        [SerializeField] private Sprite[] symbolSprites;

        // delay between each slot for spinnig, and stopping
        private float slotSpinningDalay = 0.15f;

        // delay between slots spin start and stop
        [SerializeField] private float delayBetweenSpinStartStop;

        // delay between slots win animation and win screen openning 
        private float delayBetweenSlotsWinAnimWinScreen = 1.5f;

        // stores the win combination position, array index is the Slot position, and each element represents row position at each slot
        private int[] winCombinationPos = new int[] { -1, -1, -1, -1, -1 };

        // used for spinning the slots without checking WinCombination
        private bool isTesting = false;

        private WaitForSeconds delayBetweenSlots;
        private WaitForSeconds waitBetweenSpinStartStop;
        private WaitForSeconds waitBetweenWinAnimScreen;


        private void Start()
        {
            InitMachine();
        }

        private void InitMachine()
        {
            delayBetweenSlots = new WaitForSeconds(slotSpinningDalay);
            waitBetweenSpinStartStop = new WaitForSeconds(delayBetweenSpinStartStop);
            waitBetweenWinAnimScreen = new WaitForSeconds(delayBetweenSlotsWinAnimWinScreen);

            for (int i = 0; i < slots.Length; i++)
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

        /// <summary>
        /// spins slot machine with checking win combination
        /// </summary>
        public void SpinSlotMachine()
        {
            isTesting = false;
            StartCoroutine(HandleSlotsSpinning());
        }

        /// <summary>
        /// spins slot machine without checking win combination
        /// </summary>
        public void SpinSlotMachineForTest()
        {
            isTesting = true;
            StartCoroutine(HandleSlotsSpinning());
        }

        /// <summary>
        /// Handles the slots spinning operation
        /// </summary>
        /// <returns></returns>
        private IEnumerator HandleSlotsSpinning()
        {
            ResetMachine();

            yield return StartCoroutine(SpinSlots());
            yield return waitBetweenSpinStartStop;
            yield return StartCoroutine(StopSlots());

            if (isTesting)
            {
                int row = Random.Range(0, 3);
                for (int i = 0; i < slots.Length; i++)
                {
                    slots[i].PlaySymbolWinAnimationInRow(row);
                }

                yield return waitBetweenWinAnimScreen;
                UIController.Instance.OpenWinScreen();
            }
            else if (CheckForWinningCombination())
            {
                for (int i = 0; i < slots.Length; i++)
                {
                    slots[i].PlaySymbolWinAnimationInRow(winCombinationPos[i]);
                }

                yield return waitBetweenWinAnimScreen;
                UIController.Instance.OpenWinScreen();
            }

            UIController.Instance.ToggleStartButton(true);
        }

        /// <summary>
        /// Starts spinning operation one by one with some delay
        /// </summary>
        /// <returns></returns>
        private IEnumerator SpinSlots()
        {
            for (int i = 0; i < slots.Length; i++)
            {
                slots[i].Spin();
                yield return delayBetweenSlots;
            }
        }

        /// <summary>
        /// stops spinning operation one by one with some delay
        /// </summary>
        /// <returns></returns>
        private IEnumerator StopSlots()
        {
            for (int i = 0; i < slots.Length; i++)
            {
                slots[i].Stop();
                yield return delayBetweenSlots;
            }
        }

        /// <summary>
        /// Checks for the any winning combination present or not
        /// </summary>
        /// <returns></returns>
        private bool CheckForWinningCombination()
        {
            // 3 represnts that each slots display 3 rows on display
            for (int i = 0; i < 3; i++)
            {
                int count = 0;
                int row = i;

                winCombinationPos[0] = row;

                for (int j = 0; j < slots.Length - 1; j++)
                {
                    if (!IsSymbolMatchedToNextAdjacent(j, ref row))
                    {
                        break;
                    }
                    else
                    {
                        count++;
                        winCombinationPos[j + 1] = row;
                    }
                }

                if (count >= slots.Length - 1)
                {
                    return true;
                }

                winCombinationPos.Clear();
            }
            return false;
        }


        /// <summary>
        /// Checks if the Current slot symbol is matched to the next slots symbols
        /// </summary>
        private bool IsSymbolMatchedToNextAdjacent(int currentSlotIndex, ref int row)
        {
            if (!IsValidSlot(currentSlotIndex) || !IsValidRow(row)) { return false; }

            SymbolType currentSymbolType = slots[currentSlotIndex].GetSymbolTypeInRow(row);

            int nextSlotIndex = currentSlotIndex + 1;

            if (!IsValidSlot(nextSlotIndex)) { return false; }


            if (currentSymbolType == slots[nextSlotIndex].GetSymbolTypeInRow(row))
            {
                return true;
            }


            if (IsValidRow(row + 1) && currentSymbolType == slots[nextSlotIndex].GetSymbolTypeInRow(row + 1))
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

        /// <summary>
        /// Checks if the row is in the range
        /// </summary>
        private bool IsValidRow(int row)
        {
            return (row >= 0 && row <= 2);
        }

        /// <summary>
        /// Checks if the slot is in the range
        /// </summary>
        private bool IsValidSlot(int slotIndex)
        {
            return (slotIndex >= 0 && slotIndex < slots.Length);
        }

        /// <summary>
        /// return the symbol sprite based on the type passed in parameter
        /// symbol sprites stores in the same order as the SymbolType are
        /// </summary>
        public Sprite GetSymbolSprite(SymbolType type)
        {
            int index = (int)type;
            if (index < 0 || index >= symbolSprites.Length)
            {
                return null;
            }
            return symbolSprites[index];
        }
    }

}