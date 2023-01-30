﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
#nullable disable
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MQEC_Api.Models
{
    [Table("comTaxType")]
    public partial class ComTaxType
    {
        [Key]
        [Column("TaxType_No")]
        [StringLength(10)]
        public string TaxTypeNo { get; set; }
        [Required]
        [Column("TaxType_Name")]
        [StringLength(50)]
        public string TaxTypeName { get; set; }
        [Required]
        [Column("VAT_Type")]
        [StringLength(10)]
        public string VatType { get; set; }
        [Column("Tax_Rate", TypeName = "decimal(10, 2)")]
        public decimal TaxRate { get; set; }
    }
}