using System;
using System.Collections.Generic;

namespace buyge_backend.db
{
    public partial class TbEndereco
    {
        public int CdEndereco { get; set; }
        public string NmLogradouroEndereco { get; set; } = null!;
        public long NrEndereco { get; set; }
        public string NmBairroEndereco { get; set; } = null!;
        public long NrCepEndereco { get; set; }
        public string NmCidadeEndereco { get; set; } = null!;
        public string SgEstado { get; set; } = null!;
        public int FkCdCliente { get; set; }

        public virtual TbCliente FkCdClienteNavigation { get; set; } = null!;
    }
}
