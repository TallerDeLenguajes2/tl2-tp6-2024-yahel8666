using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

public class PresupuestoController : Controller
{

    private readonly ILogger<PresupuestoController> _logger;

    private IPresupuestoRepository _presupuestoRepository;

    private IProductosRepository _productosRepository;

    private IClientesRepository _clientesRepository;
    public PresupuestoController(ILogger<PresupuestoController> logger, IPresupuestoRepository _presupuestoRepository, IProductosRepository _productosRepository, IClientesRepository _clientesRepository)
    {
        _logger = logger;
        this._presupuestoRepository = _presupuestoRepository;
        this._productosRepository = _productosRepository;
        this._clientesRepository = _clientesRepository;
    }

    public IActionResult Index()
    {
        try
        {
            if (!IsLoggedIn()) return RedirectToAction("Index", "Login");
            ViewData["EsAdmin"] = HttpContext.Session.GetString("AccessLevel") == "Admin";
            return View(_presupuestoRepository.ObtenerPresupuestos());
        }
        catch (Exception ex)
        {
            _logger.LogError(ex.ToString());
            return RedirectToAction("Index", "Home");
        }
    }

    [HttpGet]

    public IActionResult DetallesDelPresupuesto(int id)
    {
        try
        {
            if (!IsLoggedIn()) return RedirectToAction("Index", "Login");
            return View(_presupuestoRepository.ObtenerPresupuestoPorId(id));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex.ToString());
            return RedirectToAction("Index");
        }
    }

    [HttpGet]
    public IActionResult AltaPresupuesto()
    {
        try
        {
            if (!IsLoggedIn())
                return RedirectToAction("Index", "Login");

            if (!IsAdmin())
            {
                TempData["ErrorMessage"] = "No tienes permisos para realizar esta acción.";
                return RedirectToAction("Index");
            }

            List<Cliente> Clientes = _clientesRepository.ObtenerClientes();
            ViewData["Clientes"] = Clientes.Select(c => new SelectListItem
            {
                Value = c.ClienteId.ToString(),
                Text = c.Nombre
            }).ToList();

            return View();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex.ToString());
            return RedirectToAction("Index");
        }
    }

    [HttpPost]
    public IActionResult CrearPresupuesto(AltaPresupuestoViewModel presupuestoVM)
    {
        try
        {
            if (!IsLoggedIn()) return RedirectToAction("Index", "Login");

            if (!IsAdmin())
            {
                TempData["ErrorMessage"] = "No tienes permisos para realizar esta acción.";
                return RedirectToAction("Index");
            }

            if (!ModelState.IsValid)
                return RedirectToAction("Index");

            var presupuesto = new Presupuesto(presupuestoVM);
            _presupuestoRepository.CrearPresupuesto(presupuesto);
            return RedirectToAction("Index");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex.ToString());
            return RedirectToAction("AltaPresupuesto");
        }
    }

    [HttpGet]

    public IActionResult AgregarProductoAPresupuesto(int id)
    {
        try
        {
            if (!IsLoggedIn()) return RedirectToAction("Index", "Login");

            if (!IsAdmin())
            {
                TempData["ErrorMessage"] = "No tienes permisos para realizar esta acción.";
                return RedirectToAction("Index");
            }

            List<Producto> productos = _productosRepository.ObtenerProductos();
            ViewData["Productos"] = productos.Select(p => new SelectListItem
            {
                Value = p.IdProducto.ToString(),
                Text = p.Descripcion
            }).ToList();

            var model = new AgregarProductoAPresuViewModel();
            model.IdPresupuesto = id;

            return View(model);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex.ToString());
            return RedirectToAction("Index");
        }
    }

    [HttpPost]
    public IActionResult AgregarProductoEnPresupuesto(AgregarProductoAPresuViewModel infoProducto)
    {
        try
        {
            if (!IsLoggedIn()) return RedirectToAction("Index", "Login");

            if (!IsAdmin())
            {
                TempData["ErrorMessage"] = "No tienes permisos para realizar esta acción.";
                return RedirectToAction("Index");
            }

            if (!ModelState.IsValid)
                return RedirectToAction("Index");

            _presupuestoRepository.AgregarProducto(infoProducto.IdPresupuesto, infoProducto.IdProducto, infoProducto.Cantidad);
            return RedirectToAction("Index");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex.ToString());
            return RedirectToAction("Index");
        }
    }

    [HttpGet]
    public IActionResult EliminarProductoAPresupuesto(int id)
    {
        try
        {
            if (!IsLoggedIn()) return RedirectToAction("Index", "Login");

            if (!IsAdmin())
            {
                TempData["ErrorMessage"] = "No tienes permisos para realizar esta acción.";
                return RedirectToAction("Index");
            }

            Presupuesto presupuesto = _presupuestoRepository.ObtenerPresupuestoPorId(id);
            ViewData["Productos"] = presupuesto.Detalle.Select(p => new SelectListItem
            {
                Value = p.Producto.IdProducto.ToString(),
                Text = p.Producto.Descripcion
            }).ToList();

            return View(id);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex.ToString());
            return RedirectToAction("Index");
        }
    }
    public IActionResult EliminarProductoEnPresupuesto(int idPresupuesto, int idProducto)
    {
        try
        {
            if (!IsLoggedIn()) return RedirectToAction("Index", "Login");

            if (!IsAdmin())
            {
                TempData["ErrorMessage"] = "No tienes permisos para realizar esta acción.";
                return RedirectToAction("Index");
            }

            _presupuestoRepository.EliminarProducto(idPresupuesto, idProducto);
            return RedirectToAction("Index");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex.ToString());
            return RedirectToAction("Index");
        }
    }

    [HttpGet]
    public IActionResult ModificarPresupuesto(int id)
    {
        try
        {
            if (!IsLoggedIn()) return RedirectToAction("Index", "Login");

            if (!IsAdmin())
            {
                TempData["ErrorMessage"] = "No tienes permisos para realizar esta acción.";
                return RedirectToAction("Index");
            }

            List<Cliente> Clientes = _clientesRepository.ObtenerClientes();
            ViewData["Clientes"] = Clientes.Select(c => new SelectListItem
            {
                Value = c.ClienteId.ToString(),
                Text = c.Nombre
            }).ToList();

            var presupuesto = _presupuestoRepository.ObtenerPresupuestoPorId(id);
            var presupuestoVM = new ModificarPresupuestoViewModel
            {
                IdPresupuesto = id,
                FechaCreacion = presupuesto.FechaCreacion
            };

            return View(presupuestoVM);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex.ToString());
            return RedirectToAction("Index");
        }
    }

    [HttpPost]
    public IActionResult ModificarPresupuesto(ModificarPresupuestoViewModel presupuestoVM)
    {
        try
        {
            if (!IsLoggedIn()) return RedirectToAction("Index", "Login");

            if (!IsAdmin())
            {

                TempData["ErrorMessage"] = "No tienes permisos para realizar esta acción.";
                return RedirectToAction("Index");
            }

            if (!ModelState.IsValid)
                return RedirectToAction("Index");

            var presupuesto = new Presupuesto(presupuestoVM);
            _presupuestoRepository.ModificarPresupuesto(presupuesto);
            return RedirectToAction("Index");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex.ToString());
            ViewBag.ErrorMessage = "No se pudo modificar el presupuesto";
            return RedirectToAction("Index");
        }
    }

    [HttpGet]
    public IActionResult EliminarPresupuesto(int id)
    {
        try
        {
            if (!IsLoggedIn()) return RedirectToAction("Index", "Login");

            if (!IsAdmin())
            {
                TempData["ErrorMessage"] = "No tienes permisos para realizar esta acción.";
                return RedirectToAction("Index");
            }

            return View(_presupuestoRepository.ObtenerPresupuestoPorId(id));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex.ToString());
            ViewBag.ErrorMessage = "No se pudo cargar el presupuesto para eliminar";
            return RedirectToAction("Index");
        }
    }

    [HttpGet]
    public IActionResult EliminarPresupuestoPorId(int id)
    {
        try
        {
            if (!IsLoggedIn()) return RedirectToAction("Index", "Login");

            if (!IsAdmin())
            {
                TempData["ErrorMessage"] = "No tienes permisos para realizar esta acción.";
                return RedirectToAction("Index");
            }

            _presupuestoRepository.EliminarPresupuestoPorId(id);
            return RedirectToAction("Index");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex.ToString());
            return RedirectToAction("Index");
        }
    }
    private bool IsAdmin()
    {
        return HttpContext.Session.GetString("AccessLevel") == "Admin";
    }

    private bool IsLoggedIn()
    {
        return !string.IsNullOrEmpty(HttpContext.Session.GetString("User"));
    }

}