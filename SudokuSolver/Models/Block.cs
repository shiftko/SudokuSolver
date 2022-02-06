using SudokuSolver.Models.Abstract;

namespace SudokuSolver.Models;

internal class Block : CellsContainer
{
    public HashSet<string> GetUsedValuesFromAdjacentCells(int xCoord, int yCoord)
    {
        HashSet<string> possibleValues = new(Cell.PossibleValues);
        foreach (var cell in GetEmptyCells())
        {
            if (!(cell.XCoord == xCoord && cell.YCoord == yCoord))
            {
                possibleValues.IntersectWith(cell.GetForbiddenValues());
            }
        }

        possibleValues.ExceptWith(GetRefCellsValues());
        return possibleValues;
    }
}