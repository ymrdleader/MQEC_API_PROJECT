// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
#nullable disable
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MQEC_Api.Models
{
    public partial class Invoice
    {
        [Key]
        [Column("Company_No")]
        [StringLength(50)]
        public string CompanyNo { get; set; }
        [Key]
        [Column("TaxID")]
        [StringLength(20)]
        public string TaxId { get; set; }
        [Key]
        [StringLength(2)]
        public string InvoiceType { get; set; }
        [Key]
        [StringLength(5)]
        public string InvoiceCategory { get; set; }
        [Key]
        [StringLength(1)]
        public string InvoiceKind { get; set; }
        [Key]
        [StringLength(10)]
        public string InvoiceNumber { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? InvoiceDate { get; set; }
        [Required]
        [Column("Seller_ID")]
        [StringLength(50)]
        public string SellerId { get; set; }
        [Required]
        [Column("Seller_Name")]
        [StringLength(60)]
        public string SellerName { get; set; }
        [Required]
        [Column("Seller_Address")]
        [StringLength(100)]
        public string SellerAddress { get; set; }
        [Required]
        [Column("Seller_PersonInCharge")]
        [StringLength(30)]
        public string SellerPersonInCharge { get; set; }
        [Required]
        [Column("Seller_TelephoneNumber")]
        [StringLength(25)]
        public string SellerTelephoneNumber { get; set; }
        [Required]
        [Column("Seller_FacsimileNumber")]
        [StringLength(25)]
        public string SellerFacsimileNumber { get; set; }
        [Required]
        [Column("Seller_EmailAddress")]
        [StringLength(80)]
        public string SellerEmailAddress { get; set; }
        [Required]
        [Column("Seller_CustomerNumber")]
        [StringLength(50)]
        public string SellerCustomerNumber { get; set; }
        [Required]
        [Column("Seller_RoleRemark")]
        [StringLength(40)]
        public string SellerRoleRemark { get; set; }
        [Required]
        [Column("Buyer_ID")]
        [StringLength(50)]
        public string BuyerId { get; set; }
        [Required]
        [Column("Buyer_Name")]
        [StringLength(60)]
        public string BuyerName { get; set; }
        [Required]
        [Column("Buyer_Address")]
        [StringLength(100)]
        public string BuyerAddress { get; set; }
        [Required]
        [Column("Buyer_PersonInCharge")]
        [StringLength(30)]
        public string BuyerPersonInCharge { get; set; }
        [Required]
        [Column("Buyer_TelephoneNumber")]
        [StringLength(25)]
        public string BuyerTelephoneNumber { get; set; }
        [Required]
        [Column("Buyer_FacsimileNumber")]
        [StringLength(25)]
        public string BuyerFacsimileNumber { get; set; }
        [Required]
        [Column("Buyer_EmailAddress")]
        [StringLength(80)]
        public string BuyerEmailAddress { get; set; }
        [Required]
        [Column("Buyer_CustomerNumber")]
        [StringLength(50)]
        public string BuyerCustomerNumber { get; set; }
        [Required]
        [Column("Buyer_RoleRemark")]
        [StringLength(40)]
        public string BuyerRoleRemark { get; set; }
        [Required]
        [StringLength(10)]
        public string CheckNumber { get; set; }
        [Required]
        [StringLength(1)]
        public string BuyerRemark { get; set; }
        [Required]
        [StringLength(200)]
        public string MainRemark { get; set; }
        [Required]
        [StringLength(1)]
        public string CustomsClearanceMark { get; set; }
        [Required]
        [StringLength(2)]
        public string Category { get; set; }
        [Required]
        [StringLength(20)]
        public string RelateNumber { get; set; }
        [Required]
        [StringLength(1)]
        public string GroupMark { get; set; }
        public bool DonateMark { get; set; }
        [Required]
        [StringLength(10)]
        public string CarrierType { get; set; }
        [Required]
        [StringLength(64)]
        public string CarrierId1 { get; set; }
        [Required]
        [StringLength(64)]
        public string CarrierId2 { get; set; }
        [Required]
        [Column("NPOBAN")]
        [StringLength(10)]
        public string Npoban { get; set; }
        public bool PrintMark { get; set; }
        [Required]
        [StringLength(4)]
        public string RandomNumber { get; set; }
        [Column("Cancel_Date", TypeName = "datetime")]
        public DateTime? CancelDate { get; set; }
        [Column("Cancel_Flag")]
        public bool CancelFlag { get; set; }
        [Required]
        [Column("Cancel_Reason")]
        [StringLength(200)]
        public string CancelReason { get; set; }
        [Column("Cancel_User")]
        [StringLength(20)]
        public string CancelUser { get; set; }
        [Column("Delete_Date", TypeName = "datetime")]
        public DateTime? DeleteDate { get; set; }
        [Column("Delete_Flag")]
        public bool DeleteFlag { get; set; }
        [Required]
        [Column("Delete_Reason")]
        [StringLength(200)]
        public string DeleteReason { get; set; }
        [Column("Delete_User")]
        [StringLength(20)]
        public string DeleteUser { get; set; }
        [Column(TypeName = "decimal(12, 2)")]
        public decimal SalesAmount { get; set; }
        [Column(TypeName = "decimal(12, 2)")]
        public decimal FreeTaxSalesAmount { get; set; }
        [Column(TypeName = "decimal(12, 2)")]
        public decimal ZeroTaxSalesAmount { get; set; }
        [Required]
        [StringLength(1)]
        public string TaxType { get; set; }
        [Column(TypeName = "decimal(12, 2)")]
        public decimal TaxRate { get; set; }
        [Column(TypeName = "decimal(12, 2)")]
        public decimal TaxAmount { get; set; }
        [Column(TypeName = "decimal(12, 2)")]
        public decimal TotalAmount { get; set; }
        [Column(TypeName = "decimal(12, 2)")]
        public decimal DiscountAmount { get; set; }
        [Column(TypeName = "decimal(12, 2)")]
        public decimal OriginalCurrencyAmount { get; set; }
        [Column(TypeName = "decimal(12, 2)")]
        public decimal ExchangeRate { get; set; }
        [Required]
        [StringLength(3)]
        public string Currency { get; set; }
        [Column("Create_Date", TypeName = "datetime")]
        public DateTime? CreateDate { get; set; }
        [Column("Creare_User")]
        [StringLength(50)]
        public string CreareUser { get; set; }
        [Column("Update_Date", TypeName = "datetime")]
        public DateTime? UpdateDate { get; set; }
        [Column("Update_User")]
        [StringLength(50)]
        public string UpdateUser { get; set; }
    }
}