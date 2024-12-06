using System;
using System.Collections.Generic;
using System.Text;

namespace Model.DataModel
{
    public class Capteur
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Type { get; set; }
        public double? Value { get; set; }
        public DateTime Dt_Modif { get; set; }
    }
}
