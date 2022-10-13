using System;
using System.Collections.Generic;

namespace buyge_backend.db
{
    public partial class TbCompra
    {
        public TbCompra()
        {
            TbItemCompra = new HashSet<TbItemCompra>();
        }

        public int CdCompra { get; set; }
        public string VlTotalCompra { get; set; } = null!;
        public int FkCdCliente { get; set; }

        public virtual TbCliente FkCdClienteNavigation { get; set; } = null!;
        public virtual ICollection<TbItemCompra> TbItemCompra { get; set; }
    }
}
