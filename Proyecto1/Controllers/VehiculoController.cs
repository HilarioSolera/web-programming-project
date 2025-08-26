using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Modelos;
using Proyecto1.Services.Interfaces;

namespace Proyecto1.Controllers
{
    public class VehiculoController : Controller
    {
        private readonly IServicioVehiculo _servicioVehiculo;
        private readonly IServicioCliente _servicioCliente;
        private const string Modulo = "Vehiculo";

        public VehiculoController(IServicioVehiculo servicioVehiculo, IServicioCliente servicioCliente)
        {
            _servicioVehiculo = servicioVehiculo;
            _servicioCliente = servicioCliente;
        }

        private async Task CargarClientesEnViewBagAsync()
        {
            var clientes = await _servicioCliente.ObtenerTodosAsync();
            ViewBag.Clientes = clientes
                .Select(c => new SelectListItem
                {
                    Value = c.Id.ToString(),
                    Text = $"{c.NombreCompleto} ({c.Identificacion})"
                })
                .OrderBy(x => x.Text)
                .ToList();
        }

        public async Task<IActionResult> Index(string filtro)
        {
            var vehiculos = await _servicioVehiculo.ObtenerTodosAsync();

            if (!string.IsNullOrWhiteSpace(filtro))
            {
                filtro = filtro.Trim().ToUpper();
                vehiculos = vehiculos.Where(v =>
                    v.Placa.ToUpper().Contains(filtro) ||
                    v.Marca.ToUpper().Contains(filtro) ||
                    (v.Cliente?.Identificacion?.ToUpper().Contains(filtro) ?? false)
                ).ToList();
            }

            return View(vehiculos);
        }

        public async Task<IActionResult> Create()
        {
            await CargarClientesEnViewBagAsync();
            return View(new Vehiculo());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Vehiculo vehiculo)
        {
            vehiculo.Placa = vehiculo.Placa?.Trim().ToUpper() ?? string.Empty;

            if (!ModelState.IsValid)
            {
                TempData["MensajeVehiculoXError"] = "⚠️ Verificá los campos requeridos.";
                await CargarClientesEnViewBagAsync();
                return View(vehiculo);
            }

            var vehiculos = await _servicioVehiculo.ObtenerTodosAsync();
            bool placaDuplicada = vehiculos.Any(v =>
                v.Placa.Equals(vehiculo.Placa, StringComparison.OrdinalIgnoreCase));

            if (placaDuplicada)
            {
                TempData["MensajeVehiculoXError"] = "❌ Ya existe un vehículo con esta placa.";
                await CargarClientesEnViewBagAsync();
                return View(vehiculo);
            }

            var (exito, _) = await _servicioVehiculo.AgregarAsync(vehiculo);

            if (exito)
            {
                TempData["MensajeVehiculoXExito"] = "✅ Vehículo registrado correctamente.";
                return RedirectToAction(nameof(Index));
            }
            else
            {
                TempData["MensajeVehiculoXError"] = "❌ No se pudo registrar el vehículo.";
                await CargarClientesEnViewBagAsync();
                return View(vehiculo);
            }
        }

        public async Task<IActionResult> Edit(int id)
        {
            var vehiculo = await _servicioVehiculo.ObtenerPorIdAsync(id);

            if (vehiculo is null)
            {
                TempData["MensajeVehiculoXError"] = "❌ Vehículo no encontrado.";
                return RedirectToAction(nameof(Index));
            }

            await CargarClientesEnViewBagAsync();
            return View(vehiculo);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Vehiculo vehiculo)
        {
            if (id != vehiculo.Id)
                return BadRequest("❌ El ID proporcionado no coincide con el vehículo.");

            vehiculo.Placa = vehiculo.Placa?.Trim().ToUpper() ?? string.Empty;

            if (!ModelState.IsValid)
            {
                TempData["MensajeVehiculoXError"] = "⚠️ Verificá los campos requeridos.";
                await CargarClientesEnViewBagAsync();
                return View(vehiculo);
            }

            var vehiculos = await _servicioVehiculo.ObtenerTodosAsync();
            bool placaDuplicada = vehiculos.Any(v =>
                v.Id != vehiculo.Id &&
                v.Placa.Equals(vehiculo.Placa, StringComparison.OrdinalIgnoreCase));

            if (placaDuplicada)
            {
                TempData["MensajeVehiculoXError"] = "❌ Ya existe otro vehículo con esta placa.";
                await CargarClientesEnViewBagAsync();
                return View(vehiculo);
            }

            var (exito, _) = await _servicioVehiculo.ActualizarAsync(id, vehiculo);

            if (exito)
            {
                TempData["MensajeVehiculoXExito"] = "✅ Vehículo actualizado correctamente.";
                return RedirectToAction(nameof(Index));
            }
            else
            {
                TempData["MensajeVehiculoXError"] = "❌ No se pudo actualizar el vehículo.";
                await CargarClientesEnViewBagAsync();
                return View(vehiculo);
            }
        }

        public async Task<IActionResult> Details(int id)
        {
            var vehiculo = await _servicioVehiculo.ObtenerPorIdAsync(id);

            if (vehiculo is null)
            {
                TempData["MensajeVehiculoXError"] = "❌ Vehículo no encontrado.";
                return RedirectToAction(nameof(Index));
            }

            return View(vehiculo);
        }

        public async Task<IActionResult> Delete(int id)
        {
            var vehiculo = await _servicioVehiculo.ObtenerPorIdAsync(id);

            if (vehiculo is null)
            {
                TempData["MensajeVehiculoXError"] = "❌ Vehículo no encontrado.";
                return RedirectToAction(nameof(Index));
            }

            return View(vehiculo);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            try
            {
                var (exito, _) = await _servicioVehiculo.EliminarAsync(id);

                TempData[$"MensajeVehiculoX{(exito ? "Exito" : "Error")}"] = exito
                    ? "✅ Vehículo eliminado correctamente."
                    : "❌ No se puede eliminar el vehículo porque está vinculado a otros registros.";

                return RedirectToAction(nameof(Index));
            }
            catch (Exception)
            {
                TempData["MensajeVehiculoXError"] = "❌ Error inesperado al intentar eliminar el vehículo.";
                return RedirectToAction(nameof(Index));
            }
        }
    }
}
