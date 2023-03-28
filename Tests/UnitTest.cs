using System;
using System.Collections.Generic;
using Main;
using Main.classes.Lexer;
using Main.classes.Token;
using Xunit;
using Xunit.Abstractions;

namespace Tests;

public class UnitTest {
	private readonly ITestOutputHelper _testOutputHelper;
	public UnitTest(ITestOutputHelper testOutputHelper) {
		_testOutputHelper = testOutputHelper;
	}

	[Fact]
	private void TestInput() {
		string input = @"
			let a = 123;
			func test () {}
		";

		KeyValuePair<string, string>[] expectedTokens = {
			new KeyValuePair<string, string>(TokenType.Let, "let"),
			new KeyValuePair<string, string>(TokenType.Ident, "a"),
			new KeyValuePair<string, string>(TokenType.Assign, "="),
			new KeyValuePair<string, string>(TokenType.Int, "123"),
			new KeyValuePair<string, string>(TokenType.Semicolon, ";"),
			new KeyValuePair<string, string>(TokenType.Ident, "func"),
			new KeyValuePair<string, string>(TokenType.Ident, "test"),
			new KeyValuePair<string, string>(TokenType.Lparen, "("),
			new KeyValuePair<string, string>(TokenType.Rparen, ")"),
			new KeyValuePair<string, string>(TokenType.Lbrace, "{"),
			new KeyValuePair<string, string>(TokenType.Rbrace, "}"),
			new KeyValuePair<string, string>(TokenType.Eof, ""),
		};
		
		Lexer lexer = new(input);

		Assert.NotNull(lexer);

		int i = 0;

		while (i < expectedTokens.Length) {
			Token token = lexer.NextToken();
			
			_testOutputHelper.WriteLine($"{token.Type} {token.Literal}");
			Assert.Equal(token.Type, expectedTokens[i].Key);
			Assert.Equal(token.Literal, expectedTokens[i].Value);

			i++;
		}
	}
}
