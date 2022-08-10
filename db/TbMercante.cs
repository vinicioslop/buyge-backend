using System;
using System.Collections.Generic;

namespace buyge_backend.db
{
    public partial class TbMercante
    {
        public int CdMercante { get; set; }
        public string NmLojaMercante { get; set; } = null!;
        public string DsLojaMercante { get; set; } = null!;
        public byte[]? ImgLogoMercante { get; set; }
        public int FkCdCliente { get; set; }

        public virtual TbCliente FkCdClienteNavigation { get; set; } = null!;
    }
}
