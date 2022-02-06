using SudokuSolver.Models;

namespace SudokuSolver.Helpers;

internal static class ConsoleLogger
{
    public static void ShowGrid(List<Cell> cells)
    {
        for (var y = 0; y < 9; y++)
        {
            var delimiter = (y + 1) % 3 == 0
                ? $"{Environment.NewLine}{Environment.NewLine}"
                : Environment.NewLine;
            for (var x = 0; x < 9; x++)
            {
                var ident = (x + 1) % 3 == 0 ? "  " : " ";
                Console.Write($"{cells[y * 9 + x].Value}{ident}");
            }

            Console.Write(delimiter);
        }
    }

    public static void LogToConsole(Cell cell)
    {
        Console.WriteLine($@"cell: {cell.XCoord} : {cell.YCoord} : {cell.Value}");
    }
}