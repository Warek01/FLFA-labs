﻿using System.Runtime.Serialization;

namespace Main.Classes.Exceptions.InvalidSyntax;

[Serializable]
public class InvalidSyntaxException : Exception {
	public InvalidSyntaxException() { }

	public InvalidSyntaxException(string message) : base(message) { }

	public InvalidSyntaxException(string message, Exception innerException) :
		base(message, innerException) { }

	protected InvalidSyntaxException(SerializationInfo info, StreamingContext context) :
		base(info, context) { }
}
