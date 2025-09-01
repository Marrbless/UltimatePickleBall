using NUnit.Framework;
using PickleP2P.Core.Voice.Codec;

public class G711Tests
{
    [Test]
    public void Roundtrip_Zero()
    {
        short[] pcm = new short[160];
        var enc = G711MuLaw.Encode(pcm, pcm.Length);
        short[] dec = new short[pcm.Length];
        G711MuLaw.Decode(enc, enc.Length, dec);
        for (int i = 0; i < pcm.Length; i++) Assert.AreEqual(0, dec[i]);
    }
}

