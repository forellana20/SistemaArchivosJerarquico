using System;
using System.Collections.Generic;

namespace SistemaArchivosJerarquico.Models
{
    /// <summary>
    /// Clase NodoArchivo que representa cada elemento del sistema de archivos jerárquico
    /// Implementa la estructura básica de un nodo de árbol con referencias a hijos
    /// </summary>
    public class NodoArchivo
    {
        #region Propiedades
        public string Nombre { get; set; }
        public TipoNodo Tipo { get; set; }
        public List<NodoArchivo> Hijos { get; set; }
        public NodoArchivo Padre { get; set; }
        #endregion

        #region Constructor
        /// <summary>
        /// Constructor para crear un nuevo nodo
        /// Complejidad: O(1) - operación constante
        /// </summary>
        /// <param name="nombre">Nombre del archivo o carpeta</param>
        /// <param name="tipo">Tipo de nodo (Carpeta o Archivo)</param>
        /// <param name="padre">Referencia al nodo padre</param>
        public NodoArchivo(string nombre, TipoNodo tipo, NodoArchivo padre = null)
        {
            Nombre = nombre;
            Tipo = tipo;
            Padre = padre;

            // Solo las carpetas pueden tener hijos (nodos internos)
            if (tipo == TipoNodo.Carpeta)
            {
                Hijos = new List<NodoArchivo>();
            }
        }
        #endregion

        #region Métodos de información
        /// <summary>
        /// Determina si el nodo es una hoja (archivo sin hijos)
        /// Complejidad: O(1)
        /// </summary>
        /// <returns>True si es hoja, False si es nodo interno</returns>
        public bool EsHoja()
        {
            return Tipo == TipoNodo.Archivo || (Hijos != null && Hijos.Count == 0);
        }

        /// <summary>
        /// Calcula la altura de este nodo (distancia a la hoja más lejana)
        /// Complejidad: O(n) donde n es el número de descendientes
        /// </summary>
        /// <returns>Altura del nodo</returns>
        public int CalcularAltura()
        {
            // Caso base: si es hoja, altura = 0
            if (EsHoja())
                return 0;

            int alturaMaxima = 0;

            // Recursión: encontrar la altura máxima de los hijos
            foreach (var hijo in Hijos)
            {
                int alturaHijo = hijo.CalcularAltura();
                if (alturaHijo > alturaMaxima)
                    alturaMaxima = alturaHijo;
            }

            return alturaMaxima + 1;
        }

        /// <summary>
        /// Calcula el nivel de este nodo (distancia desde la raíz)
        /// Complejidad: O(h) donde h es la altura del árbol
        /// </summary>
        /// <returns>Nivel del nodo</returns>
        public int CalcularNivel()
        {
            int nivel = 0;
            NodoArchivo actual = this.Padre;

            // Recorrer hacia arriba hasta la raíz
            while (actual != null)
            {
                nivel++;
                actual = actual.Padre;
            }

            return nivel;
        }
        #endregion

        #region Métodos de manipulación
        /// <summary>
        /// Agrega un hijo al nodo actual (solo si es carpeta)
        /// Complejidad: O(n) por la verificación de duplicados
        /// </summary>
        /// <param name="hijo">Nodo hijo a agregar</param>
        /// <returns>True si se agregó exitosamente</returns>
        public bool AgregarHijo(NodoArchivo hijo)
        {
            // Validar que solo las carpetas pueden tener hijos
            if (Tipo != TipoNodo.Carpeta)
            {
                return false;
            }

            // Verificar duplicados - búsqueda lineal O(n)
            foreach (var hijoExistente in Hijos)
            {
                if (hijoExistente.Nombre.Equals(hijo.Nombre, StringComparison.OrdinalIgnoreCase))
                {
                    return false;
                }
            }

            // Establecer relación padre-hijo
            hijo.Padre = this;
            Hijos.Add(hijo);
            return true;
        }
        #endregion

        public override string ToString()
        {
            string tipoStr = Tipo == TipoNodo.Carpeta ? "[CARPETA]" : "[ARCHIVO]";
            return $"{tipoStr} {Nombre}";
        }
    }
}