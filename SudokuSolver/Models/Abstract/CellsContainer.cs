namespace SudokuSolver.Models.Abstract;

internal abstract class CellsContainer
{
    public List<Cell> RefCells { get; } = new();

    public IEnumerable<string> GetValues()
    {
        return from cell in RefCells
            where cell.Value != Cell.InitialValue
            select cell.Value;
    }

    public void ForbidValueForRefCells(string value)
    {
        foreach (var refCell in RefCells)
        {
            refCell.RemoveAvailableValue(value);
        }
    }
}