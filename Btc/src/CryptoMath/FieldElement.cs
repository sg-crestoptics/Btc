namespace Btc.CryptoMath
{
    internal class FieldElement
    {
        public FieldElement(int value, int prime)
        {
            if (value >= prime || value < 0)
            {
                throw new ArgumentException("Invalid value! Make sure 0 <= value < prime.");
            }
            Value = value;
            Prime = prime;
        }

        public int Value { get; set; }
        public int Prime { get; set; }

        #region OperatorOverloading

        #region ToString
        public override string ToString()
        {
            return $"({Value},{Prime})";
        }
        #endregion

        #region Equality
        public override bool Equals(object obj) => this.Equals(obj as FieldElement);
        // override object.Equals
        public bool Equals(FieldElement obj)
        {

            if (obj == null || GetType() != obj.GetType())
            {
                return false;
            }
            if (Object.ReferenceEquals(this, obj))
                return true;
            return Value == obj.Value && Prime == obj.Prime;

        }

        // override object.GetHashCode
        public override int GetHashCode() => (Value, Prime).GetHashCode();
        public static bool operator ==(FieldElement lhs, FieldElement rhs)
        {
            if (lhs is null)
            {
                if (rhs is null)
                {
                    return true;
                }

                // Only the left side is null.
                return false;
            }
            // Equals handles case of null on right side.
            return lhs.Equals(rhs);
        }
        public static bool operator !=(FieldElement lhs, FieldElement rhs) => !(lhs == rhs);
        #endregion

        #region AdditionSubtraction
        public static FieldElement operator +(FieldElement lhs, FieldElement rhs)
        {
            if (lhs.Prime != rhs.Prime)
                throw new InvalidOperationException("Cannot subtract two field elements with different prime values!");
            int valueSum = (lhs.Value + rhs.Value) % lhs.Prime;
            int value = valueSum > 0 ? valueSum : lhs.Prime + valueSum;
            return new FieldElement(value, lhs.Prime);
        }

        public static FieldElement operator -(FieldElement lhs, FieldElement rhs)
        {
            if (lhs.Prime != rhs.Prime)
                throw new InvalidOperationException("Cannot add two field elements with different prime values!");
            int valueSum = (lhs.Value - rhs.Value) % lhs.Prime;
            int value = valueSum > 0 ? valueSum : lhs.Prime + valueSum;
            return new FieldElement(value, lhs.Prime);
        }
        #endregion

        #region Multiplication
        public static FieldElement operator *(FieldElement lhs, FieldElement rhs)
        {
            if (lhs.Prime != rhs.Prime)
                throw new InvalidOperationException("Cannot multiply two field elements with different prime values!");
            int valueSum = (lhs.Value * rhs.Value) % lhs.Prime;
            int value = valueSum > 0 ? valueSum : lhs.Prime + valueSum;
            return new FieldElement(valueSum, lhs.Prime);
        }
        public static FieldElement operator *(FieldElement lhs, int rhs)
        {
            int valueSum = (lhs.Value * rhs) % lhs.Prime;
            int value = valueSum > 0 ? valueSum : lhs.Prime + valueSum;
            return new FieldElement(value, lhs.Prime);
        }
        public static FieldElement operator *(int lhs, FieldElement rhs) => rhs * lhs;
        #endregion

        #region Division
        public static FieldElement operator /(FieldElement lhs, FieldElement rhs)
        {
            if (lhs.Prime != rhs.Prime)
                throw new InvalidOperationException("Cannot divide two field elements with different prime values!");
            if (rhs.Value == 0)
                throw new InvalidOperationException("Cannot divide by zero!");
            long value = (long)(lhs.Value * Math.Pow((double)rhs.Value, (double)rhs.Prime - 2));
            value %= lhs.Prime;
            return new FieldElement((int)value, lhs.Prime);

        }
        #endregion

        #region Exponential
        public static FieldElement Pow(FieldElement lhs, int rhs)
        {
            int exponent = rhs;
            while (exponent < 0)
            {
                exponent += lhs.Prime - 1;
            }
            long valueSum = (long)(Math.Pow((double)lhs.Value, (double)exponent) % lhs.Prime);
            valueSum %= lhs.Prime;
            int value = valueSum > 0 ? (int)valueSum : lhs.Prime + (int)valueSum;
            return new FieldElement(value, lhs.Prime);
        }
        #endregion


        #endregion
    }
}
