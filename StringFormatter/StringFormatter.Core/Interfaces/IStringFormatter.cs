namespace StringFormatter.Core.Interfaces;

public interface IStringFormatter
{
    // String interpolation for target object
    string Format(string template, object target);
}
