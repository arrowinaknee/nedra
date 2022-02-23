﻿
int size;
Node[][] nodes;
Random rnd = new();

// parse triangle size from args
if(args.Length != 1)
{
	Console.WriteLine("expected 1 argument");
	return;
}
if (!int.TryParse(args[0], out size))
{
	Console.WriteLine("arg 1 must be a number");
	return;
}
if (size <= 0)
{
	Console.WriteLine("arg 1 must be greater than 0");
	return;
}

// initialize arrays with random numbers
nodes = new Node[size][];
for(int i = 0; i < size; i++)
{
	nodes[i] = new Node[i + 1];
	for (int j = 0; j < i + 1; j++)
		nodes[i][j] = new(rnd.Next() % 10);
}

// display the triangle
for (int i = 0; i < size - 1; i++)
{
	// space before the numbers line
	for (int j = 0; j < (size - i - 1) * 2; j++)
		Console.Write(" ");

	// numbers with 3 spaces between
	for (int j = 0; j < i + 1; j++)
		Console.Write($"{nodes[i][j].Value}   ");

	Console.WriteLine();

	// space before the connections line
	for (int j = 0; j < (size - i - 1) * 2 - 1; j++)
		Console.Write(" ");
	for (int j = 0; j < i + 1; j++)
		Console.Write("/ \\ ");
	Console.WriteLine();
}
for (int j = 0; j < size; j++)
	Console.Write($"{nodes[size-1][j].Value}   ");
Console.WriteLine();

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

// display the results
Console.WriteLine($"Maximum possible sum in the triangle: {nodes[0][0].MaxSum}");
Console.WriteLine($"Completed in {(etime - stime).TotalMilliseconds}ms");

struct Node
{
	public int Value = 0;
	public int MaxSum = 0;

	public Node() { }
	public Node(int value) { Value = value; MaxSum = value; }
}