﻿using System;
using System.Collections.Generic;

namespace buyge_backend.db
{
    public partial class TbCarrinho
    {
        public TbCarrinho()
        {
            TbItemCarrinho = new HashSet<TbItemCarrinho>();
        }

        public int CdCarrinho { get; set; }
        public int FkCdCliente { get; set; }

        public virtual TbCliente FkCdClienteNavigation { get; set; } = null!;
        public virtual ICollection<TbItemCarrinho> TbItemCarrinho { get; set; }
    }
}
