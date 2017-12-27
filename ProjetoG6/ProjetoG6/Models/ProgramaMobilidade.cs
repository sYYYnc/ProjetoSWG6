using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ProjetoG6.Models
{
    public class ProgramaMobilidade
    {
        [Key]
        public int ProgramaMobilidadeID { get; set; }
        //----
        [Required]
        [StringLength(225)]
        public string Nome { get; set; }
    }
}
