using System;
using System.Collections.Generic;
using System.Text;

namespace Model.ViewModel
{
    
        public class CapteurVM
        {
            public int? Id { get; set; }
            public string Capteur_Name { get; set; }
            public string Capteur_Type { get; set; }
            public double? Capteur_Value { get; set; }
            
        }
    public class ResponsDeletCapteur
    {
        public int Id { get; set; }
        public string Message { get; set; }
    }
    }

