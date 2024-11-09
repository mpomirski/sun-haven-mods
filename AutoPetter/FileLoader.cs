// From https://github.com/Morthy/sunhaven-mods/tree/main/Morthy.Util
using System;
using System.IO;
using System.Linq;
using System.Reflection;

namespace AutoPetter;

public class FileLoader {
    private static string GetResourcePath(Assembly assembly, string name) {
        var resourcePath = name;
        if (!name.StartsWith(assembly.GetName().Name))
            resourcePath = assembly.GetManifestResourceNames()
                .Single(str => str.EndsWith(name));

        return resourcePath;
    }

    public static byte[] LoadFileBytes(Assembly assembly, string name) {
        using var stream = assembly.GetManifestResourceStream(GetResourcePath(assembly, name));
        using var reader = new StreamReader(stream ?? throw new InvalidOperationException());
        using var memoryStream = new MemoryStream();
        reader.BaseStream.CopyTo(memoryStream);
        var bytes = memoryStream.ToArray();

        return bytes;
    }
}