using System;
using System.Collections.Generic;

namespace DevHelper.Data.Model;

public partial class Problema
{
    public int Id { get; set; }

    public int UsuarioId { get; set; }

    public string Nome { get; set; } = null!;

    public string Descricao { get; set; } = null!;

    public virtual ICollection<ArquivoProblema> ArquivoProblemas { get; set; } = new List<ArquivoProblema>();

    public virtual ICollection<Solucao> Solucaos { get; set; } = new List<Solucao>();

    public virtual Usuario Usuario { get; set; } = null!;
}
