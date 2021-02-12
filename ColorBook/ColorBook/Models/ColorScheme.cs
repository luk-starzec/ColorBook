using System;
using System.Collections.Generic;

namespace ColorBook.Models
{
    public class ColorScheme
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public List<NamedColor> Colors { get; set; }
    }
}
