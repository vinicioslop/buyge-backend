namespace buyge_backend.db
{
    public partial class TbEnderecoLoja
    {
        public int CdEndereco { get; set; }
        public string NmLogradouro { get; set; } = null!;
        public long NrEndereco { get; set; }
        public string NmBairro { get; set; } = null!;
        public string NrCep { get; set; } = null!;
        public string NmCidade { get; set; } = null!;
        public string SgEstado { get; set; } = null!;
        public int FkCdMercante { get; set; }

        public virtual TbMercante FkCdMercanteNavigation { get; set; } = null!;
    }
}
