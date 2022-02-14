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

        while (HasEmptyCells())
        {
            _attempts++;
            SetUpCells();

            var emptyCells = Cells.FindAll(cell => !cell.HasValue());
            var cell = GetBestCell(emptyCells);
            if (cell.HasAvailableValues())
            {
                _sequence.Push(cell);
                cell.ApplyFirstAvailableValue();
            }
            else
            {
                GetPreviousCellWithAnotherValue()
                    .ApplyFirstAvailableValue();
            }
        }

        _watch.Stop();
        Console.WriteLine($"Attempts: {_attempts}");
        Console.WriteLine($"Execution Time: {_watch.ElapsedMilliseconds} ms");

        ConsoleLogger.DrawTheGrid(Cells);
    }

    private bool HasEmptyCells()
    {
        return Cells.Exists(cell => cell.Value == Cell.InitialValue);
    }

    private static Cell GetBestCell(List<Cell> emptyCells)
    {
        /* get the cell with the least number of possible values */
        return emptyCells.Aggregate((a, b) => a.AvailableValuesCount() > b.AvailableValuesCount() ? b : a);
    }

    private Cell GetPreviousCellWithAnotherValue()
    {
        while (true)
        {
            /* get the last fulfilled cell with other possible values */
            var previousCell = _sequence.Peek();
            var availableValues = previousCell.GetAvailableValues().Where(v => v != previousCell.Value);
            if (availableValues.Any())
            {
                previousCell.ForbidCurrentValue();
                return previousCell;
            }

            /*
             * In this case, the cell does not have any valid values,
             * so it will be reset and removed from the sequence stack.
             */
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