using System.Diagnostics;

namespace SudokuSolver.Models;

internal class Executor
{
    public List<Cell>? Cells { get; init; }
    public List<XLine>? XLines { get; set; }
    public List<YLine>? YLines { get; set; }
    public List<Block>? Blocks { get; set; }

    private readonly Stack<Cell> _sequence = new();
    private readonly Stopwatch _watch = new();
    private int _attempts = 0;

    public async void Run()
    {
        _watch.Start();
        PreSetUpCells();

        do
        {
            _attempts++;
            var emptyCells = Cells.FindAll(cell => !cell.HasValue());
            var cell = emptyCells.Find(cell =>
                cell.AvailableValuesCount() == emptyCells.Min(c => c.AvailableValuesCount()));

            if (cell.AvailableValuesCount() == 0)
            {
                ApplyValue(CheckPreviousCell());
            }
            else
            {
                _sequence.Push(cell);
                ApplyValue(cell);
            }
        } while (IsNotReady());

        _watch.Stop();
        Console.WriteLine($"Attempts: {++_attempts}");
        Console.WriteLine($"Execution Time: {_watch.ElapsedMilliseconds} ms");
        DrawToConsole();
    }

    private void ApplyValue(Cell cell)
    {
        var value = cell.GetFirstAvailableValue();
        cell.Value = value;
        // cell.RefXLine?.ForbidValueForRefCells(value);
        // cell.RefYLine?.ForbidValueForRefCells(value);
        // cell.RefBlock?.ForbidValueForRefCells(value);
        PreSetUpCells();
        // Console.WriteLine($@"cell: {cell.XCoord} : {cell.YCoord} : {cell.Value}");
    }

    private Cell CheckPreviousCell()
    {
        /* step back and try other value or other cell */
        var previousCell = _sequence.Peek();
        var availableValues =
            new List<string>(previousCell.GetAvailableValues()
                .FindAll(option => option != previousCell.Value));
        if (availableValues.Count == 0)
        {
            previousCell.Reset();
            _sequence.Pop();
            return CheckPreviousCell();
        }

        previousCell.ForbidCurrentValue();
        return previousCell;
    }

    private void PreSetUpCells()
    {
        foreach (var cell in Cells)
        {
            if (!cell.HasValue())
            {
                cell.Setup();
            }
        }
    }

    private void DrawToConsole()
    {
        for (var y = 0; y < 9; y++)
        {
            var delimiter = (y + 1) % 3 == 0
                ? Environment.NewLine + Environment.NewLine
                : Environment.NewLine;
            for (var x = 0; x < 9; x++)
            {
                var ident = (x + 1) % 3 == 0 ? "  " : " ";
                Console.Write($"{Cells[y * 9 + x].Value}{ident}");
            }

            Console.Write(delimiter);
        }

        Console.WriteLine();
    }

    private bool IsNotReady()
    {
        return Cells.Exists(cell => cell.Value == "0");
    }
}