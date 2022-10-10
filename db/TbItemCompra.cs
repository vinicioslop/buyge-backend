using System;
using System.Collections.Generic;

namespace buyge_backend.db
{
    public partial class TbItemCompra
    {
        public int CdItemCompra { get; set; }
        public int FkCdProduto { get; set; }
        public int FkCdCompra { get; set; }
        public decimal VlItemCompra { get; set; }

        public virtual TbCompra FkCdCompraNavigation { get; set; } = null!;
    }
}
