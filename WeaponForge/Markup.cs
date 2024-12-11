using VampireCommandFramework;

namespace WeaponForge
{
    internal static class Markup
    {
        public static string Highlight(int i) => Highlight(i.ToString());
        public static string Highlight(string s) => s.Bold().Color(HighlightColor);

        public static string Secondary(int i) => Secondary(i.ToString());
        public static string Secondary(string s) => s.Bold().Color(SecondaryColor);

        public const string HighlightColor = "#90ee90"; // Light green for highlights
        public const string SecondaryColor = "#87ceeb"; // Light blue for secondary text

        public static string Prefix = $"[WF]".Color("#ff4500").Bold(); // Orange prefix for clarity
    }
}
