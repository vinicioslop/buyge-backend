namespace buyge_backend.db
{
    public partial class TbMercante
    {
        public TbMercante()
        {
            TbEnderecoLoja = new HashSet<TbEnderecoLoja>();
            TbProduto = new HashSet<TbProduto>();
        }

        public int CdMercante { get; set; }
        public string NmLoja { get; set; } = null!;
        public string DsLoja { get; set; } = null!;
        public string NmEmail { get; set; } = null!;
        public string? ImgLogoLink { get; set; }
        public byte[]? ImgLogo { get; set; }
        public string? NrCnpj { get; set; }
        public string? NrTelefoneFixo { get; set; }
        public string? NrTelefoneCelular { get; set; }
        public int FkCdCliente { get; set; }

        public virtual TbCliente FkCdClienteNavigation { get; set; } = null!;
        public virtual ICollection<TbEnderecoLoja> TbEnderecoLoja { get; set; }
        public virtual ICollection<TbProduto> TbProduto { get; set; }
    }
}
