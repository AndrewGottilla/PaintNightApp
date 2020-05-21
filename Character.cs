using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PaintNight
{
    class Character
    {
        public string Title { get; set; }
        
        public Character()
        {
            this.Title = "Default Character";
        }
        public Character(string c)
        {
            this.Title = c;
        }
    }
}
