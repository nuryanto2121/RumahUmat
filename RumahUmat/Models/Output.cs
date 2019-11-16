using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RumahUmat.Models
{

    public class OutPut : BaseEntity
    {
        public int Status { get; set; } = 200;
        public Boolean Error { get; set; } = false;
        public object Data { get; set; }
        public string Message { get; set; }
    }
    public class OutPutList<T> : BaseEntity
    {
        public int Status { get; set; } = 200;
        public int Total { get; set; }
        public int Current_Page { get; set; }
        public int Last_Page { get; set; }
        public List<T> Data { get; set; }
        public bool Error { get; set; }
        public string Message { get; set; }

    }

}
