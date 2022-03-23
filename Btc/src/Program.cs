using Btc.CryptoMath;

try
{
    TestEllipticCurvePointFF();
}
catch (Exception e)
{
    Console.WriteLine(e.Message);
}

void TestEllipticCurvePointFF()
{
    int prime = 223;
    FieldElement a = new FieldElement(0, prime);
    FieldElement b = new FieldElement(7, prime);
    FieldElement x = new FieldElement(192, prime);
    FieldElement y = new FieldElement(105, prime);
    FieldElement[] xValid = new FieldElement[] {
        new FieldElement(192,prime),
        new FieldElement(17,prime),
        new FieldElement(1,prime)
    };
    FieldElement[] yValid = new FieldElement[] {
        new FieldElement(105,prime),
        new FieldElement(56,prime),
        new FieldElement(193,prime)
    };

    FieldElement[] xInvalid = new FieldElement[] {
        new FieldElement(200,prime),
        new FieldElement(42,prime)
    };
    FieldElement[] yInvalid = new FieldElement[] {
        new FieldElement(119,prime),
        new FieldElement(99,prime)
    };
    for (int i = 0; i < xValid.Length; i++)
    {
        try
        {
            EllipticCurvePointFF p1 = new EllipticCurvePointFF(xValid[i], yValid[i], a, b);
            Console.WriteLine(p1.ToString());
        }
        catch (Exception e)
        {
            Console.WriteLine(e.Message);
        }
    }

    for (int i = 0; i < xInvalid.Length; i++)
    {
        try
        {
            EllipticCurvePointFF p1 = new EllipticCurvePointFF(xInvalid[i], yInvalid[i], a, b);
            Console.WriteLine(p1.ToString());
        }
        catch (Exception e)
        {
            Console.WriteLine(e.Message);
        }
    }

}

void TestEllipticCurvePoint()
{
    EllipticCurvePoint p1;
    EllipticCurvePoint p2;
    EllipticCurvePoint inf;
    EllipticCurvePoint sum;
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

