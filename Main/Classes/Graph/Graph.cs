using System.Text;
using Main.classes.Node;
using Main.classes.NodeVisitor;

namespace Main.classes.Graph;

public class Graph : INodeVisitor {
	private static string ReplaceLastChar(string str, char rep = ' ') {
		if (!string.IsNullOrEmpty(str)) {
			return str[..^1] + rep;
		}

		return "";
	}

	private const char   Space = ' ';
	private const char   LTurn = '┌';
	private const char   VPipe = '│';
	private const char   RTurn = '└';
	private const string Tab   = "    ";
	private const string HPipe = "──";

	private readonly StringBuilder         _sb;
	private readonly Stack<string>         _indentStack;
	private readonly Stack<BranchOriental> _orientalStack;

	public Graph() {
		_sb = new StringBuilder();

		_indentStack   = new Stack<string>();
		_orientalStack = new Stack<BranchOriental>();

		_indentStack.Push(string.Empty);
		_orientalStack.Push(BranchOriental.None);
	}

	public object VisitBinOp(Token.Token op, INode left, INode right) {
		BranchOriental legacyOriental = _orientalStack.Pop();
		string         legacyIndent   = _indentStack.Pop();

		if (legacyOriental == BranchOriental.Left) {
			_indentStack.Push(ReplaceLastChar(legacyIndent, Space) + Tab + LTurn);
			_orientalStack.Push(BranchOriental.Left);
			left.Accept(this);
		}
		else {
			_indentStack.Push(ReplaceLastChar(legacyIndent, VPipe) + Tab + VPipe);
			_orientalStack.Push(BranchOriental.Left);
			left.Accept(this);
		}

		if (legacyOriental == BranchOriental.Left) {
			_sb.AppendLine(ReplaceLastChar(legacyIndent, LTurn) + HPipe + " (" + op.ToString() + ")");
		}
		else {
			_sb.AppendLine(ReplaceLastChar(legacyIndent, RTurn) + HPipe + " (" + op.ToString() + ")");
		}

		if (legacyOriental == BranchOriental.Right) {
			_indentStack.Push(ReplaceLastChar(legacyIndent, Space) + Tab + RTurn);
			_orientalStack.Push(BranchOriental.Right);
			right.Accept(this);
		}
		else {
			_indentStack.Push(ReplaceLastChar(legacyIndent, VPipe) + Tab + VPipe);
			_orientalStack.Push(BranchOriental.Right);
			right.Accept(this);
		}

		return _sb.ToString();
	}

	public object VisitNum(Token.Token num) {
		BranchOriental legacyOriental = _orientalStack.Pop();
		string         legacyIndent   = _indentStack.Pop();

		if (legacyOriental == BranchOriental.Left) {
			_sb.AppendLine(ReplaceLastChar(legacyIndent, LTurn) + HPipe + "  " + num);
		}
		else {
			_sb.AppendLine(ReplaceLastChar(legacyIndent, RTurn) + HPipe + "  " + num);
		}

		return _sb.ToString();
	}

	public object VisitUnaryOp(Token.Token op, INode node) {
		BranchOriental legacyOriental = _orientalStack.Pop();
		string         legacyIndent   = _indentStack.Pop();

		if (legacyOriental == BranchOriental.Left) {
			_sb.AppendLine($"{ReplaceLastChar(legacyIndent, LTurn)}{HPipe} ({op})");
		}
		else {
			_sb.AppendLine($"{ReplaceLastChar(legacyIndent, RTurn)}{HPipe} ({op})");
		}

		if (legacyOriental == BranchOriental.Right) {
			_indentStack.Push(ReplaceLastChar(legacyIndent, Space) + Tab + RTurn);
			_orientalStack.Push(BranchOriental.Right);
			node.Accept(this);
		}
		else {
			_indentStack.Push(ReplaceLastChar(legacyIndent, VPipe) + Tab + RTurn);
			_orientalStack.Push(BranchOriental.Right);
			node.Accept(this);
		}

		return _sb.ToString();
	}

	private enum BranchOriental {
		None,
		Left,
		Right
	}
}
