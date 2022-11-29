namespace buyge_backend.db
{
    public partial class TbCliente
    {
        public TbCliente()
        {
            TbCompra = new HashSet<TbCompra>();
            TbEndereco = new HashSet<TbEndereco>();
            TbItemCarrinho = new HashSet<TbItemCarrinho>();
            TbItemFavorito = new HashSet<TbItemFavorito>();
            TbMercante = new HashSet<TbMercante>();
        }

        public int CdCliente { get; set; }
        public string NmCliente { get; set; } = null!;
        public string? NmSobrenome { get; set; }
        public string? NrCpf { get; set; }
        public DateTime DtNascimento { get; set; }
        public string? NrTelefone { get; set; }
        public string NmEmail { get; set; } = null!;
        public string NmSenha { get; set; } = null!;
        public string NmTipoConta { get; set; } = null!;

        public virtual ICollection<TbCompra> TbCompra { get; set; }
        public virtual ICollection<TbEndereco> TbEndereco { get; set; }
        public virtual ICollection<TbItemCarrinho> TbItemCarrinho { get; set; }
        public virtual ICollection<TbItemFavorito> TbItemFavorito { get; set; }
        public virtual ICollection<TbMercante> TbMercante { get; set; }
    }
}
