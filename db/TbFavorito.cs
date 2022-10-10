using System;
using System.Collections.Generic;

namespace buyge_backend.db
{
    public partial class TbFavorito
    {
        public int CdFavorito { get; set; }
        public int FkCdCliente { get; set; }

        public virtual TbCliente FkCdClienteNavigation { get; set; } = null!;
    }
}
