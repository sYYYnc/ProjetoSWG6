using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ProjetoG6.Models
{
    public class Candidatos
    {
        [Key]
        public int CandidatoID { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "Introduza o seu nome")]
        public string Nome { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "Introduza o seu email")]
        [DataType(DataType.EmailAddress, ErrorMessage = "Introduza um email valido")]
        public string Email { get; set; }

        [Display(Name = "Data de Nascimento")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Introduza a data de nascimento")]
        [DataType(DataType.Date, ErrorMessage = "Introduza uma data valida")]
        public System.DateTime DataNascimento { get; set; }

        [Display(Name = "Numero de aluno")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Introduza o seu numero de aluno")]
        [MaxLength(9, ErrorMessage = "Numero de aluno tem 9 digitos")]
        public string NumeroAluno { get; set; }

        [Display(Name = "Palavra-Chave")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Introduza uma password")]
        [MinLength(6, ErrorMessage = "Password tem que ter 6 digitos no minimo")]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Display(Name = "Confirmação Palavra-Chave")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Introduza uma password")]
        [MinLength(6, ErrorMessage = "Password tem que ter 6 digitos no minimo")]
        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessage = "As palavras-chaves não coincidem")]
        public string PasswordConfirmacao { get; set; }

        public int BolsaID { get; set; }
        [Display(Name = "Candidatura")]
        public int CandidaturaID { get; set; }
        public int EntervistaID { get; set; }
    }
}
