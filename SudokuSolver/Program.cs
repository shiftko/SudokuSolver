using System.Text.RegularExpressions;
using SudokuSolver.Models;

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

for (var i = 0; i < numberOfBlocks; i++)
{
    blocks.Add(new Block());
}

for (var i = 0; i < numberOfXLines; i++)
{
    xLines.Add(new XLine());
}

for (var i = 0; i < numberOfYLines; i++)
{
    yLines.Add(new YLine());
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

var executor = new Executor
{
    Cells = cells,
    XLines = xLines,
    YLines = yLines,
    Blocks = blocks
};

executor.Run();