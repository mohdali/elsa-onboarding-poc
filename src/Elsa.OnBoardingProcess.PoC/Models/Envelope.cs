namespace Elsa.OnBoardingProcess.PoC.Models;

public class Envelope<T>  where T : class
{
    public Envelope(T? settings, bool goToPrevious)
    {
        Input = settings;
        GoToPrevious = goToPrevious;
    }
    
    public bool GoToPrevious { get; }
    public T Input { get; }
}