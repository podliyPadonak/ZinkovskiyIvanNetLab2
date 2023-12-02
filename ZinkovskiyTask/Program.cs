// Абстрактный класс «Функция формы»
abstract class ShapeFunction
{
    protected int Size; // Кількість коефіцієнтів функції форми
    protected int Dim; // Розмірність
    protected double[,] C; // Матриця коефіцієнтів
    protected double[,] X; // Координати вершин СЕ
    protected ShapeFunction()
    {
        Size = Dim = 0;
    }
    protected void SetCoord(int psize, double[,] px)
    {
        Size = psize;
        X = new double[Size, Dim];
        for (int i = 0; i < Size; i++)
            for (int j = 0; j < Dim; j++)
                X[i, j] = px[i, j];
        Create();
    }
    protected abstract double ShapeCoeff(int i, int j);
    private bool Solve(double[,] matr, double[] result, double eps = 1.0E-10)
    {
        double coeff;
        for (var i = 0; i < Size - 1; i++)
        {
            if (matr[i, i] < eps)
                continue;
            for (var j = i + 1; j < Size; j++)
            {
                if (Math.Abs(coeff = matr[j, i]) < eps)
                    continue;
                for (var k = i; k < Size + 1; k++)
                    matr[j, k] -= (coeff * matr[i, k] / matr[i, i]);
            }
        }
        if (Math.Abs(matr[Size - 1, Size - 1]) < eps)
            return false;
        result[Size - 1] = matr[Size - 1, Size] / matr[Size - 1, Size - 1];
        for (int k = 0; k < Size - 1; k++)
        {
            int i = Size - k - 2;
            var sum = matr[i, Size];

            for (int j = i + 1; j < Size; j++)
                sum -= result[j] * matr[i, j];
            if (Math.Abs(matr[i, i]) < eps)
                return false;
            result[i] = sum / matr[i, i];
        }
        return true;
    }
    public void Create()
    {
        double[,] A = new double[Size, Size + 1];
        double[] res = new double[Size];
        C = new double[Size, Size];
        for (int i = 0; i < Size; i++)
        {
            for (int j = 0; j < Size; j++)
            {
                for (int k = 0; k < Size; k++)
                    A[j, k] = ShapeCoeff(j, k);
                A[j, Size] = (i == j) ? 1.0 : 0.0;
            }
            if (!Solve(A, res))
                Console.WriteLine("Bad FE!");
            for (int j = 0; j < Size; j++)
                C[i, j] = res[j];
        }
    }
    public void Print()
    {
        for (int i = 0; i < Size; i++)
        {
            for (int j = 0; j < Size; j++)
                Console.Write("{0} ", C[i, j]);
            Console.WriteLine();
        }
    }
}
// Функції форми трикутного елемента
class ShapeTriangle : ShapeFunction
{
    public ShapeTriangle(double[,] px)
    {
        Size = 3;
        Dim = 2;
        SetCoord(Size, px);
    }
    protected override double ShapeCoeff(int i, int j)
    {
        double[] s = { 1.0, X[i, 0], X[i, 1] };
        return s[j];
    }
}
// Функції форми чотирикутного елемента
class ShapeQuadrangle : ShapeFunction
{
    public ShapeQuadrangle(double[,] px)
    {
        Size = 4;
        Dim = 2;
        SetCoord(Size, px);
    }
    protected override double ShapeCoeff(int i, int j)
    {
        double[] s = { 1.0, X[i, 0], X[i, 1], X[i, 0] * X[i, 1] };
        return s[j];
    }
}
// Функції форми тетраедра
class ShapeTetrahedron : ShapeFunction
{
    public ShapeTetrahedron(double[,] px)
    {
        Size = 4;
        Dim = 3;
        SetCoord(Size, px);
    }
    protected override double ShapeCoeff(int i, int j)
    {
        double[] s = { 1.0, X[i, 0], X[i, 1], X[i, 2] };
        return s[j];
    }
}
// Функції форми куба
class ShapeCube : ShapeFunction
{
    public ShapeCube(double[,] px)
    {
        Size = 8;
        Dim = 3;
        SetCoord(Size, px);
    }
    protected override double ShapeCoeff(int i, int j)
    {
        double[] s = { 1.0, X[i, 0], X[i, 1], X[i, 2], X[i, 0] * X[i, 1], X[i, 1] * X[i, 2], X[i, 2] * X[i, 0], X[i, 0] * X[i, 1] * X[i, 2] };
        return s[j];
    }
}

class Program
{
    static void Main(string[] args)
    {
        Console.WriteLine("РОБОТА ЗIНКОВСЬКОГО IВАНА, ОНЛАЙН ГРУПА\n");
        Console.WriteLine("Маємо наступний трикутник: { { 0, 2 }, { 2, 0 }, { 2, 2 } }\nРезультат:");
        double[,] triX = { { 0, 2 }, { 2, 0 }, { 2, 2 } }; // Координати вершин СЕ
        ShapeTriangle triFe = new ShapeTriangle(triX);
        triFe.Print();
        Console.WriteLine("\nМаємо наступний чотирикутник: { { 0, 0 }, { 2, 0 }, { 2, 2 }, { 0, 2 } } }\nРезультат:");
        double[,] quadX = { { 0, 0 }, { 2, 0 }, { 2, 2 }, { 0, 2 } }; // Координати вершин СЕ
        var quadFe = new ShapeQuadrangle(quadX);
        quadFe.Print();
        Console.WriteLine("\nМаємо наступний тетраедр: { { 0, 0, 0 }, { 2, 0, 0 }, { 2, 2, 0 }, { 0, 2, 2 } }\nРезультат:");
        double[,] tetrX = { { 0, 0, 0 }, { 2, 0, 0 }, { 2, 2, 0 }, { 0, 2, 2 } }; // Координати вершин СЕ
        var tetrFe = new ShapeTetrahedron(tetrX);
        tetrFe.Print();
        Console.WriteLine("\nМаємо наступний куб: { { 0, 0, 0 }, { 2, 0, 0 }, { 2, 2, 0 }, { 0, 2, 2 }, { 2, 3, 6 }, { 3, 6, 4 }, { 4, 2, 0 }, { 5, 3, 1 } }\nРезультат:");
        double[,] cubX = { { 0, 0, 0 }, { 2, 0, 0 }, { 2, 2, 0 }, { 0, 2, 2 }, { 2, 3, 6 }, { 3, 6, 4 }, { 4, 2, 0 }, { 5, 3, 1 } }; // Координати вершин СЕ
        var cubFe = new ShapeCube(cubX);
        cubFe.Print();
    }
}

