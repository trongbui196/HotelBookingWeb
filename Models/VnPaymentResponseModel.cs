namespace WebApplication1.Models
{
    public class VnPaymentResponseModel
    {
        public bool Success { get; set; }
        public string PaymentMethod { get; set; }
        public string OrderDescription { get; set; }
        public string OrderId { get; set; }
        public string PaymentId { get; set; }
        public string TransactionId { get; set; }
        public string Token { get; set; }
        public string VnPayResponseCode { get; set; }

    }
    public class VnPaymentRequestModel
    { 
        public int? OrderID { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int? ammount { get; set; }
        public string CreatedDate { get; set; }
    }
}