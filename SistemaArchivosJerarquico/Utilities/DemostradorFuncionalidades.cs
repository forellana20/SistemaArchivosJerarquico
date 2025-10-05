using System;
using System.Collections.Generic;
using SistemaArchivosJerarquico.Models;
using SistemaArchivosJerarquico.Services;

namespace SistemaArchivosJerarquico.Utilities
{
    /// <summary>
    /// Clase utilitaria para demostrar las funcionalidades del sistema
    /// Útil para pruebas y demostraciones en consola
    /// </summary>
    public static class DemostradorFuncionalidades
    {
        /// <summary>
        /// Crea y retorna un sistema de archivos con la estructura de ejemplo
        /// </summary>
        /// <returns>Sistema de archivos configurado</returns>
        public static ArbolSistemaArchivos CrearEstructuraEjemplo()
        {
            ArbolSistemaArchivos sistema = new ArbolSistemaArchivos("root");

            // Crear carpetas principales
            sistema.AgregarNodo("/root", "documentos", TipoNodo.Carpeta);
            sistema.AgregarNodo("/root", "fotos", TipoNodo.Carpeta);
            sistema.AgregarNodo("/root", "sistema", TipoNodo.Carpeta);

            // Agregar archivos a documentos
            sistema.AgregarNodo("/root/documentos", "cv.docx", TipoNodo.Archivo);
            sistema.AgregarNodo("/root/documentos", "tesis.pdf", TipoNodo.Archivo);

            // Crear subcarpeta en fotos y agregar archivos
            sistema.AgregarNodo("/root/fotos", "vacaciones", TipoNodo.Carpeta);
            sistema.AgregarNodo("/root/fotos", "perfil.jpg", TipoNodo.Archivo);
            sistema.AgregarNodo("/root/fotos/vacaciones", "playa.jpg", TipoNodo.Archivo);
            sistema.AgregarNodo("/root/fotos/vacaciones", "montaña.jpg", TipoNodo.Archivo);

            // Agregar archivo a sistema
            sistema.AgregarNodo("/root/sistema", "config.sys", TipoNodo.Archivo);

            return sistema;
        }

        /// <summary>
        /// Ejecuta una demostración completa en consola
        /// Útil para pruebas sin interfaz gráfica
        /// </summary>
        public static void EjecutarDemostracionConsola()
        {
            Console.WriteLine("=== DEMOSTRACIÓN DEL SISTEMA DE ARCHIVOS JERÁRQUICO ===");
            Console.WriteLine("Implementación basada en TAD Árbol\n");

            // Crear sistema con estructura de ejemplo
            var sistema = CrearEstructuraEjemplo();

            // Mostrar estructura
            Console.WriteLine("=== ESTRUCTURA DEL SISTEMA ===");
            var estructura = sistema.MostrarEstructura();
            foreach (var linea in estructura)
            {
                Console.WriteLine(linea);
            }

            // Mostrar recorridos
            Console.WriteLine("\n=== RECORRIDO PREORDEN ===");
            var preorden = sistema.RecorridoPreorden();
            foreach (var linea in preorden)
            {
                Console.WriteLine(linea);
            }

            Console.WriteLine("\n=== RECORRIDO POSTORDEN ===");
            var postorden = sistema.RecorridoPostorden();
            foreach (var linea in postorden)
            {
                Console.WriteLine(linea);
            }

            Console.WriteLine("\n=== RECORRIDO EN ANCHURA ===");
            var anchura = sistema.RecorridoAnchura();
            foreach (var linea in anchura)
            {
                Console.WriteLine(linea);
            }

            // Mostrar estadísticas
            Console.WriteLine("\n=== ESTADÍSTICAS ===");
            var stats = sistema.ObtenerEstadisticas();
            foreach (var stat in stats)
            {
                Console.WriteLine($"{stat.Key}: {stat.Value}");
            }

            Console.WriteLine("\nPresione cualquier tecla para continuar...");
            Console.ReadKey();
        }

        /// <summary>
        /// Realiza pruebas de búsqueda y muestra los resultados
        /// </summary>
        /// <param name="sistema">Sistema de archivos a probar</param>
        public static void PruebasBusqueda(ArbolSistemaArchivos sistema)
        {
            Console.WriteLine("\n=== PRUEBAS DE BÚSQUEDA ===");

            // Pruebas de búsqueda por nombre
            string[] nombresPrueba = { "cv.docx", "vacaciones", "archivo_inexistente.txt" };

            foreach (var nombre in nombresPrueba)
            {
                var nodo = sistema.BuscarNodo(nombre);
                if (nodo != null)
                {
                    Console.WriteLine($"✓ Encontrado '{nombre}': {sistema.ObtenerRutaAbsoluta(nodo)}");
                }
                else
                {
                    Console.WriteLine($"✗ No encontrado '{nombre}'");
                }
            }

            // Pruebas de búsqueda por ruta
            string[] rutasPrueba = { "/root/fotos/vacaciones", "/root/documentos/cv.docx", "/root/inexistente" };

            Console.WriteLine("\n--- Búsqueda por ruta ---");
            foreach (var ruta in rutasPrueba)
            {
                var nodo = sistema.BuscarNodoPorRuta(ruta);
                if (nodo != null)
                {
                    Console.WriteLine($"✓ Encontrado '{ruta}': {nodo}");
                }
                else
                {
                    Console.WriteLine($"✗ No encontrado '{ruta}'");
                }
            }
        }
    }
}