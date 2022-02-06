namespace SudokuSolver.Models.Abstract;

internal abstract class CellsContainer
{
    public List<Cell> RefCells { get; } = new();

    public IEnumerable<string> GetRefCellsValues()
    {
        return from cell in RefCells
            where cell.Value != Cell.InitialValue
            select cell.Value;
    }

    public IEnumerable<Cell> GetEmptyCells()
    {
        return from cell in RefCells
            where cell.Value == Cell.InitialValue
            select cell;
    }

    public void ForbidValueForRefCells(string value)
    {
        foreach (var refCell in RefCells)
        {
            refCell.RemoveAvailableValue(value);
        }
    }
}