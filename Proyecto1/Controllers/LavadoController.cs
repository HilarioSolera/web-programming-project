using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Proyecto1.Services.Interfaces;
using Modelos;
using Microsoft.EntityFrameworkCore;

namespace Proyecto1.Controllers
{
    public class LavadoController : Controller
    {
        private readonly IServicioLavado _servicioLavado;
        private readonly IServicioCliente _servicioCliente;
        private readonly IServicioVehiculo _servicioVehiculo;
        private readonly IServicioEmpleado _servicioEmpleado;
        private const string Modulo = "Lavado";

        public LavadoController(
            IServicioLavado servicioLavado,
            IServicioCliente servicioCliente,
            IServicioVehiculo servicioVehiculo,
            IServicioEmpleado servicioEmpleado)
        {
            _servicioLavado = servicioLavado;
            _servicioCliente = servicioCliente;
            _servicioVehiculo = servicioVehiculo;
            _servicioEmpleado = servicioEmpleado;
        }

        public async Task<IActionResult> Index(string? filtro)
        {
            try
            {
                var lavados = await _servicioLavado.ObtenerTodosAsync(filtro);
                var clientes = await _servicioCliente.ObtenerTodosAsync();
                var vehiculos = await _servicioVehiculo.ObtenerTodosAsync();
                var empleados = await _servicioEmpleado.ObtenerTodosAsync();

                var listaIndex = lavados.Select(l => new
                {
                    l.Id,
                    l.Fecha,
                    l.TipoLavado,
                    l.Estado,
                    l.Precio,
                    Cliente = clientes.FirstOrDefault(c => c.Id == l.ClienteId)?.NombreCompleto ?? "Cliente desconocido",
                    Vehiculo = vehiculos.FirstOrDefault(v => v.Id == l.VehiculoId)?.Placa ?? "Vehículo desconocido",
                    Empleado = empleados.FirstOrDefault(e => e.Id == l.EmpleadoId)?.NombreCompleto ?? "No asignado"
                }).ToList();

                ViewBag.Busqueda = filtro;
                return View(listaIndex);
            }
            catch (Exception ex)
            {
                TempData[$"Mensaje{Modulo}XError"] = $"❌ Error al cargar lavados: {ex.Message}";
                return View(new List<object>());
            }
        }

        public async Task<IActionResult> Details(int id)
        {
            try
            {
                var lavado = await _servicioLavado.ObtenerPorIdAsync(id);
                if (lavado is null)
                {
                    TempData[$"Mensaje{Modulo}XError"] = "❌ Lavado no encontrado.";
                    return RedirectToAction(nameof(Index));
                }

                var cliente = await _servicioCliente.ObtenerPorIdAsync(lavado.ClienteId);
                var vehiculo = await _servicioVehiculo.ObtenerPorIdAsync(lavado.VehiculoId);
                var empleado = lavado.EmpleadoId.HasValue
                    ? await _servicioEmpleado.ObtenerPorIdAsync(lavado.EmpleadoId.Value)
                    : null;

                ViewBag.ClienteInfo = cliente is null
                    ? "Cliente desconocido"
                    : $"{cliente.NombreCompleto} ({cliente.Identificacion})";

                ViewBag.VehiculoInfo = vehiculo is null
                    ? "Vehículo desconocido"
                    : $"{vehiculo.Placa} ({vehiculo.Marca} {vehiculo.Modelo})";

                ViewBag.EmpleadoInfo = empleado is null
                    ? string.Empty
                    : $"{empleado.NombreCompleto} ({empleado.Puesto})";

                return View(lavado);
            }
            catch (Exception ex)
            {
                TempData[$"Mensaje{Modulo}XError"] = $"❌ Error inesperado: {ex.Message}";
                return RedirectToAction(nameof(Index));
            }
        }

        public async Task<IActionResult> Create()
        {
            await CargarListasDesplegables();
            return View(new Lavado());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Lavado lavado)
        {
            lavado.Precio = lavado.TipoLavado switch
            {
                "Básico" => 8000,
                "Premium" => 12000,
                "Deluxe" => 20000,
                _ => lavado.Precio
            };

            if (!ModelState.IsValid)
            {
                TempData[$"Mensaje{Modulo}XError"] = "❌ El formulario contiene errores.";
                await CargarListasDesplegables();
                return View(lavado);
            }

            try
            {
                var (exito, _) = await _servicioLavado.AgregarAsync(lavado);
                TempData[$"Mensaje{Modulo}X{(exito ? "Exito" : "Error")}"] = exito
                    ? "✅ Lavado registrado correctamente."
                    : "❌ No se pudo registrar el lavado.";

                return exito ? RedirectToAction(nameof(Index)) : View(lavado);
            }
            catch (Exception ex)
            {
                TempData[$"Mensaje{Modulo}XError"] = $"❌ Error inesperado: {ex.Message}";
                await CargarListasDesplegables();
                return View(lavado);
            }
        }

        public async Task<IActionResult> Edit(int id)
        {
            try
            {
                var lavado = await _servicioLavado.ObtenerPorIdAsync(id);
                if (lavado is null)
                {
                    TempData[$"Mensaje{Modulo}XError"] = "❌ Lavado no encontrado para edición.";
                    return RedirectToAction(nameof(Index));
                }

                await CargarListasDesplegables();
                return View(lavado);
            }
            catch (Exception ex)
            {
                TempData[$"Mensaje{Modulo}XError"] = $"❌ Error inesperado al cargar el lavado: {ex.Message}";
                return RedirectToAction(nameof(Index));
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Lavado lavado)
        {
            if (id != lavado.Id)
                return BadRequest("❌ El ID del lavado no coincide.");

            lavado.Precio = lavado.TipoLavado switch
            {
                "Básico" => 8000,
                "Premium" => 12000,
                "Deluxe" => 20000,
                _ => lavado.Precio
            };

            if (!ModelState.IsValid)
            {
                TempData[$"Mensaje{Modulo}XError"] = "❌ El formulario contiene errores de validación.";
                await CargarListasDesplegables();
                return View(lavado);
            }

            try
            {
                var (exito, _) = await _servicioLavado.ActualizarAsync(id, lavado);
                TempData[$"Mensaje{Modulo}X{(exito ? "Exito" : "Error")}"] = exito
                    ? "✅ Lavado actualizado correctamente."
                    : "❌ No se pudo actualizar el lavado.";

                return exito ? RedirectToAction(nameof(Index)) : View(lavado);
            }
            catch (Exception ex)
            {
                TempData[$"Mensaje{Modulo}XError"] = $"❌ Error inesperado al editar el lavado: {ex.Message}";
                await CargarListasDesplegables();
                return View(lavado);
            }
        }

        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                var lavado = await _servicioLavado.ObtenerPorIdAsync(id);
                if (lavado is null)
                {
                    TempData["MensajeLavadoXError"] = "❌ Lavado no encontrado para eliminación.";
                    return RedirectToAction(nameof(Index));
                }

                // 🔽 Cargar entidades relacionadas
                lavado.Cliente = await _servicioCliente.ObtenerPorIdAsync(lavado.ClienteId);
                lavado.Vehiculo = await _servicioVehiculo.ObtenerPorIdAsync(lavado.VehiculoId);
                lavado.Empleado = lavado.EmpleadoId.HasValue
                    ? await _servicioEmpleado.ObtenerPorIdAsync(lavado.EmpleadoId.Value)
                    : null;

                return View(lavado);
            }
            catch (Exception ex)
            {
                TempData["MensajeLavadoXError"] = $"❌ Error inesperado al cargar el lavado: {ex.Message}";
                return RedirectToAction(nameof(Index));
            }
        }


        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ConfirmarEliminar(int id)
        {
            try
            {
                var (exito, _) = await _servicioLavado.EliminarAsync(id);
                TempData[$"Mensaje{Modulo}X{(exito ? "Exito" : "Error")}"] = exito
                    ? "✅ Lavado eliminado exitosamente."
                    : "❌ No se pudo eliminar el lavado.";

                return RedirectToAction(nameof(Index));
            }
            catch (DbUpdateException)
            {
                TempData[$"Mensaje{Modulo}XError"] = "❌ No se puede eliminar el lavado porque está relacionado con otras entidades.";
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                TempData[$"Mensaje{Modulo}XError"] = $"❌ Error inesperado al eliminar el lavado: {ex.Message}";
                return RedirectToAction(nameof(Index));
            }
        }

        private async Task CargarListasDesplegables()
        {
            var clientes = await _servicioCliente.ObtenerTodosAsync();
            var vehiculos = await _servicioVehiculo.ObtenerTodosAsync();
            var empleados = await _servicioEmpleado.ObtenerTodosAsync();

            ViewBag.Clientes = clientes.Select(c => new SelectListItem
            {
                Value = c.Id.ToString(),
                Text = $"{c.NombreCompleto} ({c.Identificacion})"
            }).OrderBy(x => x.Text).ToList();

            ViewBag.Vehiculos = vehiculos.Select(v => new SelectListItem
            {
                Value = v.Id.ToString(),
                Text = $"{v.Placa} ({v.Marca} {v.Modelo})"
            }).OrderBy(x => x.Text).ToList();

            ViewBag.Empleados = empleados.Select(e => new SelectListItem
            {
                Value = e.Id.ToString(),
                Text = $"{e.NombreCompleto} ({e.Puesto})"
            }).OrderBy(x => x.Text).ToList();
        }
    }
}