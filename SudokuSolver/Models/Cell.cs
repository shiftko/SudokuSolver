namespace SudokuSolver.Models;

internal class Cell
{
    public const string InitialValue = "0";

    public static readonly List<string> PossibleValues = new() {"1", "2", "3", "4", "5", "6", "7", "8", "9"};

    public int XCoord { get; init; }
    public int YCoord { get; init; }

    public string Value { get; set; } = InitialValue;
    public XLine RefXLine { get; init; } = new();
    public YLine RefYLine { get; init; } = new();
    public Block RefBlock { get; init; } = new();

    private List<string> AvailableValues { get; set; } = new();

    public void Setup()
    {
        // using a HashSet increases performance drastically
        var forbiddenValues = GetForbiddenValues();

        // using this logic increases performance drastically --> 23ms
        var usedValuesFromAdjacentLines = RefBlock.GetUsedValuesFromAdjacentCells(XCoord, YCoord);

        if (usedValuesFromAdjacentLines.Any())
        {
            AvailableValues = PossibleValues
                .FindAll(pv => !forbiddenValues.Contains(pv))
                .FindAll(pv => usedValuesFromAdjacentLines.Contains(pv));
        }
        else
        {
            AvailableValues = PossibleValues.FindAll(pv => !forbiddenValues.Contains(pv));
        }
    }

    public void Reset()
    {
        AvailableValues = new List<string>();
        Value = InitialValue;
    }

    public bool HasValue()
    {
        return Value != InitialValue;
    }

    public bool HasAvailableValues()
    {
        return AvailableValues.Count > 0;
    }

    public int AvailableValuesCount()
    {
        return AvailableValues.Count;
    }

    public void ApplyFirstAvailableValue()
    {
        Value = AvailableValues.First();
    }

    public List<string> GetAvailableValues()
    {
        return AvailableValues;
    }

    public void ForbidCurrentValue()
    {
        AvailableValues.Remove(Value);
    }

    public HashSet<string> GetForbiddenValues()
    {
        HashSet<string> forbiddenValues = new();
        forbiddenValues.UnionWith(RefXLine.GetRefCellsValues());
        forbiddenValues.UnionWith(RefYLine.GetRefCellsValues());
        forbiddenValues.UnionWith(RefBlock.GetRefCellsValues());
        return forbiddenValues;
    }
}