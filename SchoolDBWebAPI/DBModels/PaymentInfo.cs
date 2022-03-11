using System;

#nullable disable

namespace SchoolDBWebAPI.DBModels
{
    public partial class PaymentInfo
    {
        public int Id { get; set; }
        public string Guid { get; set; }
        public int StudentId { get; set; }
        public string PayerId { get; set; }
        public string ItemName { get; set; }
        public string Currency { get; set; }
        public decimal? Price { get; set; }
        public int? Quantity { get; set; }
        public decimal? TaxAmount { get; set; }
        public decimal? HandlingFee { get; set; }
        public decimal? Subtotal { get; set; }
        public decimal? TotalPrice { get; set; }
        public string InvoiceNo { get; set; }
        public int? PlanType { get; set; }
        public int? PaymentMode { get; set; }
        public int? Status { get; set; }
        public DateTime? PaymentDate { get; set; }
        public DateTime? PaymentStartDate { get; set; }
        public DateTime? PaymentEndDate { get; set; }

        public virtual StudentMaster Student { get; set; }
    }
}