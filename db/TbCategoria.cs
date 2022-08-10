using System;
using System.Collections.Generic;

namespace buyge_backend.db
{
    public partial class TbCategoria
    {
        public TbCategoria()
        {
            FkCdProduto = new HashSet<TbProduto>();
        }

        public int CdCategoria { get; set; }
        public string NmCategoria { get; set; } = null!;
        public string DsCategoria { get; set; } = null!;

        public virtual ICollection<TbProduto> FkCdProduto { get; set; }
    }
}
