using System;
using System.Collections.Generic;

namespace buyge_backend.db
{
    public partial class TbProdutoImagem
    {
        public int CdProdutoImagem { get; set; }
        public byte[] ImgProduto { get; set; } = null!;
        public string DsImagemProduto { get; set; } = null!;
        public int FkCdProduto { get; set; }
    }
}
