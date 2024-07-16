using SalesWeb.Models.Enums;

namespace SalesWeb.Models
{
    public class SaleStatusModel
    {

        public SaleStatus Status { get; set; }
        
        public string GetStatusColor()
        {
            switch (Status)
            {
                case SaleStatus.Pending:
                    return "text-warning"; // exemplo de cor para status Pendente
                case SaleStatus.Billed:
                    return "text-success"; // exemplo de cor para status Faturado
                case SaleStatus.Canceled:
                    return "text-danger"; // exemplo de cor para status Cancelado
                default:
                    return "text-dark"; // cor padrão
            }
        }
        
    }
}
