using System;
using System.Collections.Generic;

namespace buyge_backend.db
{
    public partial class TbItemCarrinho
    {
        public int CdItemCarrinho { get; set; }
        public int FkCdProduto { get; set; }
        public int FkCdCliente { get; set; }

        public virtual TbCliente FkCdClienteNavigation { get; set; } = null!;
        public virtual TbProduto FkCdProdutoNavigation { get; set; } = null!;
    }
}
