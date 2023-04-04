namespace Elsa.OnBoardingProcess.PoC.Components;

public interface ICustomDynamicComponent
{
    public object GetValue();

    public void SetValue(object data);
}