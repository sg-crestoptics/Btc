using Btc.CryptoMath;
EllipticCurvePoint p1;
EllipticCurvePoint p2;
EllipticCurvePoint inf;
EllipticCurvePoint sum;

try
{
    p1 = new EllipticCurvePoint(-1, -1, 5, 7);
    p2 = new EllipticCurvePoint(-1, 1, 5, 7);
    inf = EllipticCurvePoint.InfinityPoint(5, 7);

    sum = p1 + inf;
    Console.WriteLine($"X:{sum.X} Y:{sum.Y}");

    sum = inf + p2;
    Console.WriteLine($"X:{sum.X} Y:{sum.Y}");

    sum = p1 + p2;
    Console.WriteLine($"p1 + p2 == infinito:{p1.IsInfinity}");


}
catch (Exception e)
{
    Console.WriteLine(e.Message);
}

