namespace Settings
{
    public sealed class Colors
    {
        public readonly Color darkGray;
        public readonly Color gray;
        public readonly Color violet;
        public readonly Color yellow;
        public readonly Color blue;
        public readonly Color green;

        public Colors()
        {
            darkGray = Color.FromArgb("#FFFFFF");
            gray = Color.FromArgb("#FFFFFF");
            violet = Color.FromArgb("#FFFFFF");
            yellow = Color.FromArgb("#FFFFFF");
            blue = Color.FromArgb("#FFFFFF");
            green = Color.FromArgb("#FFFFFF");
        }

        public Colors(string darkGray, string gray, string violet, string yellow, string blue, string green)
        {
            this.darkGray = Color.FromArgb(darkGray);
            this.gray = Color.FromArgb(gray);
            this.violet = Color.FromArgb(violet);
            this.yellow = Color.FromArgb(yellow);
            this.blue = Color.FromArgb(blue);
            this.green = Color.FromArgb(green);
        }
    }
}
