using System;
using System.Collections.Generic;

namespace buyge_backend.db
{
    public partial class TbEndereco
    {
        public int CdEndereco { get; set; }
        public string NmLogradouro { get; set; } = null!;
        public long NrEndereco { get; set; }
        public string NmBairro { get; set; } = null!;
        public string NrCep { get; set; } = null!;
        public string NmCidade { get; set; } = null!;
        public string SgEstado { get; set; } = null!;
        public string NmTituloEndereco { get; set; } = null!;
        public string NmTipoEndereco { get; set; } = null!;
        public ulong IdPrincipal { get; set; }
        public int FkCdCliente { get; set; }

        public virtual TbCliente FkCdClienteNavigation { get; set; } = null!;
    }
}
