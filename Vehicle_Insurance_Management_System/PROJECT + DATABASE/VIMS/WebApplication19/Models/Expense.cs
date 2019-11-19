using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace VIMS.Models
{
    public class Expense
    {
        [Key]
        public int ExpenseId { get; set; }
        [Required]
        [DisplayName("Date of Expense")]
        public DateTime DateOfExpense { get; set; }
        [Required]
        [DisplayName("Description")]
        public string TypeOfExpense { get; set; }
        [Required]
        [DisplayName("Amount Costed")]
        public int AmountOfExpense { get; set; }
        [Required]
        public string ApplicationUserId { get; set; }
        public virtual ApplicationUser ApplicationUser { get; set; }
    }
}