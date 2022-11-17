﻿using System;
using System.Collections.Generic;

namespace buyge_backend.db
{
    public partial class TbProdutoImagem
    {
        public int CdProdutoImagem { get; set; }
        public string? ImgProdutoLink { get; set; }
        public byte[]? ImgProduto { get; set; }
        public string DsImagemProduto { get; set; } = null!;
        public int FkCdProduto { get; set; }

        public virtual TbProduto FkCdProdutoNavigation { get; set; } = null!;
    }
}
