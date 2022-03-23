using Btc.CryptoMath;
using Btc.CryptoMath.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Btc.CryptoMath
{
    /// <summary>
    /// Class describing a point on an elliptic curve with finite field math
    /// <c>y² = ax³ + b + c</c>
    /// </summary>
    internal class EllipticCurvePointFF
    {
        /// <summary>
        /// A point P(x,y) in an elliptic curve in the finite field of type
        /// <c>y² = x³ + <paramref name="a"/>x + <paramref name="b"/></c>
        /// </summary>
        /// <param name="x">X</param>
        /// <param name="y">Y</param>
        /// <param name="a">A</param>
        /// <param name="b">B</param>
        /// <exception cref="ExceptionPointNotOnCurve">Raised when the x and y specified does not match the elliptic curve equation for the particolar a and b passed</exception>
        public EllipticCurvePointFF(FieldElement x, FieldElement y, FieldElement a, FieldElement b)
        {
            A = a;
            B = b;
            //the case for infinity point
            if (x == null && y == null)
                return;
            //if only one between x and y is null, point is not valid
            if ((x == null && y != null) || (x != null && y == null))
            {
                throw new ArgumentException("Invalid value for x and y. Give a value to both x,y or give null to both x,y to obtain the point at infinity for the elliptic curve defined by A and B");
            }
            X = x;
            Y = y;
            if (FieldElement.Pow(Y, 2) != FieldElement.Pow(X, 3) + A * X + B)
            {
                throw new ExceptionPointNotOnCurve(String.Format("Point ({0},{1}) is not on the elliptic curve.", X, Y));
            }

        }

        public FieldElement X { get; }
        public FieldElement Y { get; }

        public FieldElement A { get; }
        public FieldElement B { get; }

        #region InfinityPoint
        public bool IsInfinity => X == null && Y == null;
        /// <summary>
        /// Returns the infinity point for the elliptic curve 
        /// <c>y² = x³ + <paramref name="a"/>x + <paramref name="b"/></c>
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        public static EllipticCurvePointFF InfinityPoint(FieldElement a, FieldElement b)
        {
            return new EllipticCurvePointFF(null, null, a, b);
        }
        #endregion

        #region OperatorOverride

        #region ToString
        public override string ToString()
        {
            if (X == null && Y == null)
            {
                return $"(X:null, Y:null, A:{A.ToString()}, B:{B.ToString()})";

            }
            return $"(X:{X.ToString()}, Y:{Y.ToString()}, A:{A.ToString()}, B:{B.ToString()})";
        }
        #endregion

        #region Equality
        public override bool Equals(object obj) => this.Equals(obj as EllipticCurvePointFF);

        public bool Equals(EllipticCurvePointFF obj)
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

        public static bool operator ==(EllipticCurvePointFF lhs, EllipticCurvePointFF rhs)
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
        public static bool operator !=(EllipticCurvePointFF lhs, EllipticCurvePointFF rhs) => !(lhs == rhs);
        #endregion

        #region Sum
        public static EllipticCurvePointFF operator +(EllipticCurvePointFF lhs, EllipticCurvePointFF rhs)
        {
            if (lhs.A != rhs.A || lhs.B != rhs.B)
                throw new ArgumentException("Adding two points not on the same curve");

            // point at infinity
            if (lhs.X == null)
                return rhs;
            if (rhs.X == null)
                return lhs;

            // the case where x is the same, but y is different returns the infinity point
            if (lhs.X == rhs.X && lhs.Y != rhs.Y)
                return new EllipticCurvePointFF(null, null, lhs.A, lhs.B);

            FieldElement slope;
            FieldElement x;
            FieldElement y;
            // if P1(lhs) == P2(rhs), we are in the tangent case
            if (lhs == rhs)
            {
                // if Y is 0 we return the infinity point
                if (lhs.Y.Value == 0)
                    return new EllipticCurvePointFF(null, null, lhs.A, rhs.B);

                slope = (3 * FieldElement.Pow(lhs.X, 2) + lhs.A) / (2 * lhs.Y);
                x = FieldElement.Pow(slope, 2) - (2 * lhs.X);
                y = slope * (lhs.X - x) - lhs.Y;
            }
            else // normal case P1(lhs) != P2(rhs) and X1 != X2
            {
                slope = (rhs.Y - lhs.Y) / (lhs.X - rhs.X);
                x = FieldElement.Pow(slope, 2) - lhs.X - rhs.X;
                y = slope * (lhs.X - x) - lhs.Y;
            }


            return new EllipticCurvePointFF(x, y, lhs.A, lhs.B);
        }
        #endregion

        #endregion
    }
}
