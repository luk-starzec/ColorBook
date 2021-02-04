using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ColorBook.Models
{
    public class ColorScheme
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public KeyValuePair<string, string>[] Colors { get; set; }
    }
}
