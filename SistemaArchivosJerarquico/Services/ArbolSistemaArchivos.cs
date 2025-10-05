using System;
using System.Collections.Generic;
using System.Linq;
using SistemaArchivosJerarquico.Models;

namespace SistemaArchivosJerarquico.Services
{
    /// <summary>
    /// Clase principal que implementa el sistema de archivos como un árbol jerárquico
    /// Basada en los conceptos de TAD Árbol vistos en clase
    /// </summary>
    public class ArbolSistemaArchivos
    {
        #region Propiedades
        public NodoArchivo Raiz { get; private set; }
        #endregion

        #region Constructor
        /// <summary>
        /// Constructor que inicializa el árbol con un nodo raíz
        /// Complejidad: O(1)
        /// </summary>
        /// <param name="nombreRaiz">Nombre del directorio raíz</param>
        public ArbolSistemaArchivos(string nombreRaiz = "root")
        {
            Raiz = new NodoArchivo(nombreRaiz, TipoNodo.Carpeta);
        }
        #endregion

        #region Métodos de inserción
        /// <summary>
        /// Agrega un nuevo nodo al árbol especificando la ruta del padre
        /// Complejidad: O(h + n) donde h es altura y n es número de hijos para verificar duplicados
        /// </summary>
        /// <param name="rutaPadre">Ruta del nodo padre</param>
        /// <param name="nombreNodo">Nombre del nuevo nodo</param>
        /// <param name="tipo">Tipo del nuevo nodo</param>
        /// <returns>True si se agregó exitosamente</returns>
        public bool AgregarNodo(string rutaPadre, string nombreNodo, TipoNodo tipo)
        {
            // Buscar el nodo padre - O(h) en el peor caso
            NodoArchivo padre = BuscarNodoPorRuta(rutaPadre);
            if (padre == null)
            {
                return false;
            }

            if (padre.Tipo != TipoNodo.Carpeta)
            {
                return false;
            }

            // Crear y agregar el nuevo nodo
            NodoArchivo nuevoNodo = new NodoArchivo(nombreNodo, tipo, padre);
            return padre.AgregarHijo(nuevoNodo);
        }
        #endregion

        #region Métodos de recorrido
        /// <summary>
        /// Recorrido en PREORDEN (Raíz -> Izquierda -> Derecha)
        /// Implementación recursiva según algoritmo visto en clase
        /// Complejidad: O(n) donde n es el número total de nodos
        /// </summary>
        /// <param name="nodo">Nodo desde donde iniciar (null = raíz)</param>
        /// <param name="resultado">Lista para almacenar el resultado</param>
        public List<string> RecorridoPreorden(NodoArchivo nodo = null, List<string> resultado = null)
        {
            if (resultado == null) resultado = new List<string>();
            if (nodo == null) nodo = Raiz;

            // 1. Procesar el nodo actual (VISITAR RAÍZ)
            resultado.Add($"{new string(' ', nodo.CalcularNivel() * 2)}{nodo}");

            // 2. Recorrer subárboles (HIJOS) de izquierda a derecha
            if (nodo.Tipo == TipoNodo.Carpeta && nodo.Hijos != null)
            {
                foreach (var hijo in nodo.Hijos)
                {
                    RecorridoPreorden(hijo, resultado); // Llamada recursiva
                }
            }

            return resultado;
        }

        /// <summary>
        /// Recorrido en POSTORDEN (Izquierda -> Derecha -> Raíz)
        /// Implementación recursiva según algoritmo visto en clase
        /// Complejidad: O(n) donde n es el número total de nodos
        /// </summary>
        /// <param name="nodo">Nodo desde donde iniciar</param>
        /// <param name="resultado">Lista para almacenar el resultado</param>
        public List<string> RecorridoPostorden(NodoArchivo nodo = null, List<string> resultado = null)
        {
            if (resultado == null) resultado = new List<string>();
            if (nodo == null) nodo = Raiz;

            // 1. Recorrer subárboles primero (HIJOS)
            if (nodo.Tipo == TipoNodo.Carpeta && nodo.Hijos != null)
            {
                foreach (var hijo in nodo.Hijos)
                {
                    RecorridoPostorden(hijo, resultado); // Llamada recursiva
                }
            }

            // 2. Procesar el nodo actual al final (VISITAR RAÍZ)
            resultado.Add($"{new string(' ', nodo.CalcularNivel() * 2)}{nodo}");

            return resultado;
        }

        /// <summary>
        /// Recorrido en ANCHURA (por niveles)
        /// Utiliza una cola para implementar BFS según teoría vista
        /// Complejidad: O(n) donde n es el número total de nodos
        /// </summary>
        public List<string> RecorridoAnchura()
        {
            List<string> resultado = new List<string>();
            if (Raiz == null) return resultado;

            // Usar cola para recorrido por niveles (FIFO)
            Queue<(NodoArchivo nodo, int nivel)> cola = new Queue<(NodoArchivo, int)>();
            cola.Enqueue((Raiz, 0));

            int nivelActual = -1;

            while (cola.Count > 0)
            {
                var (nodo, nivel) = cola.Dequeue();

                // Mostrar separador de nivel
                if (nivel != nivelActual)
                {
                    resultado.Add($"\n--- NIVEL {nivel} ---");
                    nivelActual = nivel;
                }

                resultado.Add($"  {nodo}");

                // Agregar hijos a la cola para procesamiento posterior
                if (nodo.Tipo == TipoNodo.Carpeta && nodo.Hijos != null)
                {
                    foreach (var hijo in nodo.Hijos)
                    {
                        cola.Enqueue((hijo, nivel + 1));
                    }
                }
            }

            return resultado;
        }
        #endregion

        #region Métodos de búsqueda
        /// <summary>
        /// Búsqueda de un nodo por nombre usando DFS (Depth-First Search)
        /// Implementación recursiva según algoritmos de búsqueda vistos
        /// Complejidad: O(n) en el peor caso
        /// </summary>
        /// <param name="nombre">Nombre del nodo a buscar</param>
        /// <param name="nodoInicial">Nodo desde donde iniciar la búsqueda</param>
        /// <returns>Nodo encontrado o null</returns>
        public NodoArchivo BuscarNodo(string nombre, NodoArchivo nodoInicial = null)
        {
            if (nodoInicial == null)
                nodoInicial = Raiz;

            // Caso base: encontramos el nodo
            if (nodoInicial.Nombre.Equals(nombre, StringComparison.OrdinalIgnoreCase))
            {
                return nodoInicial;
            }

            // Recursión: buscar en los hijos (DFS)
            if (nodoInicial.Tipo == TipoNodo.Carpeta && nodoInicial.Hijos != null)
            {
                foreach (var hijo in nodoInicial.Hijos)
                {
                    var resultado = BuscarNodo(nombre, hijo); // Llamada recursiva
                    if (resultado != null)
                        return resultado;
                }
            }

            return null; // No encontrado
        }

        /// <summary>
        /// Búsqueda por ruta absoluta
        /// Navega el árbol siguiendo el camino especificado
        /// Complejidad: O(h * m) donde h es altura y m es longitud promedio de nombres
        /// </summary>
        /// <param name="ruta">Ruta absoluta del nodo</param>
        /// <returns>Nodo encontrado o null</returns>
        public NodoArchivo BuscarNodoPorRuta(string ruta)
        {
            if (string.IsNullOrEmpty(ruta) || ruta == "/")
                return Raiz;

            // Limpiar y dividir la ruta
            ruta = ruta.Trim('/');
            string[] partes = ruta.Split('/', StringSplitOptions.RemoveEmptyEntries);

            NodoArchivo actual = Raiz;
            int inicio = 0;

            // Si la primera parte es el nombre de la raíz, omitirla
            if (partes.Length > 0 && partes[0].Equals(Raiz.Nombre, StringComparison.OrdinalIgnoreCase))
            {
                inicio = 1;
            }

            // Navegar por cada parte de la ruta
            for (int i = inicio; i < partes.Length; i++)
            {
                if (actual.Tipo != TipoNodo.Carpeta || actual.Hijos == null)
                    return null;

                // Búsqueda lineal en los hijos - O(n)
                NodoArchivo siguienteNodo = null;
                foreach (var hijo in actual.Hijos)
                {
                    if (hijo.Nombre.Equals(partes[i], StringComparison.OrdinalIgnoreCase))
                    {
                        siguienteNodo = hijo;
                        break;
                    }
                }

                if (siguienteNodo == null)
                    return null;

                actual = siguienteNodo;
            }

            return actual;
        }
        #endregion

        #region Métodos de información
        /// <summary>
        /// Obtiene la ruta absoluta de un nodo
        /// Construye el camino desde el nodo hasta la raíz
        /// Complejidad: O(h) donde h es la altura del nodo
        /// </summary>
        /// <param name="nodo">Nodo del cual obtener la ruta</param>
        /// <returns>Ruta absoluta como string</returns>
        public string ObtenerRutaAbsoluta(NodoArchivo nodo)
        {
            if (nodo == null) return "";

            List<string> partes = new List<string>();
            NodoArchivo actual = nodo;

            // Construir la ruta desde el nodo hacia la raíz
            while (actual != null)
            {
                partes.Insert(0, actual.Nombre); // Insertar al inicio - O(n)
                actual = actual.Padre;
            }

            return "/" + string.Join("/", partes);
        }

        /// <summary>
        /// Cuenta el número total de carpetas y archivos en el árbol
        /// Implementación recursiva
        /// Complejidad: O(n) donde n es el número total de nodos
        /// </summary>
        /// <param name="nodo">Nodo desde donde contar</param>
        /// <returns>Tupla con (carpetas, archivos)</returns>
        public (int carpetas, int archivos) ContarElementos(NodoArchivo nodo = null)
        {
            if (nodo == null)
                nodo = Raiz;

            int carpetas = 0;
            int archivos = 0;

            if (nodo.Tipo == TipoNodo.Carpeta)
            {
                carpetas = 1; // Contar este nodo como carpeta

                // Recursión: contar en los hijos
                if (nodo.Hijos != null)
                {
                    foreach (var hijo in nodo.Hijos)
                    {
                        var (subCarpetas, subArchivos) = ContarElementos(hijo);
                        carpetas += subCarpetas;
                        archivos += subArchivos;
                    }
                }
            }
            else
            {
                archivos = 1; // Contar este nodo como archivo
            }

            return (carpetas, archivos);
        }

        /// <summary>
        /// Calcula la altura total del árbol
        /// Complejidad: O(n) donde n es el número total de nodos
        /// </summary>
        /// <returns>Altura del árbol</returns>
        public int CalcularAlturaArbol()
        {
            return Raiz?.CalcularAltura() ?? 0;
        }

        /// <summary>
        /// Calcula la altura de un nodo específico por su nombre
        /// Complejidad: O(n) para buscar + O(m) para calcular altura
        /// </summary>
        /// <param name="nombreNodo">Nombre del nodo</param>
        /// <returns>Altura del nodo o -1 si no se encuentra</returns>
        public int CalcularAlturaNodo(string nombreNodo)
        {
            var nodo = BuscarNodo(nombreNodo);
            return nodo?.CalcularAltura() ?? -1;
        }

        /// <summary>
        /// Muestra la estructura del árbol de forma visual
        /// Utiliza caracteres especiales para representar la jerarquía
        /// Complejidad: O(n) donde n es el número total de nodos
        /// </summary>
        /// <param name="nodo">Nodo desde donde mostrar</param>
        /// <param name="prefijo">Prefijo para la indentación</param>
        /// <param name="esUltimo">Indica si es el último hijo</param>
        /// <param name="resultado">Lista para almacenar el resultado</param>
        public List<string> MostrarEstructura(NodoArchivo nodo = null, string prefijo = "", bool esUltimo = true, List<string> resultado = null)
        {
            if (resultado == null) resultado = new List<string>();
            if (nodo == null) nodo = Raiz;

            // Mostrar el nodo actual con formato de árbol
            resultado.Add(prefijo + (esUltimo ? "└── " : "├── ") + nodo.Nombre);

            // Mostrar los hijos recursivamente
            if (nodo.Tipo == TipoNodo.Carpeta && nodo.Hijos != null && nodo.Hijos.Count > 0)
            {
                for (int i = 0; i < nodo.Hijos.Count; i++)
                {
                    bool esUltimoHijo = (i == nodo.Hijos.Count - 1);
                    string nuevoPrefijo = prefijo + (esUltimo ? "    " : "│   ");
                    MostrarEstructura(nodo.Hijos[i], nuevoPrefijo, esUltimoHijo, resultado);
                }
            }

            return resultado;
        }

        /// <summary>
        /// Obtiene estadísticas completas del sistema de archivos
        /// Complejidad: O(n) para recorrer todo el árbol
        /// </summary>
        public Dictionary<string, object> ObtenerEstadisticas()
        {
            var (carpetas, archivos) = ContarElementos();
            int altura = CalcularAlturaArbol();
            int totalNodos = carpetas + archivos;
            double alturaIdeal = Math.Log2(totalNodos + 1);

            return new Dictionary<string, object>
            {
                ["TotalCarpetas"] = carpetas,
                ["TotalArchivos"] = archivos,
                ["TotalNodos"] = totalNodos,
                ["AlturaArbol"] = altura,
                ["NodoRaiz"] = Raiz.Nombre,
                ["AlturaIdeal"] = Math.Round(alturaIdeal, 2),
                ["Eficiencia"] = altura <= alturaIdeal * 1.5 ? "Buena" : "Mejorable"
            };
        }
        #endregion
    }
}