# Parser & Building an Abstract Syntax Tree

### Course: Formal Languages & Finite Automata

### Author: Dobrojan Alexandru (FAF-212)

----
## Objectives:

1. Get familiar with parsing, what it is and how it can be programmed [1].
2. Get familiar with the concept of AST [2].
3. In addition to what has been done in the 3rd lab work do the following:
   1. In case you didn't have a type that denotes the possible types of tokens you need to:
      1. Have a type __*TokenType*__ (like an enum) that can be used in the lexical analysis to categorize the tokens.
      2. Please use regular expressions to identify the type of the token.
   2. Implement the necessary data structures for an AST that could be used for the text you have processed in the 3rd lab work.
   3. Implement a simple parser program that could extract the syntactic information from the input text.
   
---
## Theory

A parser is a component that analyzes the structure of a program and converts
it into an Abstract Syntax Tree (AST). The AST represents the program's syntax
and semantics in a hierarchical manner. It serves as an intermediate representation
for further analysis, transformation, and code generation. The parser applies grammar
rules to the program's tokens, constructing the AST based on the language's syntax.
The AST captures the relationships between different language constructs and is used
for static checks, optimization, and code generation.

---
## Implementation description

My program works in a form of REPL that accepts basic arithmetic expressions, 
for example 1 + 2.0 * 3 / (4 - 5.123). It accepts integers, negative numbers, floats,
operations +, -, *, / and organizing terms into groups using parenthesis.

My main program basically accepts input, invokes needed classes and methods and processes the
output, prints error for example and the graph form, the value and the lisp form.
```csharp
while (true) {
	Console.Write("\nExpression: ");
	var input = Console.ReadLine();

...

Console.ForegroundColor = ConsoleColor.Green;
Console.WriteLine(value);
Console.ResetColor();
Console.WriteLine(graph);
Console.WriteLine(lispForm);
```

Parsing is being done by the Parser class, which also acts as a basic lexer for
the input. It is a long class and uses many other helper classes.

For the lexer part is has the currentToken, currentPosition, charCount, currentChar properties
and Advance method to parse forward the string. Here is an example:
```csharp
public Parser(string? text) {
     Text       = string.IsNullOrEmpty(text) ? string.Empty : text;
     _charCount = Text.Length;
     _curToken  = Token.Token.None();

     _curPos = -1;
     Advance();
 }
```

Here are some examples of helper classes and interfaces:
- NodeVisitor, an interface that knows how to parse a given Token, it returns **object**
since it may return either an Expression type or a string for output.
```csharp
public interface INodeVisitor {
   object VisitNum(Token.Token     num);
   object VisitUnaryOp(Token.Token op, INode node);
   object VisitBinOp(Token.Token   op, INode left, INode right);
}
```
- Token, that simply represents a token.
```csharp
public class Token {
   public TokenType Type  { get; private set; }
   public string    Value { get; }
...
```
- BinOp, binary operation between 2 terms.
```csharp
...
public BinOp(Token op, Expression left, Expression right) {
		Op    = op;
		Left  = left;
		Right = right;
	}

	public override object Accept(INodeVisitor visitor) {
		return visitor.VisitBinOp(Op, Left, Right);
	}
}
```
- Num, that represents a number, that basically invokes parsing number.
```csharp
...
 public override object Accept(INodeVisitor visitor) {
     return visitor.VisitNum(Token);
 }
```


Here is an example of parsing grouping terms and operations with parenthesis,
it returns the node between parentheses and moves forward:
```csharp
 private Expression.Expression GrabBracketExpr() {
     ExpectToken(TokenType.LeftParenthesis);
     NextToken();

     Expression.Expression node = GrabExpr();

     ExpectToken(TokenType.RightParenthesis);
     NextToken();

     return node;
 }
```

## Conclusions / Screenshots / Results
```csharp
Input expression, for example 1 + 2 * 3 / (4 - 5)
Input Q to exit

Expression: 1 + 1
2
    ┌──  1
── (+)
    └──  1

(+ 1 1)
```

```csharp
Expression: 1+ 2    -3 / (4 + (1 * 2))
2.5
         ┌──  1
    ┌── (+)
    │    └──  2
── (-)
    │    ┌──  3
    └── (/)
         │    ┌──  4
         └── (+)
              │    ┌──  1
              └── (*)
                   └──  2

(- (+ 1 2) (/ 3 (+ 4 (* 1 2))))
```
```csharp
Expression: ----1.123256 * 0.00001 / (+(-((4))))
-0.00000280814
         ┌── (-)
         │    └── (-)
         │         └── (-)
         │              └── (-)
         │                   └──  1.123256
    ┌── (*)
    │    └──  0.00001
── (/)
    └── (+)
         └── (-)
              └──  4

(/ (* (- (- (- (- 1.123256)))) 0.00001) (+ (- 4)))
```

In conclusion, parsing an input string and outputting is not as hard as is 
tedious and takes a lot of time to implement dozens of classes, interfaces and methods.
It is also important to notice that there are lots of cases to be considered during parsing,
for example multiple unary operations like ----- or +++, it is important to consider that
-- is positive and ++++ do the same. Also there are cases then parenthesis do not match,
or the terms are in multiple layers of parenthesis that have no effect but require extra logic
to parse. There is also important to consider order of operations as the * and / are more important
than + and -, but not when they are in parenthesis.

Lisp form is a convenient way to represent operations both for machine and humans,
building it is not hard and it may help when there are lots of terms and operations.
