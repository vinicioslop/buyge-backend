using System;
using System.Collections.Generic;

namespace buyge_backend.db
{
    public partial class TbJogador
    {
        public int CdJogador { get; set; }
        public int NrNivelJogador { get; set; }
        public long NrXpJogador { get; set; }
        public string NmClasse { get; set; } = null!;
        public int FkCdCliente { get; set; }

        public virtual TbCliente FkCdClienteNavigation { get; set; } = null!;
    }
}
