namespace Nexus.Core;

public static class Properties
{
    /// <summary>
    /// A generic setter helper. The analyzer identifies it as a setter because
    /// one of its parameters has the [PropertyValue] attribute.
    /// </summary>
    public static void Set<T>(
        INexus target,
        [LinkedPropertyEnum(nameof(target))] Enum e,
        [PropertyValue] T value)
    {
        target._setValue(e, value);
    }

    [PropertyGetter]
    public static T Get<T>(
        INexus target,
        [LinkedPropertyEnum(nameof(target))] Enum e)
    {
        Console.WriteLine($"Getting generic value for {e} from {target.GetType().Name}");
        return (T)target._getValue(e);
    }
    
    [PropertyGetter]
    public static void Get<T>(
        INexus target,
        [LinkedPropertyEnum(nameof(target))] Enum e, ref T returnValue)
    {
        Console.WriteLine($"Getting generic value for {e} from {target.GetType().Name}");
        returnValue = (T)target._getValue(e);
    }
}