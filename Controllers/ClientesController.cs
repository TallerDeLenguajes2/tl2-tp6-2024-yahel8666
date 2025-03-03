using Microsoft.AspNetCore.Mvc;
public class ClientesController : Controller
{
    readonly ILogger<ClientesController> _logger;

    ClientesRepository _clientesRepository;

    public ClientesController(ILogger<ClientesController> logger)
    {
        _logger = logger;
        _clientesRepository = new ClientesRepository();
    }
    
    public IActionResult Index()
    {
        return View(_clientesRepository.GetAllClientes());
    }

    [HttpGet]
    public IActionResult ModificarCliente(int id)
    {
        var cliente  = _clientesRepository.GetCliente(id);
        var clienteVM = new ModificarClienteVM(cliente); 
        return View(clienteVM);
    }

    [HttpPost]
    public IActionResult ModificarCliente(ModificarClienteVM clienteVM)
    {
        if(!ModelState.IsValid) return RedirectToAction ("Index");
        var cliente = new Cliente(clienteVM);
        _clientesRepository.UpdateCliente(cliente);
        return RedirectToAction ("Index"); 
    }

    [HttpGet]
    public IActionResult EliminarCliente(int id)
    {
        return View(_clientesRepository.GetCliente(id));
    }

    [HttpGet]
    public IActionResult EliminarClientePorId(int id)
    {
        _clientesRepository.DeleteCliente(id);
        return RedirectToAction ("Index"); 
    }

    [HttpGet]
    public IActionResult CrearCliente()
    {
        return View();
    }

    [HttpPost]
    public IActionResult CrearCliente(CrearClienteVM clienteVM)
    {
        if(!ModelState.IsValid) return RedirectToAction ("Index");
        var cliente = new Cliente(clienteVM);
        _clientesRepository.CreateCliente(cliente);
        return RedirectToAction ("Index");
    }
}