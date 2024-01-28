namespace Paraclete;

using Paraclete.Ansi;

public class ToDoItem(string description, DateOnly expirationDate = default)
{
    public string Description { get; set; } = description;
    public DateOnly ExpirationDate { get; set; } = expirationDate;

    public AnsiString ToDisplayString(bool done)
    {
        var now = DateOnly.FromDateTime(DateTime.Now.Date);
        var descriptionColor = done
            ? AnsiSequences.ForegroundColors.Gray
            : now switch
            {
                var _ when ExpirationDate == default => AnsiSequences.ForegroundColors.Yellow,
                var _ when ExpirationDate < now      => AnsiSequences.ForegroundColors.Red,
                var _ when ExpirationDate == now     => AnsiSequences.ForegroundColors.Orange,
                _                                    => AnsiSequences.ForegroundColors.Yellow,
            };

        var expirationDate = (ExpirationDate != default
            ? AnsiSequences.ForegroundColors.Gray + ExpirationDate.ToString(" (yyyy-MM-dd)")
            : string.Empty);

        return descriptionColor + Description + expirationDate + AnsiSequences.Reset;
    }

    public string ToPersistString(bool done) =>
        (done ? "- " : "  ") +
        ExpirationDate.ToString("yyyy-MM-dd ") +
        Description;
}
