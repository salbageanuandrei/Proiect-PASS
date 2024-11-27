using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Proiect_PASS_New_1.Models
{

  [Table("Students")]
  public class Student
  {
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    [Required]
    public int Id { get; set; }
    public string NrMatricol { get; set; }
    public string Nume  { get; set; }
    public string Prenume { get; set; }
    public double Medie { get; set; }
  }
}