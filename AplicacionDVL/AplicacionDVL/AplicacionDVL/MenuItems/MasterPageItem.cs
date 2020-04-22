using System;
using System.Collections.Generic;
using System.Text;

namespace AplicacionDVL.MenuItems
{
    public class MasterPageItem
    {
        public string MenuTitle { get; set; }

        public string MenuDetail { get; set; }

        public string Icon { get; set; }

        public Type Page { get; set; }
    }
}
