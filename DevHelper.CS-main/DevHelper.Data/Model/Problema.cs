using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace DevHelper.Data.Model;

public partial class Problema
{
    public int Id { get; set; }

    [Required]
    public int UsuarioId { get; set; }
    [Required]
    public string Nome { get; set; } = null!;
    [Required]
    public string Descricao { get; set; } = null!;

    public virtual ICollection<ArquivoProblema> ArquivoProblemas { get; set; } = new List<ArquivoProblema>();

    public virtual ICollection<Solucao> Solucaos { get; set; } = new List<Solucao>();

    [Required]
    public virtual Usuario Usuario { get; set; } = null!;
}
