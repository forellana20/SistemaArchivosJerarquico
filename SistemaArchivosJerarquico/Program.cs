using System;
using System.Windows.Forms;
using SistemaArchivosJerarquico.Forms;

namespace SistemaArchivosJerarquico
{
    /// <summary>
    /// Clase principal de la aplicación
    /// Punto de entrada del programa
    /// </summary>
    internal static class Program
    {
        /// <summary>
        /// Punto de entrada principal para la aplicación.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new FormPrincipal());
        }
    }
}