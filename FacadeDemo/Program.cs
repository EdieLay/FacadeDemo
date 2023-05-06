// See https://aka.ms/new-console-template for more information

MegaPlugin plugin = new();
Console.WriteLine("Default preset: ");
plugin.PrintSettings();

plugin.NiceVocalPreset();
Console.WriteLine("NiceVocal preset: ");
plugin.PrintSettings();

plugin.RapVocalPreset();
Console.WriteLine("RapVocal preset: ");
plugin.PrintSettings();

plugin.CosmicGuitarPreset();
Console.WriteLine("CosmicGuitar preset: ");
plugin.PrintSettings();

plugin.DrumBusPreset();
Console.WriteLine("DrumBus preset: ");
plugin.PrintSettings();


enum EqModes
{
    Bell,
    LowShelf,
    LowCut,
    HighShelf,
    HighCut,
    Notch,
    BandPass,
    TiltShelf,
    FlatTilt
}

enum ReverbModes
{
    Hall,
    Room,
    Plate,
    Ambience
}

interface IPlugin
{
    void PrintSettings();
}

class MegaPlugin : IPlugin
{
    public Equalizer eq;
    public Compressor comp;
    public Reverb rev;

    public MegaPlugin(Equalizer eq, Compressor comp, Reverb rev)
    {
        this.eq = eq;
        this.comp = comp;
        this.rev = rev;
    }
    public MegaPlugin()
    {
        eq = new Equalizer();
        comp = new Compressor();
        rev = new Reverb();
    }

    public void NiceVocalPreset()
    {
        eq.VocalPreset();
        comp.SoftPreset();
        rev.MediumPlatePreset();
    }
    public void RapVocalPreset()
    {
        eq.VocalPreset();
        comp.AggressivePreset();
        rev.SmallRoomPreset();
    }
    public void CosmicGuitarPreset()
    {
        eq.GuitarPreset();
        comp.MediumPreset();
        rev.AmbiencePreset();
    }
    public void DrumBusPreset()
    {
        eq.DrumsPreset();
        comp.AggressivePreset();
        rev.SmallRoomPreset();
    }

    public void PrintSettings()
    {
        Console.WriteLine("-----------------------------");
        eq.PrintSettings();
        comp.PrintSettings();
        rev.PrintSettings();
        Console.WriteLine("-----------------------------");
    }
}

class Equalizer : IPlugin
{
    public List<EqNode> nodes;

    public Equalizer() 
    {
        nodes = new();
    }

    public void AddNode(EqNode node)
    {
        nodes.Add(node);
    }

    public void RemoveNode(int index)
    {
        nodes.RemoveAt(index);
    }

    public void VocalPreset()
    {
        nodes.Clear();
        nodes.Add(new EqNode(EqModes.LowCut, 80.0, 0.0, 1.0));
        nodes.Add(new EqNode(EqModes.LowShelf, 180.0, -1.5, 1.5));
        nodes.Add(new EqNode(EqModes.Bell, 600.0, -2.0, 6.0));
        nodes.Add(new EqNode(EqModes.HighShelf, 14000.0, 4.0, 1.3));
    }

    public void GuitarPreset()
    {
        nodes.Clear();
        nodes.Add(new EqNode(EqModes.LowCut, 60.0, 0.0, 0.75));
        nodes.Add(new EqNode(EqModes.Bell, 233.0, -3.0, 1.0));
        nodes.Add(new EqNode(EqModes.FlatTilt, 600.0, 0.5, 1.0));
        nodes.Add(new EqNode(EqModes.Bell, 3600.0, -1.0, 0.285));
    }

    public void DrumsPreset()
    {
        nodes.Clear();
        nodes.Add(new EqNode(EqModes.Bell, 409.0, -6.0, 0.562));
        nodes.Add(new EqNode(EqModes.Bell, 2140.0, 2.0, 0.67));
    }

    public void PrintSettings()
    {
        Console.WriteLine("Equalizer settings:");
        if (nodes.Count == 0)
        {
            Console.WriteLine("No equalizer nodes" + Environment.NewLine);
            return;
        }
        string str = String.Empty;
        Console.WriteLine("Node\tMode\t\tFreq\tGain\tQ");
        for (int i = 0; i < nodes.Count; i++)
        {
            str = str + (i+1) + "\t" + nodes[i].ToString() + Environment.NewLine;
        }
        Console.WriteLine(str);
    }
}

class EqNode
{
    public EqModes mode;
    public double freq;
    public double gain;
    public double q;

    public EqNode(EqModes mode, double freq, double gain, double q)
    {
        this.mode = mode;
        this.freq = freq;
        this.gain = gain;
        this.q = q;
    }
    public EqNode()
    {
        mode = EqModes.Bell;
        freq = 2000;
        gain = 0.0;
        q = 1.0;
    }

    public override string ToString()
    {
        string str = String.Empty;
        if (mode == EqModes.Bell || mode == EqModes.LowCut || mode == EqModes.HighCut || mode == EqModes.Notch)
        {
            str = mode.ToString() + "\t\t" + freq + "\t" + gain + "\t" + q;
        } 
        else
        {
            str = mode.ToString() + "\t" + freq + "\t" + gain + "\t" + q;
        }
        return str;
    }
}

public class Compressor : IPlugin
{
    public double threshold;
    public double ratio;
    public double attack;
    public double release;

    public Compressor(double threshold, double ratio, double attack, double release)
    {
        this.threshold = threshold;
        this.ratio = ratio;
        this.attack = attack;
        this.release = release;
    }
    public Compressor()
    {
        threshold = -18.0;
        ratio = 4.0;
        attack = 0.25;
        release = 200.0;
    }

    public void SoftPreset()
    {
        threshold = -16.0;
        ratio = 2.1;
        attack = 2.0;
        release = 100.0;
    }
    public void MediumPreset()
    {
        threshold = -20.0;
        ratio = 4.0;
        attack = 0.5;
        release = 50.0;
    }
    public void AggressivePreset()
    {
        threshold = -23.0;
        ratio = 10.0;
        attack = 0.01;
        release = 15.0;
    }
    public void PrintSettings()
    {
        string str = "Compressor settings:" + Environment.NewLine +
            "Threshold: " + threshold + Environment.NewLine +
            "Ratio: " + ratio + Environment.NewLine +
            "Attack: " + attack + Environment.NewLine +
            "Release: " + release + Environment.NewLine;
        Console.WriteLine(str);
    }
}

class Reverb : IPlugin
{
    public ReverbModes mode;
    public double decay;
    public double predelay;
    public double size;

    public Reverb(ReverbModes mode, double decay, double predelay, double size)
    {
        this.mode = mode;
        this.decay = decay;
        this.predelay = predelay;
        this.size = size;
    }
    public Reverb()
    {
        mode = ReverbModes.Room;
        decay = 4.0;
        predelay = 20.0;
        size = 1.0;
    }

    public void SmallRoomPreset()
    {
        mode = ReverbModes.Room;
        decay = 1.0;
        predelay = 14.0;
        size = 0.6;
    }
    public void MediumPlatePreset()
    {
        mode = ReverbModes.Plate;
        decay = 2.0;
        predelay = 19.0;
        size = 0.7;
    }
    public void LargeHallPreset()
    {
        mode = ReverbModes.Hall;
        decay = 4.0;
        predelay = 25.0;
        size = 1.0;
    }
    public void AmbiencePreset()
    {
        mode = ReverbModes.Ambience;
        decay = 7.0;
        predelay = 37.0;
        size = 1.0;
    }

    public void PrintSettings()
    {
        string str = "Reverb settings:" + Environment.NewLine +
            "Mode: " + mode + Environment.NewLine +
            "Decay: " + decay + Environment.NewLine +
            "PreDelay: " + predelay + Environment.NewLine +
            "Size: " + size + Environment.NewLine;
        Console.WriteLine(str);
    }
}
