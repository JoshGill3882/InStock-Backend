namespace instock_server_application.Businesses.Dtos;

public class StatStockDto
{
    public int AmountChanged { get; }
    public string ReasonForChange { get; }
    public string DateTimeAdded { get; }

    public StatStockDto(int amountChanged, string reasonForChange, string dateTimeAdded)
    {
        AmountChanged = amountChanged;
        ReasonForChange = reasonForChange;
        DateTimeAdded = dateTimeAdded;
    }
}