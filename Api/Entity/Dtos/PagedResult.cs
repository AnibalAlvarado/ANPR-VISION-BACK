using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entity.Dtos
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    public class PagedResult<T>
    {
        /// <summary>
        /// Lista de elementos devueltos para la página actual.
        /// </summary>
        //[Required(ErrorMessage = "La lista de elementos no puede ser nula.")]
        public List<T> Items { get; set; } = new();

        /// <summary>
        /// Número total de registros disponibles.
        /// </summary>
        //[Range(0, int.MaxValue, ErrorMessage = "El total de registros no puede ser negativo.")]
        public int TotalCount { get; set; }

        /// <summary>
        /// Número de la página actual (1 en adelante).
        /// </summary>
        //[Range(1, int.MaxValue, ErrorMessage = "El número de página debe ser mayor o igual a 1.")]
        public int Page { get; set; }

        /// <summary>
        /// Cantidad de elementos por página.
        /// </summary>
        //[Range(1, 500, ErrorMessage = "El tamaño de página debe estar entre 1 y 500.")]
        public int PageSize { get; set; }
    }

}
