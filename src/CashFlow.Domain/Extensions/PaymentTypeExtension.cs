using CashFlow.Domain.Enums;
using CashFlow.Domain.Reports;

namespace CashFlow.Domain.Extensions;

public static class PaymentTypeExtension
{
    public static string PaymentTypeToString(this PaymentType paymentType)
    {
        return paymentType switch
        {
            PaymentType.Cash => ResourceReportGenerationMessage.CASH,
            PaymentType.CreditCard => ResourceReportGenerationMessage.CREDIT_CARD,
            PaymentType.DebitCard => ResourceReportGenerationMessage.DEBIT_CARD,
            PaymentType.ElectronicTransfer => ResourceReportGenerationMessage.ELECTRONIC_TRANSFER,
            _ => string.Empty
        };
    }
}