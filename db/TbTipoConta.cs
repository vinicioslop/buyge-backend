using System;
using System.Collections.Generic;

namespace buyge_backend.db
{
    public partial class TbTipoConta
    {
        public TbTipoConta()
        {
            TbCliente = new HashSet<TbCliente>();
        }

        public int CdTipoConta { get; set; }
        public string NmTipoConta { get; set; } = null!;

        public virtual ICollection<TbCliente> TbCliente { get; set; }
    }
}
