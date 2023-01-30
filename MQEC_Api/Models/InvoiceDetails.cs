﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
#nullable disable
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MQEC_Api.Models
{
    public partial class InvoiceDetails
    {
        [Key]
        [StringLength(10)]
        public string InvoiceNumber { get; set; }
        [Key]
        public int Serno { get; set; }
        [Required]
        [StringLength(50)]
        public string Prodid { get; set; }
        [Required]
        [StringLength(256)]
        public string Description { get; set; }
        public int Quantity { get; set; }
        [Required]
        [StringLength(6)]
        public string Unit { get; set; }
        [Column(TypeName = "decimal(12, 2)")]
        public decimal UnitPrice { get; set; }
        [Column(TypeName = "decimal(12, 2)")]
        public decimal Amount { get; set; }
        [Required]
        [Column("SourceTaxType_No")]
        [StringLength(10)]
        public string SourceTaxTypeNo { get; set; }
        [Required]
        [StringLength(40)]
        public string Remark { get; set; }
        [Required]
        [StringLength(20)]
        public string RelateNumber { get; set; }
        [Required]
        [Column("Source_No")]
        [StringLength(50)]
        public string SourceNo { get; set; }
        [Column("Source_Serno")]
        public int SourceSerno { get; set; }
        public int ComQty { get; set; }
        public int AvaQty { get; set; }
    }
}