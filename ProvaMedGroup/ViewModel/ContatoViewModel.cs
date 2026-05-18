using System.ComponentModel.DataAnnotations;
using System;

namespace ProvaMedGroup.ViewModels
{
    public class ContatoViewModel
    {
        [Required(ErrorMessage = "O campo {0} é obrigatório")]
        [StringLength(50, ErrorMessage = "O Máximo de caracteres permitidos do campo {0} é {2} a {1}.", MinimumLength = 2)]
        public string PrimeiroNome { get; set; }

        [Required(ErrorMessage = "O campo {0} é obrigatório")]
        [StringLength(50, ErrorMessage = "O Máximo de caracteres permitidos do campo {0} é {2} a {1}.", MinimumLength = 2)]
        public string Sobrenome { get; set; }

        public char Sexo { get; set; }

        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}")]
        [Required(ErrorMessage = "O campo {0} é obrigatório")]
        public DateTime DataNascimento { get; set; }
        
        public bool Ativo { get; set; }
    }
}
