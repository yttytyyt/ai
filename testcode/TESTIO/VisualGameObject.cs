namespace ConsoleIO
{
    public class VisualGameObject
    {
        public VisualGameObject(string type, ConsoleColor consoleColor)
        {
            this.type = type;
            this.color = consoleColor;
        }

        public string type { get; set; }
        public ConsoleColor color { get; set; }
    }
}
