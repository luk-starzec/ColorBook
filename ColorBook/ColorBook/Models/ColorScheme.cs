using System;

namespace ColorBook.Models
{
    public class ColorScheme
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public NamedColor[] Colors { get; set; }
    }
}
