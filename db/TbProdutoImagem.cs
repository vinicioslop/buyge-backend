namespace buyge_backend.db
{
    public partial class TbProdutoImagem
    {
        public int CdProdutoImagem { get; set; }
        public string? ImgProdutoLink { get; set; }
        public byte[]? ImgProduto { get; set; }
        public string AltImagemProduto { get; set; } = null!;
        public ulong IdPrincipal { get; set; }
        public int FkCdProduto { get; set; }

        public virtual TbProduto FkCdProdutoNavigation { get; set; } = null!;
    }
}
