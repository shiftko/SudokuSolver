using System.Text.RegularExpressions;
using SudokuSolver.Models;

const int containerCapacity = 9;
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
var cellsValues = condition.Trim().Split();

if (cellsValues.Length != numberOfCells)
{
    throw new Exception("Invalid number of cells values!");
}

var cells = new List<Cell>
{
    Capacity = numberOfCells
};
var xLines = new List<XLine>
{
    Capacity = containerCapacity
};
var yLines = new List<YLine>
{
    Capacity = containerCapacity
};
var blocks = new List<Block>
{
    Capacity = containerCapacity
};

for (var i = containerCapacity; i > 0; i--)
{
    xLines.Add(new XLine());
    yLines.Add(new YLine());
    blocks.Add(new Block());
}

for (var y = 0; y < 9; y++)
{
    for (var x = 0; x < 9; x++)
    {
        var xLine = xLines[x];
        var yLine = yLines[y];
        var block = blocks[y / 3 * 3 + x / 3];
        var cell = new Cell()
        {
            XCoord = x,
            YCoord = y,
            RefXLine = xLine,
            RefYLine = yLine,
            RefBlock = block,
            Value = cellsValues[y * 9 + x]
        };
        xLine.RefCells.Add(cell);
        yLine.RefCells.Add(cell);
        block.RefCells.Add(cell);
        cells.Add(cell);
    }
}

var executor = new Executor
{
    Cells = cells
};

executor.Run();