// URL base de tu API C# (Asegúrate de que el puerto coincida y que la API esté corriendo)
const API_URL = 'http://localhost:TU_PUERTO_AQUI/api/propietario';

// Se ejecuta automáticamente al cargar la página
document.addEventListener('DOMContentLoaded', obtenerPropietarios);

// --- MÉTODOS CRUD ---

// GET: Obtener y mostrar todos los propietarios
async function obtenerPropietarios() {
    try {
        const respuesta = await fetch('https://localhost:53670/api/propietario');

        if (!respuesta.ok) {
            throw new Error(`Error HTTP: ${respuesta.status}`);
        }
        const propietarios = await respuesta.json();

        const tbody = document.getElementById('tablaPropietariosBody');
        tbody.innerHTML = ''; // Limpiamos la tabla antes de rellenar

        propietarios.forEach(prop => {
            const tr = document.createElement('tr');
            tr.innerHTML = `
                <td>${prop.idPropietario}</td>
                <td>${prop.dni}</td>
                <td>${prop.nombre}</td>
                <td>${prop.apellido}</td>
                <td>${prop.telefono}</td>
                <td>
                    <button class="btn btn-sm btn-warning" onclick="prepararEdicion(${prop.idPropietario})">Editar</button>
                    <button class="btn btn-sm btn-danger" onclick="eliminarPropietario(${prop.idPropietario})">Eliminar</button>
                </td>
            `;
            tbody.appendChild(tr);
        });
    } catch (error) {
        mostrarAlerta('Error al cargar la lista de propietarios.', 'danger');
        console.error(error);
    }
}

// POST / PUT: Crear o actualizar un propietario
async function guardarPropietario() {
    const id = document.getElementById('idPropietario').value;

    // Armamos el objeto con los datos del formulario
    const datosPropietario = {
        dni: document.getElementById('dni').value,
        nombre: document.getElementById('nombre').value,
        apellido: document.getElementById('apellido').value,
        email: document.getElementById('email').value,
        telefono: document.getElementById('telefono').value
    };

    try {
        let respuesta;
        // Si hay ID, es una edición (PUT). Si no hay ID, es uno nuevo (POST).
        if (id) {
            datosPropietario.idPropietario = parseInt(id);
            respuesta = await fetch(`https://localhost:53670/api/propietario/${id}`, {
                method: 'PUT',
                headers: { 'Content-Type': 'application/json' },
                body: JSON.stringify(datosPropietario)
            });
        } else {
            respuesta = await fetch(`https://localhost:53670/api/propietario`, {
                method: 'POST',
                headers: { 'Content-Type': 'application/json' },
                body: JSON.stringify(datosPropietario)
            });
        }

        if (respuesta.ok) {
            // Cerramos el modal de Bootstrap
            const modal = bootstrap.Modal.getInstance(document.getElementById('modalPropietario'));
            modal.hide();

            // Recargamos la tabla y mostramos mensaje
            obtenerPropietarios();
            mostrarAlerta('Guardado correctamente.', 'success');
        } else {
            mostrarAlerta('Error al guardar.', 'danger');
        }
    } catch (error) {
        mostrarAlerta('Error de red al intentar guardar.', 'danger');
    }
}

// DELETE: Eliminar un propietario
async function eliminarPropietario(id) {
    if (confirm('¿Estás seguro de que deseas eliminar este propietario?')) {
        try {
            const respuesta = await fetch(`https://localhost:53670/api/propietario/${id}`, {
                method: 'DELETE'
            });

            if (respuesta.ok) {
                obtenerPropietarios();
                mostrarAlerta('Propietario eliminado.', 'success');
            }
        } catch (error) {
            mostrarAlerta('Error al eliminar.', 'danger');
        }
    }
}

// --- MÉTODOS DE UTILIDAD PARA LA INTERFAZ ---

// Prepara el formulario para un nuevo ingreso (limpia los campos)
function prepararFormularioNuevo() {
    document.getElementById('modalTitle').innerText = 'Nuevo Propietario';
    document.getElementById('formPropietario').reset();
    document.getElementById('idPropietario').value = '';
}

// Obtiene los datos de un propietario para cargarlos en el modal de edición
async function prepararEdicion(id) {
    try {
        // Obtenemos los datos del propietario específico (reutilizamos tu endpoint de buscar por DNI o creamos uno de buscar por ID)
        // Para simplificar aquí, asumimos que recorres la tabla o llamas a la API
        const respuesta = await fetch(API_URL); // Idealmente debería ser fetch(`${API_URL}/id/${id}`)
        const propietarios = await respuesta.json();

        const prop = propietarios.find(p => p.idPropietario === id);

        if (prop) {
            document.getElementById('modalTitle').innerText = 'Editar Propietario';
            document.getElementById('idPropietario').value = prop.idPropietario;
            document.getElementById('dni').value = prop.dni;
            document.getElementById('nombre').value = prop.nombre;
            document.getElementById('apellido').value = prop.apellido;
            document.getElementById('email').value = prop.email;
            document.getElementById('telefono').value = prop.telefono;

            // Abrimos el modal programáticamente
            const modal = new bootstrap.Modal(document.getElementById('modalPropietario'));
            modal.show();
        }
    } catch (error) {
        console.error(error);
    }
}

// Muestra mensajes temporales en pantalla usando alertas de Bootstrap
function mostrarAlerta(mensaje, tipo) {
    const alertContainer = document.getElementById('alertContainer');
    alertContainer.innerHTML = `
        <div class="alert alert-${tipo} alert-dismissible fade show" role="alert">
            ${mensaje}
            <button type="button" class="btn-close" data-bs-dismiss="alert" aria-label="Close"></button>
        </div>
    `;

    // Ocultar automáticamente después de 3 segundos
    setTimeout(() => {
        alertContainer.innerHTML = '';
    }, 3000);
}// JavaScript source code
