using Main.classes.Node;
using Main.classes.NodeVisitor;

namespace Main.classes.Expression; 

public abstract class Expression : INode {
	public abstract object Accept(INodeVisitor visitor);
}
