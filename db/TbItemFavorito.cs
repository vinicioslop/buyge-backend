using System;
using System.Collections.Generic;

namespace buyge_backend.db
{
    public partial class TbItemFavorito
    {
        public int CdItemFavorito { get; set; }
        public int FkCdProduto { get; set; }
        public int FkCdFavorito { get; set; }

        public virtual TbFavorito FkCdFavoritoNavigation { get; set; } = null!;
        public virtual TbProduto FkCdProdutoNavigation { get; set; } = null!;
    }
}
