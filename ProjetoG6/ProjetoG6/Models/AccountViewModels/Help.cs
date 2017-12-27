using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ProjetoG6.Models.AccountViewModels
{
    public class Help
    {
        public int HelpID { get; set; }

        [Display(Name = "Página")]
        public String Campo { get; set; }
        [Display(Name = "Descrição")]
        public String Descricao { get; set; }
    }
}
