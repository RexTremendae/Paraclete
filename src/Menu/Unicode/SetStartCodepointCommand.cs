namespace Paraclete.Menu.Unicode;

using System.Numerics;
using Paraclete.IO;
using Paraclete.Screens.Unicode;

public class SetStartCodepointCommand(UnicodeControl unicodeControl, DataInputter dataInputter)
    : IInputCommand<BigInteger>
{
    private readonly UnicodeControl _unicodeControl = unicodeControl;
    private readonly DataInputter _dataInputter = dataInputter;

    public ConsoleKey Shortcut => ConsoleKey.C;

    public string Description => "Select starting [C]odepoint";

    public Task CompleteInput(BigInteger data)
    {
        _unicodeControl.SetSelectedCodepoint((int)data);
        return Task.CompletedTask;
    }

    public Task Execute() => _dataInputter.StartInput(this, "Input starting codepoint:", inputDefinition: new RadixInputDefinition());
}
