using SlotGame.Util;
using System.Collections;
using UnityEngine;

namespace SlotGame.SlotMachine
{
    /// <summary>
    /// Represent single coloum on the display
    /// Handles Slot movement, Slot stopping, Resetting Symbols
    /// </summary>
    public class Slot : MonoBehaviour
    {
        // Symbols - Holds the image icon
        [SerializeField] private Symbol[] symbols;

        // Symbols count which will be shown on display
        private int symbolCountOnScreen = 3;

        // spinning speed of this slot
        private float currentSpinSpeed = 0;

        // max spin speed of the this slot
        [SerializeField] private float maxSpinSpeed;

        // time to increase the spin speed to max speed
        [SerializeField] private float spinSpeedIncremetTime;

        // vertical space between two symbols(icon)
        private float spaceBetweenSymbols;

        // height of symbol(icon)
        private float symbolHeight;

        /// <summary>
        /// max down position of symbol, once symbol reached to threshold it will move to upward to get spin effect in circular 
        /// </summary>
        private float downMoveMaxThreshold;

        // used to start the slot spinning
        private bool canSpin;

        // used to stop the slot spinning
        private bool isStoped;

        // reference of SlotMachineController
        private SlotMachineController slotMachineController;

        // holds the reference of speed Increament coroutinue, to stop the coroutinue before starting new one
        private Coroutine speedIncrementCoroutinue;


        /// <summary>
        /// Initializes the slot
        /// </summary>
        /// <param name="_slotMachineController"></param>
        public void InitSlot(SlotMachineController _slotMachineController)
        {
            slotMachineController = _slotMachineController;

            CalculateSpaceAndMoveThreshold();
            SetSymbolPosition();
        }

        /// <summary>
        /// Reset slot to initial state
        /// </summary>
        public void ResetSlot()
        {
            currentSpinSpeed = 0;
            canSpin = false;
            isStoped = true;

            ReseTSymbols();
            SetSymbolPosition();
            SetSymbols();
        }

        private void LateUpdate()
        {
            if (canSpin)
            {
                SpinSymbols();
            }
        }

        /// <summary>
        /// Calculates the space between two Symbols(icons) and lower threshold value 
        /// </summary>
        private void CalculateSpaceAndMoveThreshold()
        {
            float totalHeight = gameObject.GetComponent<RectTransform>().rect.height;
            symbolHeight = symbols[0].ThisTransform.rect.height;

            float freeSpace = totalHeight - (symbolHeight * symbolCountOnScreen);

            spaceBetweenSymbols = freeSpace / ((symbolCountOnScreen - 1) + 2);
            downMoveMaxThreshold = 2 * (symbolHeight + spaceBetweenSymbols);
        }

        /// <summary>
        /// Resets each symbols present in this slot
        /// </summary>
        private void ReseTSymbols()
        {
            for (int i = 0; i < symbols.Length; i++)
            {
                symbols[i].ResetSymbol();
            }
        }

        /// <summary>
        /// Init each symbol by respected symbol sprite and Symbol type
        /// </summary>
        private void SetSymbols()
        {
            // randomIndex - store the value from 0 to symbolLength
            int[] randomIndex = new int[symbols.Length];
            for (int i = 0; i < symbols.Length; i++)
            {
                randomIndex[i] = i;
            }
            randomIndex.Shuffle();

            for (int i = 0; i < symbols.Length; i++)
            {
                SymbolType type = (SymbolType)randomIndex[i];
                Sprite sprite = slotMachineController.GetSymbolSprite(type);
                symbols[i].InitSymbol(sprite, type);
            }
        }

        /// <summary>
        /// Sets the position of each symbols 
        /// </summary>
        /// <param name="topSymbolIndex"> symbol present on the top, it's index  </param>
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

        /// <summary>
        /// Spins the Slots, moves each symbol in the down direction and once it's reached to threshold, shift to the top
        /// </summary>
        private void SpinSymbols()
        {
            bool movedToTopPos = false;
            int topSymbolIndex = -1;

            for (int i = 0; i < symbols.Length; i++)
            {
                symbols[i].ThisTransform.localPosition += currentSpinSpeed * Time.deltaTime * Vector3.down;

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

                    movedToTopPos = true;
                    topSymbolIndex = i;
                }
            }

            if (isStoped && movedToTopPos)
            {
                canSpin = false;
                SetSymbolPosition(topSymbolIndex);
            }
        }

        /// <summary>
        /// Start the Slot Spin
        /// </summary>
        public void Spin()
        {
            canSpin = true;
            isStoped = false;

            if (speedIncrementCoroutinue != null)
            {
                StopCoroutine(speedIncrementCoroutinue);
            }
            speedIncrementCoroutinue = StartCoroutine(SpeedIncrement());
        }

        /// <summary>
        /// stops slot
        /// </summary>
        public void Stop()
        {
            isStoped = true;
        }

        /// <summary>
        /// increases the Spin speed to max spin speed in the spacidied time
        /// </summary>
        /// <returns></returns>
        private IEnumerator SpeedIncrement()
        {
            float elapcedTime = 0;

            while (elapcedTime < spinSpeedIncremetTime)
            {
                elapcedTime += Time.deltaTime;
                currentSpinSpeed = Mathf.Lerp(0, maxSpinSpeed, elapcedTime / spinSpeedIncremetTime);

                yield return null;
            }
            currentSpinSpeed = maxSpinSpeed;
        }

        /// <summary>
        /// returns the symbol present in the specific row
        /// </summary>
        private Symbol GetSymbolPresentInRow(int row)
        {
            Symbol symbol = null;

            switch (row)
            {
                case 0:
                    symbol = GetSymbolPresentAtYPos(-(symbolHeight + spaceBetweenSymbols));
                    break;

                case 1:
                    symbol = GetSymbolPresentAtYPos(0.0f);
                    break;

                case 2:
                    symbol = GetSymbolPresentAtYPos((symbolHeight + spaceBetweenSymbols));
                    break;
            }

            return symbol;
        }

        /// <summary>
        ///  returns the symbol types of Symbols present in the specific row
        /// </summary>
        public SymbolType GetSymbolTypeInRow(int row)
        {
            Symbol symbol = GetSymbolPresentInRow(row);
            if (symbol != null)
            {
                return symbol.SymbolType;
            }

            return SymbolType.Symbol_1;
        }

        /// <summary>
        /// finds and return the Symbol present at y Position
        /// </summary>
        private Symbol GetSymbolPresentAtYPos(float yPos)
        {
            Symbol symbol = null;
            for (int i = 0; i < symbols.Length; i++)
            {
                if (symbols[i].ThisTransform.localPosition.y == yPos)
                {
                    symbol = symbols[i];
                    break;
                }
            }
            return symbol;
        }

        /// <summary>
        /// Plays the Win ANimation
        /// </summary>
        public void PlaySymbolWinAnimationInRow(int row)
        {
            Symbol symbol = GetSymbolPresentInRow(row);
            symbol.PlayWinAnimation();
        }
    }
}