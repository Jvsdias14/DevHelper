using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace DevHelper.Data.Model;

public partial class Usuario
{
    public int Id { get; set; }

    public string Nome { get; set; } = null!;

    [Required]
    [EmailAddress]
    public string Email { get; set; } = null!;

    [Required]
    [DataType(DataType.Password)]
    public string Senha { get; set; } = null!;

    public string? Biografia { get; set; }

    public virtual ICollection<Problema> Problemas { get; set; } = new List<Problema>();

    public virtual ICollection<Solucao> Solucaos { get; set; } = new List<Solucao>();
}
