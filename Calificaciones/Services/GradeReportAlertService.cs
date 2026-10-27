namespace Calificaciones.Services;

/// <summary>
/// Provides the native MAUI alert presenter to the Blazor grade report page.
/// </summary>
public sealed class GradeReportAlertService
{
    private Func<string, string, string, Task>? _presentAlert;

    public void RegisterPresenter(Func<string, string, string, Task> presentAlert)
    {
        _presentAlert = presentAlert;
    }

    public Task ShowAsync(string title, string message, string cancel)
    {
        if (_presentAlert is null)
        {
            throw new InvalidOperationException("The native alert presenter has not been registered.");
        }

        return _presentAlert(title, message, cancel);
    }
}
