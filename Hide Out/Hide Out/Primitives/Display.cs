using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HideOut.Primitives
{
    class Display
    {

        
          
            public int x { get; set; }
            public int y { get; set; }
            public String text { get; set; }
          

            public Display(int x, int y, String text)
            {
                this.x = x;
                this.y = y;
                this.text = text;
                
            }
        

    }
}
