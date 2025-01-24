namespace Settings
{
    public sealed class Colors(string darkGray, string gray, string violet, string yellow, string blue, string green, string red)
    {
        public Color darkGray = Color.FromArgb(darkGray);
        public Color gray = Color.FromArgb(gray);
        public Color violet = Color.FromArgb(violet);
        public Color yellow = Color.FromArgb(yellow);
        public Color blue = Color.FromArgb(blue);
        public Color green = Color.FromArgb(green);
        public Color red = Color.FromArgb(red);
    }
}
