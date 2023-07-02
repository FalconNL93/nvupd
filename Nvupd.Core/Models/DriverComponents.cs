namespace Nvupd.Core.Models;

public enum Components
{
    Driver
}

public class DriverComponents
{
    public HashSet<Components> Components { get; } = new();
    public DriverComponents(IEnumerable<Components> components)
    {
        foreach (var component in components)
        {
            Components.Add(component);
        }
    }
}