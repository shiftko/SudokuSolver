using System.Diagnostics;
using System.Text.RegularExpressions;

const int numberOfBlocks = 9;
const int numberOfXLines = 9;
const int numberOfYLines = 9;
const int numberOfCells = 81;
var condition = @"
    0, 0, 0,  0, 0, 0,  0, 1, 4,
    0, 0, 0,  0, 0, 3,  0, 8, 5,
    8, 0, 1,  0, 2, 0,  0, 0, 7,

    0, 0, 0,  5, 0, 7,  0, 0, 0,
    0, 0, 4,  0, 0, 0,  1, 0, 0,
    0, 9, 0,  0, 0, 0,  0, 0, 0,

    5, 0, 0,  0, 0, 0,  0, 7, 2,
    0, 0, 2,  0, 1, 0,  0, 0, 0,
    0, 0, 0,  0, 4, 0,  0, 0, 1,
";

condition = Regex.Replace(condition, @"\D+", " ");
var cellsValues = condition.Trim().Split().ToList();

if (cellsValues.Count != numberOfCells)
{
    throw new Exception("Invalid number of cells values!");
}

var cells = new List<Cell>
{
    Capacity = numberOfCells
};
var xLines = new List<XLine>
{
    Capacity = numberOfXLines
};
var yLines = new List<YLine>
{
    Capacity = numberOfYLines
};
var blocks = new List<Block>
{
    Capacity = numberOfBlocks
};

for (var x = 0; x < numberOfBlocks; x++)
{
    blocks.Add(new Block());
}

for (var x = 0; x < numberOfXLines; x++)
{
    xLines.Add(new XLine());
}

for (var x = 0; x < numberOfYLines; x++)
{
    yLines.Add(new YLine());
}

for (var y = 0; y < 9; y++)
{
    for (var x = 0; x < 9; x++)
    {
        var block = blocks[y / 3 * 3 + x / 3];
        var xLine = xLines[x];
        var yLine = yLines[y];
        var cell = new Cell()
        {
            XCoord = x + 1,
            YCoord = y + 1,
            RefXLine = xLine,
            RefYLine = yLine,
            RefBlock = block,
            Value = cellsValues[y * 9 + x]
        };
        cells.Add(cell);
        xLine.RefCells.Add(cell);
        yLine.RefCells.Add(cell);
        block.RefCells.Add(cell);
    }
}

/* start analysis */
var executor = new LogicExecutor
{
    Cells = cells,
    XLines = xLines,
    YLines = yLines,
    Blocks = blocks
};

executor.Run();
/* end analysis */

/* service entities */
internal class LogicExecutor
{
    public List<Cell> Cells { get; set; }
    public List<XLine> XLines { get; set; }
    public List<YLine> YLines { get; set; }
    public List<Block> Blocks { get; set; }

    public void Run()
    {
        foreach (var cell in Cells)
        {
            Console.WriteLine($@"cell: {cell.XCoord} : {cell.YCoord} : {cell.Value}");
            cell.PreSetup();
        }
    }
}
/* service entities end */

/* domain entities */
internal class Cell
{
    private static List<string> PossibleValues { get; } =
        new List<string> {"1", "2", "3", "4", "5", "6", "7", "8", "9"}.AsReadOnly().ToList();

    private List<string> AvailableValues { get; set; }

    public int XCoord { get; set; }
    public int YCoord { get; set; }
    public string Value { get; set; } = "0";
    public XLine? RefXLine { get; set; }
    public YLine? RefYLine { get; set; }
    public Block? RefBlock { get; set; }

    public void PreSetup()
    {
        Debug.Assert(RefXLine != null, nameof(RefXLine) + " != null");
        var valuesByX = RefXLine.GetValues();
        Debug.Assert(RefYLine != null, nameof(RefYLine) + " != null");
        var valuesByY = RefYLine.GetValues();
        Debug.Assert(RefBlock != null, nameof(RefBlock) + " != null");
        var valuesByBlock = RefBlock.GetValues();

        var forbiddenValues =
            new List<string>(valuesByX.Concat(valuesByY.FindAll(i => true).Concat(valuesByBlock.FindAll(i => true))));

        AvailableValues = new List<string>(PossibleValues.FindAll(i => !forbiddenValues.Exists(fv => fv == i)));
    }

    private bool IsValid()
    {
        return RefXLine != null && RefYLine != null && RefBlock != null;
    }
}

internal class CellsContainer
{
    public List<Cell> RefCells { get; } = new();

    public List<string> GetValues()
    {
        return new List<string>(from cell in RefCells
            where cell.Value != "0"
            select cell.Value);
    }
}

internal class XLine : CellsContainer
{
}

internal class YLine : CellsContainer
{
}

internal class Block : CellsContainer
{
}
/* domain entities end */