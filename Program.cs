
int size = 5;
Node[][] nodes;
DisplayMode displayMode = DisplayMode.Colored;
int minValue = 0;
int maxValue = 10;
Random rnd = new();

if (args.Length > 0 && args[0] == "-help")
{
	Console.WriteLine("-nodisplay          Run program without displaying the triangle");
	Console.WriteLine("-nocolor            Display the optimal path only with lines");
	Console.WriteLine("-nopath             Display the triangle without a path on it");
	Console.WriteLine("-size size          Size of the triangle");
	Console.WriteLine("-range max|min-max  Bounds for values of the triangle");
	Console.WriteLine("-help               Show this info");
	Console.WriteLine();
	Console.WriteLine("Note: display options are mutually exclusive, the last one will override any other");
	return;
}

// parse command line arguments
// errors are the only output to the console during this step
Console.ForegroundColor = ConsoleColor.Red;
for (int i = 0; i < args.Length; i++)
{
	switch (args[i])
	{
		case "-nodisplay":
			displayMode = DisplayMode.Disabled;
			break;
		case "-nocolor":
			displayMode = DisplayMode.Lines;
			break;
		case "-nopath":
			displayMode = DisplayMode.NoPath;
			break;
		case "-size":
			if (args.Length < i + 2)
			{
				Console.WriteLine("Expected a value after -size, skipping");
				break;
			}
			string sizeStr = args[i + 1];
			i += 1;
			if (!int.TryParse(sizeStr, out size))
			{
				Console.WriteLine("-size must be an integer, skipping");
				size = 5;
				break;
			}
			if (size < 1)
			{
				Console.WriteLine("-size must be greater than 0, skipping");
				size = 5;
			}
			break;
		case "-range":
			if (args.Length < i + 2)
			{
				Console.WriteLine("Expected a value after -range, skipping");
				break;
			}
			string[] rangeItems = args[i + 1].Split('-');
			i += 1;
			if (rangeItems.Length > 0 && rangeItems.Length <= 2)
			{
				if (!int.TryParse(rangeItems[0], out minValue))
				{
					Console.WriteLine("-range: Min value was not an integer, skipping");
					maxValue = 10;
					break;
				}
				if (rangeItems.Length == 2)
				{
					if (!int.TryParse(rangeItems[1], out maxValue))
					{
						Console.WriteLine("-range: Max value was not an integer, skipping");
						minValue = 0;
						maxValue = 10;
						break;
					}
				}
			}
			else
			{
				Console.WriteLine("-range must be followed by max or min-max");
			}
			break;
		default:
			Console.WriteLine($"Unknown option '{args[i]}', skipping");
			Console.WriteLine("Use -help to get available commands");
			break;
	}
}
Console.ForegroundColor = ConsoleColor.White;

//// parse triangle size from args (if provided)
//if (args.Length == 0)
//{
//	size = 5;
//}
//else if (args.Length > 1)
//{
//	Console.WriteLine("expected only 1 argument");
//	return;
//}
//else if (!int.TryParse(args[0], out size))
//{
//	Console.WriteLine("arg 1 must be a number");
//	return;
//}
//if (size <= 0)
//{
//	Console.WriteLine("arg 1 must be greater than 0");
//	return;
//}

// initialize arrays with random numbers
nodes = new Node[size][];
for(int i = 0; i < size; i++)
{
	nodes[i] = new Node[i + 1];
	for (int j = 0; j < i + 1; j++)
		nodes[i][j] = new(rnd.Next() % (maxValue - minValue) + minValue);
}

// find maximum possible sum for each node, bottom to top
var stime = DateTime.Now;
for(int i = size - 2; i >= 0; i--)
{
	for (int j = 0; j < i + 1; j++)
	{
		nodes[i][j].MaxSum += Math.Max(nodes[i+1][j].MaxSum, nodes[i+1][j+1].MaxSum);
	}
}
var etime = DateTime.Now;

// find the path to get the best sum
int[] path = new int[size];
path[0] = 0;
for (int i = 1; i < size; i++)
{
	int prev = path[i - 1];
	if (nodes[i][prev].MaxSum > nodes[i][prev + 1].MaxSum)
		path[i] = prev;
	else path[i] = prev + 1;
}

Console.WriteLine();
if (displayMode != DisplayMode.Disabled)
{
	// display the triangle
	for (int i = 0; i < size - 1; i++)
	{
		// space before the numbers line
		for (int j = 0; j < (size - i - 1) * 2; j++)
			Console.Write(" ");

		// numbers with 3 spaces between
		for (int j = 0; j < i + 1; j++)
		{
			SetNumberHighlight(path[i] == j);
			Console.Write($"{nodes[i][j].Value}   ");
		}

		Console.WriteLine();

		// space before the connections line
		for (int j = 0; j < (size - i - 1) * 2 - 1; j++)
			Console.Write(" ");
		for (int j = 0; j < i + 1; j++)
		{
			SetLineHighLight(path[i + 1] == j && path[i] == j);
			Console.Write("/ ");
			SetLineHighLight(path[i + 1] == j + 1 && path[i] == j);
			Console.Write("\\ ");
		}
		Console.WriteLine();
	}
	// last row
	for (int j = 0; j < size; j++)
	{
		SetNumberHighlight(path[size - 1] == j);
		Console.Write($"{nodes[size - 1][j].Value}   ");
	}
	SetConsoleHighlight(false);
	Console.WriteLine();
}

// display the results
Console.WriteLine($"Maximum possible sum in the triangle: {nodes[0][0].MaxSum}");
Console.WriteLine($"Completed in {(etime - stime).TotalMilliseconds}ms");

void SetNumberHighlight(bool doHighlight)
{
	if (displayMode == DisplayMode.Colored)
		SetConsoleHighlight(doHighlight);
	else
		Console.ForegroundColor = ConsoleColor.White;
}
void SetLineHighLight(bool doHighlight)
{
	if (displayMode == DisplayMode.Colored)
		SetConsoleHighlight(doHighlight);
	else if (displayMode == DisplayMode.Lines)
		Console.ForegroundColor = doHighlight ? ConsoleColor.White : ConsoleColor.Black;
}
void SetConsoleHighlight(bool doHighlight) => Console.ForegroundColor = doHighlight ? ConsoleColor.Green : ConsoleColor.White;

enum DisplayMode { Colored, Lines, NoPath, Disabled }
struct Node
{
	public int Value = 0;
	public int MaxSum = 0;

	public Node() { }
	public Node(int value) { Value = value; MaxSum = value; }
}
