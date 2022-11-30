using System;
using System.Collections.Generic;

namespace buyge_backend.db
{
    public partial class TbProduto
    {
        public TbProduto()
        {
            TbItemCarrinho = new HashSet<TbItemCarrinho>();
            TbItemFavorito = new HashSet<TbItemFavorito>();
            TbProdutoImagem = new HashSet<TbProdutoImagem>();
        }

        public int CdProduto { get; set; }
        public string NmProduto { get; set; } = null!;
        public string DsProduto { get; set; } = null!;
        public decimal VlProduto { get; set; }
        public int QtProduto { get; set; }
        public decimal? VlPeso { get; set; }
        public int? VlTamanho { get; set; }
        public decimal? VlFrete { get; set; }
        public ulong IdDisponibilidade { get; set; }
        public DateTime DtCriacao { get; set; }
        public int FkCdMercante { get; set; }
        public int FkCdCategoria { get; set; }

        public virtual TbCategoria FkCdCategoriaNavigation { get; set; } = null!;
        public virtual TbMercante FkCdMercanteNavigation { get; set; } = null!;
        public virtual ICollection<TbItemCarrinho> TbItemCarrinho { get; set; }
        public virtual ICollection<TbItemFavorito> TbItemFavorito { get; set; }
        public virtual ICollection<TbProdutoImagem> TbProdutoImagem { get; set; }
    }
}
