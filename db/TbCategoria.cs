namespace buyge_backend.db
{
    public partial class TbCategoria
    {
        public TbCategoria()
        {
            TbProduto = new HashSet<TbProduto>();
        }

        public int CdCategoria { get; set; }
        public string NmCategoria { get; set; } = null!;
        public string DsCategoria { get; set; } = null!;

        public virtual ICollection<TbProduto> TbProduto { get; set; }
    }
}
