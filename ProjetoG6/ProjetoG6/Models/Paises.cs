
using System;
using System.ComponentModel.DataAnnotations;

namespace ProjetoG6.Models
{
    public class Paises
    {
        [Key]
        public int PaisID { get; set; }
        //----
        public string Pais { get; set; }

        public static implicit operator Paises(int v)
        {
            throw new NotImplementedException();
        }
    }
}
