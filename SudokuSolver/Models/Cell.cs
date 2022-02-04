namespace SudokuSolver.Models;

internal class Cell
{
    public const string InitialValue = "0";

    private readonly List<string> _possibleValues = new() {"1", "2", "3", "4", "5", "6", "7", "8", "9"};

    public string Value { get; set; } = InitialValue;
    public XLine RefXLine { get; init; } = new();
    public YLine RefYLine { get; init; } = new();
    public Block RefBlock { get; init; } = new();

    private List<string> AvailableValues { get; set; } = new();

    public void Setup()
    {
        var valuesByX = RefXLine.GetValues();
        var valuesByY = RefYLine.GetValues();
        var valuesByBlock = RefBlock.GetValues();
        var forbiddenValues = valuesByX.Concat(valuesByY.Concat(valuesByBlock)).ToList();

        AvailableValues = _possibleValues.FindAll(pv => !forbiddenValues.Exists(fv => fv == pv));
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

    public int AvailableValuesCount()
    {
        return AvailableValues.Count;
    }

    public string GetFirstAvailableValue()
    {
        return AvailableValues.First();
    }

    public List<string> GetAvailableValues()
    {
        return AvailableValues;
    }

    public void RemoveAvailableValue(string value)
    {
        if (!HasValue())
        {
            AvailableValues.Remove(value);
        }
    }

    public void ForbidCurrentValue()
    {
        AvailableValues.Remove(Value);
    }
}