using buyge_backend.db;

namespace buyge_backend
{
    public class ProdutoComImagem
    {
        public TbProduto produto { get; set; } = null!;
        public TbProdutoImagem imagem { get; set; } = null!;
    }
}