using NUnit.Framework;
using PickleP2P.Core.Mods.ObjLoader;
using System.IO;

public class ObjParserTests
{
    [Test]
    public void ParseTinyQuad()
    {
        string tmp = Path.GetTempFileName();
        File.WriteAllText(tmp, "v 0 0 0\nv 1 0 0\nv 1 1 0\nv 0 1 0\nvt 0 0\nvt 1 0\nvt 1 1\nvt 0 1\nvn 0 0 1\nf 1/1/1 2/2/1 3/3/1 4/4/1\n");
        var mesh = ObjParser.Parse(tmp);
        Assert.Greater(mesh.positions.Count, 0);
        Assert.Greater(mesh.triangles.Count, 0);
        File.Delete(tmp);
    }
}

