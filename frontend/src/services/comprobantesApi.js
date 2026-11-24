import api from "./api";
export const listar = async (signal) => 
{
    const { data } = await api.get("/Comprobantes", { signal });
    return data
}

export const ObtenerformasPago = async (signal) => {
    const { data } = await api.get("/Comprobantes/formas-pago", { signal });
    return data
}
    
export const crear = async (datos) => {
    await api.post("/comprobantes", datos)

}
    
export const actualizar = async (id, datos) => {

   await api.put(`/comprobantes/${id}`, datos)//parsear
}
    
export const darDeBaja = async (id) =>{
    await api.delete(`/comprobantes/${id}`)
}   

    

