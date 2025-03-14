using System;
using System.Linq;
using System.Reflection;

public static class AssemblyInfo // for some reason this isnt working so i had to hardcode the fallback values
{
    private static readonly Assembly _assembly = typeof(AssemblyInfo).Assembly;

    public static string Title => _assembly.GetName().Name ?? GetAttributeOrDefault("Title", "Overwolf Patcher");
    public static Version Version => _assembly.GetName()?.Version ?? new Version(1,0,0);
    public static string Description => GetAttributeOrDefault("Description", "Patcher for Overwolf");
    public static string Product => GetAttributeOrDefault("Product", "OverwolfPatcher");
    public static string Copyright => GetAttributeOrDefault("Copyright", "© 2025");
    public static string Trademark => GetAttributeOrDefault("Trademark", "™️");
    public static string Company => GetAttributeOrDefault("Company", "DecoderCoder, Bluscream");

    private static string GetAttributeOrDefault(string attributeName, string defaultValue)
    {
        var attribute = _assembly.GetCustomAttributes(typeof(Attribute), false).FirstOrDefault(attr => attr.GetType().Name == attributeName);
        return attribute != null ? attribute.ToString() : defaultValue;
    }
}
