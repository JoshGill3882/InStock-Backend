using instock_server_application.Users.Dtos;

namespace instock_server_application.Users.Services.Interfaces; 

public interface ICreateAccountService {
    public Task<string> CreateAccount(NewAccountDto newAccountDto);
}