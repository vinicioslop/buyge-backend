using System;
using System.Collections.Generic;

namespace buyge_backend.db
{
    public partial class TbCompra
    {
        public TbCompra()
        {
            TbItemCompra = new HashSet<TbItemCompra>();
        }

        public int CdCompra { get; set; }
        public decimal VlTotalCompra { get; set; }
        public decimal? VlTotalFrete { get; set; }
        public decimal? VlTotalDesconto { get; set; }
        public string IdPreferencia { get; set; } = null!;
        public string NmCollectionStatus { get; set; } = null!;
        public string NmCollectionId { get; set; } = null!;
        public string NmPaymentId { get; set; } = null!;
        public string NmStatus { get; set; } = null!;
        public string NmPaymentType { get; set; } = null!;
        public string NmMerchantOrderId { get; set; } = null!;
        public int FkCdCliente { get; set; }

        public virtual TbCliente FkCdClienteNavigation { get; set; } = null!;
        public virtual ICollection<TbItemCompra> TbItemCompra { get; set; }
    }
}
