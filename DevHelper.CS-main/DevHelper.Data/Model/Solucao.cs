using System;
using System.Collections.Generic;

namespace DevHelper.Data.Model;

public partial class Solucao
{
    public int Id { get; set; }
    public int ProblemaId { get; set; }
    public int UsuarioId { get; set; }
    public string Descricao { get; set; }
    public DateTime Data { get; set; }
    public int LikeCount { get; set; } // Adiciona a propriedade LikeCount
    public int DislikeCount { get; set; } // Adiciona a propriedade DislikeCount

    public virtual Problema Problema { get; set; }
    public virtual Usuario Usuario { get; set; }
    public virtual ICollection<ArquivoSolucao> ArquivoSolucaos { get; set; }
}

