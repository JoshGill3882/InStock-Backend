namespace instock_server_application.Businesses.Models; 

public class BusinessIdModel {
    public string BusinessId { get; set; }

    /// <summary>
    /// All Args Constructor
    /// </summary>
    /// <param name="businessId"> Business ID </param>
    public BusinessIdModel(string businessId) {
        BusinessId = businessId;
    }
}