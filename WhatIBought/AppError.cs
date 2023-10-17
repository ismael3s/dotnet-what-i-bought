namespace WhatIBoughtAPI;

public record AppError(string Message, int StatusCode = 400)
{
    private List<string> _Errors { get; set; } = new();
    public IReadOnlyList<String> Errors
    {
        get { return _Errors.AsReadOnly(); }
    }

    public AppError(string message, int statusCode, List<string> errors) : this(message, statusCode)
    {
        _Errors = errors;
    }

    public AppError AddError(string error)
    {
        _Errors.Add(error);
        return this;
    }
}