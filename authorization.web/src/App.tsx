import { useCallback, useEffect, useState } from 'react';
import axios from 'axios';
import { DataGrid, GridRowModel } from '@mui/x-data-grid';

const API_URL = import.meta.env.VITE_API != null ? import.meta.env.VITE_API :'https://localhost:44358/api';
console.log(API_URL);
const columns = [
    { field: 'id', headerName: 'ID', width: 70, editable: false },
    { field: 'employeeName', headerName: 'Nombre', width: 130, editable: true },
    { field: 'employeeLastName', headerName: 'Apellido', width: 130, editable: true },
    { field: 'permissionTypeId', headerName: 'Tipo Permiso', width: 130, editable: true },
    { field: 'permissionDate', headerName: 'Fecha', width: 130, editable: true },
];

function App() {
    const [rows, setRows] = useState([]);

    // Función para obtener datos de la API
    const obtenerDatos = async () => {
        try {
            const respuesta = await axios.get(`${API_URL}/Authorization`); // Reemplaza esto con la ruta correcta
            setRows(respuesta.data);
        } catch (error) {
            console.error('Hubo un error al obtener los datos:', error);
        }
    };

    // Función para guardar datos en la API
    const guardarDatos = useCallback(async (nuevosDatos: GridRowModel, anteriorDatos: GridRowModel) => {
        try {
            await axios.patch(`${API_URL}/Authorization`, nuevosDatos); // Reemplaza esto con la ruta correcta
            obtenerDatos(); // Vuelve a obtener los datos después de guardar para actualizar la tabla
            return nuevosDatos;
        } catch (error) {
            console.error('Hubo un error al guardar los datos:', error);
            return anteriorDatos;
        }
    }, [columns, rows]);

    // Obtiene los datos cuando se monta el componente
    useEffect(() => {
        obtenerDatos();
    }, []);

  return (
    <>
          <div style={{ height: 400, width: '100%' }}>
              <DataGrid
                  rows={rows}
                  columns={columns}
                  autoPageSize={true}
                  processRowUpdate={guardarDatos}
              />
          </div>
    </>
  )
}

export default App
