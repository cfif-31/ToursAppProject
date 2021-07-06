using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ToursApp.Classes
{
    public partial class Hotel
    {
        public int TourCount 
        {
            get
            {
                return Tours.Count();
            } 
        }
    }
}
