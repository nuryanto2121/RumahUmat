using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace RumahUmat.Models
{
    public class PosApi : BaseEntity
    {
        [Key]
        public Int32 posapiid { get; set; }
        [Required]
        public string api { get; set; }
        [Required]
        public string method { get; set; }
        [Required]
        public string function { get; set; }
    }
}
