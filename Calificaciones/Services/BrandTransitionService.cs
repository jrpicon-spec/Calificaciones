namespace Calificaciones.Services;

/// <summary>
/// Connects Blazor session actions to the native MAUI brand transition overlay.
/// </summary>
public sealed class BrandTransitionService
{
    private Func<Task>? _playFarewell;

    public void RegisterFarewellPresenter(Func<Task> playFarewell)
    {
        _playFarewell = playFarewell;
    }

    public Task PlayFarewellAsync()
    {
        if (_playFarewell is null)
        {
            throw new InvalidOperationException("The native farewell presenter has not been registered.");
        }

        return _playFarewell();
    }
}
