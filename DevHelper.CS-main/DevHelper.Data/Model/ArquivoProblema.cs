using System;
using System.Collections.Generic;

namespace DevHelper.Data.Model;

public partial class ArquivoProblema
{
    public int Id { get; set; }

    public int ProblemaId { get; set; }

    public string Nome { get; set; } = null!;

    public string Referencia { get; set; } = null!;

    public virtual Problema Problema { get; set; } = null!;
}
