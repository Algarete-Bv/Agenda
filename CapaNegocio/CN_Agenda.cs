using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using CapaDatos;
using CapaEntidad;


namespace CapaNegocio
{

    public class CN_Agenda
    {
        private CD_Agenda objcd_contactos = new CD_Agenda();

        public List<Agenda> Listar()
        {
            return objcd_contactos.Listar();
        }
        public int Registrar(Agenda obj, out string Mensaje)
        {
            Mensaje = string.Empty;

            if (obj.Nombre == "")
            {
                Mensaje += "Es necesario el NOMBRE del contacto\n";
            }

            if (obj.Apellido == "")
            {
                Mensaje += "Es necesario el APELLIDO del contacto\n";
            }

            //if (obj.Telefono == "___-___-____")
            //{
            //    Mensaje += "Es necesario el TELEFONO del contacto\n";
            //}

            if (Mensaje != string.Empty)
            {
                return 0;
            }
            else
            {
                return objcd_contactos.Registrar(obj, out Mensaje);
            }
        }

        public bool Editar(Agenda obj, out string Mensaje)
        {
            Mensaje = string.Empty;

            if (obj.Nombre == "")
            {
                Mensaje += "Es necesario el NOMBRE del contacto\n";
            }

            if (obj.Apellido == "")
            {
                Mensaje += "Es necesario el APELLIDO del contacto\n";
            }

            if (obj.Telefono == "")
            {
                Mensaje += "Es necesario el TELEFONO del contacto\n";
            }

            if (Mensaje != string.Empty)
            {
                return false;
            }
            else
            {
                return objcd_contactos.Editar(obj, out Mensaje);
            }
        }
        public bool Eliminar(Agenda obj, out string Mensaje)
        {
            return objcd_contactos.Eliminar(obj, out Mensaje);
        }
    }
}
