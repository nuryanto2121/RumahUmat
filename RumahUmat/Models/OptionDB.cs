using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace RumahUmat.Models
{
    public class OptionDB : BaseEntity
    {
        [Key]
        public Int32 OptionDBId { get; set; }
        public string ApiName { get; set; }
        public string Method { get; set; }
        public string Functions { get; set; }
        public string UserIntput { get; set; }
        public string UserEdit { get; set; }
        public DateTime TimeInput { get; set; }
        public DateTime TimeEdit { get; set; }
    }
}
