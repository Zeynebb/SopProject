using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SOP.Models;

namespace SOP.ViewModels
{
    public class Listeleme
    {

        public List<sopOnay> sopOnay { get; set; }
        public sopKayit sopKayit { get; set; }
    }
}