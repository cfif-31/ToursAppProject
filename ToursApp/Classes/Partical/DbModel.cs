using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ToursApp.Classes
{
    partial class DbModel
    {
        //+Singletone
        private static DbModel instance;
        public static DbModel init() {
            if (instance == null) {
                instance = new DbModel();
            }
            return instance;
        }
        //-Singletone
    }
}
