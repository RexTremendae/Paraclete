namespace Paraclete;

public class Terminator
{
    public bool TerminationRequested { get; private set; }
    public void RequestTermination() => TerminationRequested = true;
}
