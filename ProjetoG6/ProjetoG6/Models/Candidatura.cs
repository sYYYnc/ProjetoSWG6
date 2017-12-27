using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace ProjetoG6.Models
{
    public class Candidatura
    {
        [Key]
        public int CandidaturaID { get; set; }
        //----
        [Required]
        public DateTime DataCandidatura { get; set; }
        //----
        [Column("Estado")]
        [Required]
        [StringLength(225)]
        public string Estado
        {
            get { return Estad.ToString(); }
            private set { Estad = value.ParseEnum<Estado>(); }
        }
        [NotMapped]//para nao criar dados na tabela
        public Estado Estad { get; set; }
        //----
        public int ProgramaMobilidadeID { get; set; }
        //----
        public int PaisID { get; set; }
        //----
        public int AdministradorID { get; set; }
    }

    public enum Estado
    {
       EmAvaliacao=1,
       Aceite=2,
       Rejeitado=3
    }

    public static class StringExtensions
    {
        public static T ParseEnum<T>(this string value)
        {
            return (T)Enum.Parse(typeof(T), value, true);
        }
    }
}
