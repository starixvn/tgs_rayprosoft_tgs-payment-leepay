#pragma warning disable CS8618 // 退出建構函式時，不可為 Null 的欄位必須包含非 Null 值。請考慮宣告為可為 Null。

using System;
using System.Collections.Generic;

namespace LeePayRepositories.Entities
{
    public partial class WithdrawForm
    {
        public Guid Id { get; set; }
        public string PlayerCode { get; set; }
        public Guid BrandId { get; set; }
        public decimal Amount { get; set; }
        public Guid? BrandPaymentId { get; set; }
        public int Status { get; set; }
        public string Remark { get; set; }
        public Guid BankInfoId { get; set; }
        public string BankInfo { get; set; }
        public string ApplyInfo { get; set; }
        public string PaymentReturn { get; set; }
        public DateTime CreatedOn { get; set; }
        public DateTime UpdatedOn { get; set; }
        public decimal? ActualAmount { get; set; }
    }
}

#pragma warning restore CS8618 // 退出建構函式時，不可為 Null 的欄位必須包含非 Null 值。請考慮宣告為可為 Null。
