using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ProjetoG6.Models
{
    public class ProgramaMobilidadePais
    {
        public int ProgramaMobilidadePaisID { get; set; }
        //-----
        public int ProgramaMobilidadeID { get; set; }
        public virtual ProgramaMobilidade ProgramaMobilidade { get; set; }
        //-----
        public int PaisID { get; set; }
        public virtual Paises Paises { get; set; }
    }
}
