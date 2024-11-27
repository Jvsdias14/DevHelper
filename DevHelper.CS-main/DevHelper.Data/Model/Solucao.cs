using System;
using System.Collections.Generic;

namespace DevHelper.Data.Model;

public partial class Solucao
{
    public int Id { get; set; }

    public int ProblemaId { get; set; }

    public int UsuarioId { get; set; }

    public string Descricao { get; set; } = null!;

    public int LikeCount { get; set; } // Adiciona a propriedade LikeCount
    public int DislikeCount { get; set; } // Adiciona a propriedade DislikeCount

    public virtual ICollection<ArquivoSolucao> ArquivoSolucaos { get; set; } = new List<ArquivoSolucao>();

    public virtual Problema Problema { get; set; } = null!;

    public virtual Usuario Usuario { get; set; } = null!;
}
