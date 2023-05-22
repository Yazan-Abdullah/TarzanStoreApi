namespace FullApiOnlineStore.DTO
{
    public class OrderDetilesForUser
    {
        public string OrderDate { get; set; }

        public string DeliveryDate { get; set; }

        public string TotalPrice { get; set; }

        public string Note { get; set; }

        public string OrderStatus { get; set; }

        public string IsApproved { get; set; }

        public List<OrderCatrItemDTO> MyCart { get; set; }
    }
}
