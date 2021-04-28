using System;

namespace ColorBook.Models
{
    public class Settings
    {
        public string LightBackgroundColor { get; set; }
        public string DarkBackgroundColor { get; set; }
        public string LightTextColor { get; set; }
        public string DarkTextColor { get; set; }
        public bool AutoSync { get; set; }
        public DateTime? LastUpdate { get; set; }
    }
}
