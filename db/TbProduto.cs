using System;
using System.Collections.Generic;

namespace buyge_backend.db
{
    public partial class TbProduto
    {
        public int CdProduto { get; set; }
        public string NmProduto { get; set; } = null!;
        public string DsProduto { get; set; } = null!;
        public decimal VlProduto { get; set; }
        public int QtEstoque { get; set; }
        public int FkCdMercante { get; set; }
        public int FkCdCategoria { get; set; }

        public virtual TbCategoria FkCdCategoriaNavigation { get; set; } = null!;
        public virtual TbMercante FkCdMercanteNavigation { get; set; } = null!;
    }
}
