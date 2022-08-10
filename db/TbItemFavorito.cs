using System;
using System.Collections.Generic;

namespace buyge_backend.db
{
    public partial class TbItemFavorito
    {
        public int CdItemFavorito { get; set; }
        public int FkCdFavorito { get; set; }
        public int FkCdProduto { get; set; }

        public virtual TbProduto FkCdProdutoNavigation { get; set; } = null!;
    }
}
