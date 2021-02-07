using System.Text.Json.Serialization;

namespace ColorBook.Models
{
    public class NamedColor
    {
        public string Name { get; set; }
        public string Color { get => colorHex.Replace("#", ""); set => colorHex = $"#{value}"; }
        [JsonIgnore]
        public string ColorHex { get => colorHex; set => colorHex = value; }

        private string colorHex;

        public NamedColor()
        { }

        public NamedColor(string colorHex)
            => (Name, ColorHex) = (string.Empty, colorHex);

        public NamedColor(string name, string colorHex)
            => (Name, ColorHex) = (name, colorHex);
    }
}
