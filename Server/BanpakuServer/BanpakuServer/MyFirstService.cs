using App.Reversi.Shared;
using MagicOnion;
using MagicOnion.Server;

public class MyFirstService : ServiceBase<IMyFirstService>, IMyFirstService
{
    public async UnaryResult<int> SumAsync(int x, int y)
    {
        Console.WriteLine($"Called SumAsync - x:{x} y:{y}");
        return x + y;
    }
}