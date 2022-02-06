using System.Diagnostics;
using SudokuSolver.Helpers;

namespace SudokuSolver.Models;

internal class Executor
{
    public List<Cell> Cells { get; init; } = new();

    private readonly Stack<Cell> _sequence = new();
    private readonly Stopwatch _watch = new();
    private int _attempts;

    public void Run()
    {
        _watch.Start();

        SetUpCells();

        while (HasEmptyCells())
        {
            _attempts++;

            var emptyCells = Cells.FindAll(cell => !cell.HasValue());
            var cell = GetBestCell(emptyCells);
            if (cell.HasAvailableValues())
            {
                _sequence.Push(cell);
                ApplyValue(cell);
            }
            else
            {
                ApplyValue(CheckPreviousCell());
            }
        }

        _watch.Stop();
        Console.WriteLine($"Attempts: {_attempts}");
        Console.WriteLine($"Execution Time: {_watch.ElapsedMilliseconds} ms");

        ConsoleLogger.ShowGrid(Cells);
    }

    private bool HasEmptyCells()
    {
        return Cells.Exists(cell => cell.Value == Cell.InitialValue);
    }

    private static Cell GetBestCell(List<Cell> emptyCells)
    {
        return emptyCells.Aggregate((a, b) => a.AvailableValuesCount() > b.AvailableValuesCount() ? b : a);
    }

    private void ApplyValue(Cell cell)
    {
        cell.ApplyFirstAvailableValue();
        SetUpCells();
    }

    private Cell CheckPreviousCell()
    {
        while (true)
        {
            /* step back and try other value or other cell */
            var previousCell = _sequence.Peek();
            var availableValues = previousCell.GetAvailableValues().Where(v => v != previousCell.Value);
            if (availableValues.Any())
            {
                previousCell.ForbidCurrentValue();
                return previousCell;
            }

            previousCell.Reset();
            _sequence.Pop();
        }
    }

    private void SetUpCells()
    {
        foreach (var cell in Cells.Where(cell => !cell.HasValue()))
        {
            cell.Setup();
        }
    }
}