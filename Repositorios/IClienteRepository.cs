public interface IClientesRepository
{
    void CreateCliente(Cliente cliente);
    List<Cliente> GetAllClientes();
    void UpdateCliente(Cliente cliente);
    Cliente GetCliente(int id);
    void DeleteCliente(int id);
}
