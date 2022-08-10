using System;
using System.Collections.Generic;

namespace buyge_backend.db
{
    public partial class TbCliente
    {
        public TbCliente()
        {
            TbCarrinho = new HashSet<TbCarrinho>();
            TbEndereco = new HashSet<TbEndereco>();
            TbFavorito = new HashSet<TbFavorito>();
            TbJogador = new HashSet<TbJogador>();
            TbMercante = new HashSet<TbMercante>();
        }

        public int CdCliente { get; set; }
        public string NmCliente { get; set; } = null!;
        public string NmSobrenomeCliente { get; set; } = null!;
        public DateOnly DtNascimentoCliente { get; set; }
        public string NmEmailCliente { get; set; } = null!;
        public string NrTelefone { get; set; } = null!;
        public string NmLogin { get; set; } = null!;
        public string NmSenha { get; set; } = null!;

        public virtual ICollection<TbCarrinho> TbCarrinho { get; set; }
        public virtual ICollection<TbEndereco> TbEndereco { get; set; }
        public virtual ICollection<TbFavorito> TbFavorito { get; set; }
        public virtual ICollection<TbJogador> TbJogador { get; set; }
        public virtual ICollection<TbMercante> TbMercante { get; set; }
    }
}
