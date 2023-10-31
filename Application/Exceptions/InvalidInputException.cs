using System.Runtime.Serialization;

namespace Application.Exceptions;

[Serializable]
public class InvalidInputException : Exception
{
    public List<string> Errors { get; set; } = new();
    public InvalidInputException(List<string> errors, string message = "Dados inválidos") : base(message)
    {
        Errors = errors;
    }

    protected InvalidInputException(SerializationInfo info, StreamingContext context) : base(info, context)
    {
    }

}