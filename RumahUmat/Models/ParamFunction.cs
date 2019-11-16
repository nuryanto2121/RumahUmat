using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RumahUmat.Models
{
    public class ParamFunction
    {

        public string routine_name { get; set; }
        public string parameter_name { get; set; }
        public string data_type { get; set; }
        public int oridinal_position { get; set; }

    }
    public class FieldList
    {
        public string column_name { get; set; }
        public int oridinal_position { get; set; }
        public string data_type { get; set; }
        public int precision { get; set; }
        public int scale { get; set; }
        public int max_length{ get; set; }

    }
}
