using System;
using System.Collections.Generic;
using System.Linq;

namespace ColorBook.Models
{
    public class ColorScheme
    {
        public Guid Id { get; set; }

        public string Name { get; set; }

        private List<NamedColor> colors = new List<NamedColor>();
        public IEnumerable<NamedColor> Colors
        {
            get => colors;
            set => colors = value.ToList();
        }

        public DateTime LastUpdate { get; set; }


        public NamedColor GetColor(int index) => colors[index];

        public void AddColor(string colorHex) => colors.Add(new NamedColor(colorHex));

        public void AddColor(NamedColor color) => colors.Add(color);

        public void AddColors(IEnumerable<NamedColor> colors) => this.colors.AddRange(colors);

        public void InsertColors(int index, IEnumerable<NamedColor> colors) => this.colors.InsertRange(index, colors);

        public void RemoveColor(int index) => colors.RemoveAt(index);

        public int ReorderColors(int prevIndex, int newIndex)
        {
            var color = colors[prevIndex];
            if (prevIndex > newIndex)
            {
                colors.Insert(newIndex + 1, color);
                colors.RemoveAt(prevIndex + 1);
                return newIndex + 1;
            }
            else
            {
                colors.RemoveAt(prevIndex);
                colors.Insert(newIndex, color);
                return newIndex;
            }
        }

        public void SwitchColors(int prevIndex, int newIndex)
        {
            var color = colors[prevIndex];
            colors[prevIndex] = colors[newIndex];
            colors[newIndex] = color;
        }
    }
}
