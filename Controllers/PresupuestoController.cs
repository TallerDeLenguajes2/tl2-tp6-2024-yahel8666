using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace MiWebApi.Controllers;
public class PresupuestoController : Controller
{
       private readonly ILogger<PresupuestoController> _logger;

    private PresupuestosRepository _presupuestoRepository;

    public PresupuestoController(ILogger<PresupuestoController> logger)
    {
        _logger = logger;
        _presupuestoRepository = new PresupuestosRepository();
    }

    public IActionResult Index()
    {
        return View(_presupuestoRepository.ObtenerPresupuestos());
    }

    [HttpGet]

    public IActionResult DetallesDelPresupuesto(int id)
    {
        return View(_presupuestoRepository.ObtenerPresupuestoPorId(id));
    }

    [HttpGet]
    public IActionResult AltaPresupuesto()
    {
        ClientesRepository _clientesRepository = new ClientesRepository();
        List<Cliente> Clientes = _clientesRepository.GetAllClientes();
        ViewData["Clientes"] =  Clientes.Select(c=> new SelectListItem
        {
            Value = c.ClienteId.ToString(), 
            Text = c.Nombre
        }).ToList();
        return View();
    }
    
    [HttpPost]
    public IActionResult CrearPresupuesto(CrearPresupuestoVM presupuestoVM)
    {
       if(!ModelState.IsValid) return RedirectToAction ("Index");
       var presupuesto = new Presupuesto(presupuestoVM);
       _presupuestoRepository.CrearPresupuesto(presupuesto);
       return RedirectToAction ("Index");
    }

    [HttpGet]
    public IActionResult AgregarProductoAPresupuesto(int id)
    {
        ProductosRepository repoProductos = new ProductosRepository();
        List<Producto> productos = repoProductos.ObtenerProductos();
        ViewData["Productos"] = productos.Select(p => new SelectListItem
        {
            Value = p.IdProducto.ToString(), 
            Text = p.Descripcion 
        }).ToList();
        var model = new AgregarProductoVM();
        model.IdPresupuesto = id;
        return View(model);
    }

    [HttpPost]

    public IActionResult AgregarProductoEnPresupuesto(AgregarProductoVM infoProducto)
    {
        if(!ModelState.IsValid) return RedirectToAction ("Index");
        _presupuestoRepository.AgregarProducto(infoProducto.IdPresupuesto, infoProducto.IdProducto, infoProducto.Cantidad);
        return RedirectToAction ("Index");
    }
    
    [HttpGet]
    public IActionResult EliminarProductoAPresupuesto(int id)
    {
        Presupuesto presupuesto = _presupuestoRepository.ObtenerPresupuestoPorId(id);
        ViewData["Productos"] = presupuesto.Detalle.Select(p => new SelectListItem
        {
            Value = p.Producto.IdProducto.ToString(), 
            Text = p.Producto.Descripcion 
        }).ToList();
        return View(id);
    }

    [HttpPost]

    public IActionResult EliminarProductoEnPresupuesto(int idPresupuesto, int idProducto)
    {
        _presupuestoRepository.EliminarProducto(idPresupuesto, idProducto);
        return RedirectToAction ("Index");
    }

    [HttpGet]
    public IActionResult ModificarPresupuesto(int id)
    {
        ClientesRepository _clientesRepository = new ClientesRepository();
        List<Cliente> Clientes = _clientesRepository.GetAllClientes();
        ViewData["Clientes"] =  Clientes.Select(c=> new SelectListItem
        {
            Value = c.ClienteId.ToString(), 
            Text = c.Nombre
        }).ToList();
        var presupuesto  = _presupuestoRepository.ObtenerPresupuestoPorId(id);
        var presupuestoVM = new ModificarPresupuestoVM();
        presupuestoVM.IdPresupuesto = id;
        presupuestoVM.FechaCreacion = presupuesto.FechaCreacion;
        return View(presupuestoVM);
    }

    [HttpPost]
    public IActionResult ModificarPresupuesto(ModificarPresupuestoVM presupuestoVM)
    {
        if(!ModelState.IsValid) return RedirectToAction ("Index");
        var presupuesto = new Presupuesto(presupuestoVM);
        _presupuestoRepository.ModificarPresupuesto(presupuesto);
        return RedirectToAction ("Index"); 
    }

    [HttpGet]
    public IActionResult EliminarPresupuesto(int id)
    {
        return View(_presupuestoRepository.ObtenerPresupuestoPorId(id));
    }

    [HttpGet]
    public IActionResult EliminarPresupuestoPorId(int id)
    {
        _presupuestoRepository.EliminarPresupuestoPorId(id);
        return RedirectToAction ("Index"); 
    }
}