using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MurongEnrollment.Models
{
    public partial class Enrollments
    {
        public decimal? Fees
        {
            get
            {
                return (this.Sections?.GradeLevels?.StandardFees?.Sum(m => m.FeeAmount) ?? 0.0M) ;
            }
        }
        public decimal? TotalBalance
        {
            get
            {
                return (this.Sections?.GradeLevels?.StandardFees?.Sum(m => m.FeeAmount) ?? 0.0M) - (this.Payments.Sum(m => m.PaymentAmount) ?? 0.0M);
            }
        }
        public string BalanceStatus
        {
            get
            {
                var totalBalance = TotalBalance; ;
                return totalBalance <= 0.0M ? "Fully Paid" : totalBalance?.ToString("N2");
            }

        }
    }
}