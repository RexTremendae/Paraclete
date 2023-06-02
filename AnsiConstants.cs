namespace Time;

public static class AnsiConstants
{
    // Similar to Clear() but also clears the scrollable buffer, which Clear() doesn't.
    public const string ClearScreen = "\f\u001bc\x1b[3J";

    public const string Reset = "\x1b[m";

    public const string StrikeThrough = "\x1b[9m";
    public const string Underline = "\x1b[4m";

    public static class ForegroundColor
    {
        public const string Black       = "\x1b[30m";
        public const string DarkRed     = "\x1b[31m";
        public const string DarkGreen   = "\x1b[32m";
        public const string DarkYellow  = "\x1b[33m";
        public const string DarkBlue    = "\x1b[34m";
        public const string DarkMagenta = "\x1b[35m";
        public const string DarkCyan    = "\x1b[36m";
        public const string Gray        = "\x1b[37m";
        public const string DarkGray    = "\x1b[90m";
        public const string Red         = "\x1b[91m";
        public const string Green       = "\x1b[92m";
        public const string Yellow      = "\x1b[93m";
        public const string Blue        = "\x1b[94m";
        public const string Magenta     = "\x1b[95m";
        public const string Cyan        = "\x1b[96m";
        public const string White       = "\x1b[97m";
    }
}
