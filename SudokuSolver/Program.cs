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
if (cellsValues.Count != numberOfCells) throw new Exception("Invalid number of cells values!");
var cells = new List<Cell>
{
    Capacity = numberOfCells
};
var xLines = new List<Line>
{
    Capacity = numberOfXLines
};
var yLines = new List<Line>
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
            RefYline = yLine,
            RefBlock = block,
            Value = cellsValues[y * 9 + x]
        };
        cells.Add(cell);
        xLine.refCells.Add(cell);
        yLine.refCells.Add(cell);
        block.refCells.Add(cell);
    }
}

foreach (var cell in cells)
{
    Console.WriteLine($@"cell: {cell.XCoord} : {cell.YCoord} : {cell.Value} ");
}

internal class Cell
{
    public int XCoord { get; set; }
    public int YCoord { get; set; }
    public Line RefXLine { get; set; }
    public Line RefYline { get; set; }
    public Block RefBlock { get; set; }
    public string Value { get; set; } = "0";
}

internal class Line
{
    public List<Cell> refCells { get; set; } = new();
}

internal class XLine : Line
{
}

internal class YLine : Line
{
}

internal class Block
{
    public List<Cell> refCells { get; set; } = new();
}