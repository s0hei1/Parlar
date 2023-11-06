namespace ParlarTest.Extentions;

public class NotFoundException : Exception
{
    public NotFoundException(string? message) : base(message)
    {
    }
}