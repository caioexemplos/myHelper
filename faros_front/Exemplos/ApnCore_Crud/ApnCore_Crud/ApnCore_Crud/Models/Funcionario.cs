using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ApnCore_Crud.Models
{
    [Table("Funcionarios")]
    public class Funcionario
    {
        [Key]
        public int FuncionarioId { get; set; }
        [Required]
        public string Nome { get; set; }
        [Required]
        public string Cidade { get; set; }
        [Required]
        public string Departamento { get; set; }
        [Required]
        public string Sexo { get; set; }
    }
}
