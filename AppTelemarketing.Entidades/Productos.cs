using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppTelemarketing.Entidades
{
    public class Productos
    {
        public int CodProd { get; set; }
        public string NombreProducto { get; set; }
        public string Descripcion { get; set; }
        public decimal Precio { get; set; }
        public int Stock { get; set; }



        public Productos(int codProducto, string nombreProducto, decimal precio, int stock)
        {
            CodProd = codProducto;
            NombreProducto = nombreProducto;
            Precio = precio;
            Stock = stock;

        }
        public Productos(int codProducto, string nombreProducto)
        {
            CodProd = codProducto;
            NombreProducto = nombreProducto;

        }
    }
}
