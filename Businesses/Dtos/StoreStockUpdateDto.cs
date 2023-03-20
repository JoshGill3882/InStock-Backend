﻿using instock_server_application.Shared.Dto;

namespace instock_server_application.Businesses.Dtos; 

public class StoreStockUpdateDto {

    public string BusinessId { get; }
    public string ItemSku { get; }
    public int ChangeStockAmountBy { get; }
    public string ReasonForChange { get; }

    public StoreStockUpdateDto(string businessId, string itemSku, int changeStockAmountBy, string reasonForChange) {
        BusinessId = businessId;
        ItemSku = itemSku;
        ChangeStockAmountBy = changeStockAmountBy;
        ReasonForChange = reasonForChange;
    }
}