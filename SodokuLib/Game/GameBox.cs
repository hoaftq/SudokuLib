using System;

namespace SodokuLib.Game
{
    public class GameBox
    {
        private int? displayValue;

        public int? DisplayValue
        {
            get => displayValue;
            set
            {
                if (IsFixed)
                {
                    throw new InvalidOperationException("Can not change DisplayValue when the box is fixed");
                }

                displayValue = value;
                if (displayValue == null)
                {
                    // Reset invalid states of the box
                    IsInvalidRow = IsInvalidCol = IsInvalidBlock = false;
                }
            }
        }

        public int Value { get; set; }

        public bool IsFixed { get; set; }

        public bool IsInvalidRow { get; set; }

        public bool IsInvalidCol { get; set; }

        public bool IsInvalidBlock { get; set; }

        public bool IsInvalid => IsInvalidRow || IsInvalidCol || IsInvalidBlock;
    }
}
