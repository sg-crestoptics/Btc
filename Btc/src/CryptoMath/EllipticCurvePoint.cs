using Btc.CryptoMath.Exceptions;

namespace Btc.CryptoMath

{
    /// <summary>
    /// Class describing a point on an elliptic curve
    /// <c>y² = ax³ + b + c</c>
    /// </summary>
    internal class EllipticCurvePoint
    {
        /// <summary>
        /// A point P(x,y) in an elliptic curve of type
        /// <c>y² = x³ + <paramref name="a"/>x + <paramref name="b"/></c>
        /// </summary>
        /// <param name="x">X</param>
        /// <param name="y">Y</param>
        /// <param name="a">A</param>
        /// <param name="b">B</param>
        /// <exception cref="ExceptionPointNotOnCurve">Raised when the x and y specified does not match the elliptic curve equation for the particolar a and b passed</exception>
        public EllipticCurvePoint(float? x, float? y, float a, float b)
        {
            A = a;
            B = b;
            //the case for infinity point
            if (!x.HasValue && !y.HasValue)
                return;
            //if only one between x and y is null, point is not valid
            if ((!x.HasValue && y.HasValue) || (x.HasValue && !y.HasValue))
            {
                throw new ArgumentException("Invalid value for x and y. Give a value to both x,y or give null to both x,y to obtain the point at infinity for the elliptic curve defined by A and B");
            }
            X = x.Value;
            Y = y.Value;
            if (MathF.Pow(Y.Value, 2) != MathF.Pow(X.Value, 3) + A * X + B)
                throw new ExceptionPointNotOnCurve(String.Format("Point ({0},{1}) is not on the elliptic curve.", X, Y));
        }

        public float? X { get; }
        public float? Y { get; }
        public float A { get; }
        public float B { get; }

        #region InfinityPoint
        public bool IsInfinity => !X.HasValue && !Y.HasValue;
        /// <summary>
        /// Returns the infinity point for the elliptic curve 
        /// <c>y² = x³ + <paramref name="a"/>x + <paramref name="b"/></c>
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        public static EllipticCurvePoint InfinityPoint(float a, float b)
        {
            return new EllipticCurvePoint(null, null, a, b);
        }
        #endregion

        public override bool Equals(object obj) => this.Equals(obj as EllipticCurvePoint);

        public bool Equals(EllipticCurvePoint obj)
        {
            if (obj == null || GetType() != obj.GetType())
            {
                return false;
            }

            if (Object.ReferenceEquals(this, obj))
                return true;
            return A == obj.A && B == obj.B && X == obj.X && Y == obj.Y;
        }
        public override int GetHashCode() => (X, Y, A, B).GetHashCode();

        public static bool operator ==(EllipticCurvePoint lhs, EllipticCurvePoint rhs)
        {
            if (lhs is null)
            {
                if (rhs is null)
                {
                    return true;
                }
                return false;
            }
            return lhs.Equals(rhs);
        }
        public static bool operator !=(EllipticCurvePoint lhs, EllipticCurvePoint rhs) => !(lhs == rhs);

        public static EllipticCurvePoint operator +(EllipticCurvePoint lhs, EllipticCurvePoint rhs)
        {
            if (lhs.A != rhs.A || lhs.B != rhs.B)
                throw new ArgumentException("Adding two points not on the same curve");

            // point at infinity
            if (!lhs.X.HasValue)
                return rhs;
            if (!rhs.X.HasValue)
                return lhs;

            // the case where x is the same, but y is different returns the infinity point
            if (lhs.X.Value == rhs.X.Value && lhs.Y.Value != rhs.Y.Value)
                return new EllipticCurvePoint(null, null, lhs.A, lhs.B);

            float slope;
            float x;
            float y;
            // if P1(lhs) == P2(rhs), we are in the tangent case
            if (lhs == rhs)
            {
                // if Y is 0 we return the infinity point
                if (lhs.Y.Value == 0)
                    return new EllipticCurvePoint(null, null, lhs.A, rhs.B);

                slope = (3 * MathF.Pow(lhs.X.Value, 2) + lhs.A) / (2 * lhs.Y.Value);
                x = MathF.Pow(slope, 2) - (2 * lhs.X.Value);
                y = slope * (lhs.X.Value - x) - lhs.Y.Value;
            }
            else // normal case P1(lhs) != P2(rhs) and X1 != X2
            {
                slope = (rhs.Y.Value - lhs.Y.Value) / (lhs.X.Value - rhs.X.Value);
                x = MathF.Pow(slope, 2) - lhs.X.Value - rhs.X.Value;
                y = slope * (lhs.X.Value - x) - lhs.Y.Value;
            }


            return new EllipticCurvePoint(x, y, lhs.A, lhs.B);
        }

    }
}
