using System;
using System.Collections.Generic;

namespace buyge_backend.db
{
    public partial class TbMercante
    {
        public TbMercante()
        {
            TbProduto = new HashSet<TbProduto>();
        }

        public int CdMercante { get; set; }
        public string NmLoja { get; set; } = null!;
        public string DsLoja { get; set; } = null!;
        public string? ImgLogoLink { get; set; }
        public byte[]? ImgLogo { get; set; }
        public string NrCnpj { get; set; } = null!;
        public int FkCdCliente { get; set; }

        public virtual TbCliente FkCdClienteNavigation { get; set; } = null!;
        public virtual ICollection<TbProduto> TbProduto { get; set; }
    }
}
