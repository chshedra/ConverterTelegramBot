using System;

namespace FileHandler;

public class FileSeparatingException : Exception
{
    public FileSeparatingException(string message) : base(message) { }
}
