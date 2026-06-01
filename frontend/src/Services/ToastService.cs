namespace ClientAcquisition.Frontend.Services;

public enum ToastLevel
{
    Success,
    Error,
    Info
}

public sealed class ToastMessage
{
    public Guid Id { get; } = Guid.NewGuid();
    public required string Text { get; init; }
    public ToastLevel Level { get; init; } = ToastLevel.Info;
}

public sealed class ToastService
{
    private readonly List<ToastMessage> _messages = new();

    public IReadOnlyList<ToastMessage> Messages => _messages;

    public event Action? OnChange;

    public void ShowSuccess(string text) => Show(text, ToastLevel.Success);

    public void ShowError(string text) => Show(text, ToastLevel.Error);

    public void ShowInfo(string text) => Show(text, ToastLevel.Info);

    public void Show(string text, ToastLevel level)
    {
        var message = new ToastMessage { Text = text, Level = level };
        _messages.Add(message);
        OnChange?.Invoke();

        _ = RemoveAfterDelayAsync(message);
    }

    public void Remove(ToastMessage message)
    {
        if (_messages.Remove(message))
        {
            OnChange?.Invoke();
        }
    }

    private async Task RemoveAfterDelayAsync(ToastMessage message)
    {
        await Task.Delay(4000);
        Remove(message);
    }
}
