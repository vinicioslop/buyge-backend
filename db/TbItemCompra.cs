using System;
using System.Collections.Generic;

namespace buyge_backend.db
{
    public partial class TbItemCompra
    {
        public int CdItemCompra { get; set; }
        public string NmProduto { get; set; } = null!;
        public decimal VlItemCompra { get; set; }
        public string DsProduto { get; set; } = null!;
        public int QtItemCompra { get; set; }
        public int FkCdCompra { get; set; }

        public virtual TbCompra FkCdCompraNavigation { get; set; } = null!;
    }
}
