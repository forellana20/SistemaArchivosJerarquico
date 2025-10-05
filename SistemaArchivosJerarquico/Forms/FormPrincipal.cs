using System;
using System.Linq;
using System.Windows.Forms;
using SistemaArchivosJerarquico.Models;
using SistemaArchivosJerarquico.Services;
using SistemaArchivosJerarquico.Utilities;

namespace SistemaArchivosJerarquico.Forms
{
    /// <summary>
    /// Formulario principal de la aplicación
    /// Proporciona interfaz gráfica para interactuar con el sistema de archivos jerárquico
    /// </summary>
    public partial class FormPrincipal : Form
    {
        #region Propiedades Privadas

        /// <summary>
        /// Instancia del sistema de archivos jerárquico
        /// </summary>
        private ArbolSistemaArchivos sistemaArchivos;

        #endregion

        #region Constructor

        /// <summary>
        /// Constructor del formulario principal
        /// Inicializa los componentes y configura el estado inicial
        /// </summary>
        public FormPrincipal()
        {
            InitializeComponent();
            InicializarFormulario();
        }

        #endregion

        #region Métodos de Inicialización

        /// <summary>
        /// Configura el estado inicial del formulario
        /// </summary>
        private void InicializarFormulario()
        {
            // Configurar ComboBox de tipo de nodo
            cmbTipoNodo.SelectedIndex = 0; // Seleccionar "Archivo" por defecto

            // Mostrar mensaje inicial
            txtEstructura.Text = "Haga clic en 'Cargar Estructura de Ejemplo' para comenzar...";
            txtRecorridos.Text = "Primero cargue una estructura de ejemplo...";
            txtResultadoBusqueda.Text = "Primero cargue una estructura de ejemplo...";
            txtResultadoAgregar.Text = "Primero cargue una estructura de ejemplo...";
            txtEstadisticas.Text = "Primero cargue una estructura de ejemplo...";
        }

        #endregion

        #region Eventos de Botones - Pestaña Estructura

        /// <summary>
        /// Maneja el evento de clic del botón "Cargar Estructura de Ejemplo"
        /// </summary>
        private void btnCargarEjemplo_Click(object sender, EventArgs e)
        {
            try
            {
                // Crear sistema con estructura de ejemplo
                sistemaArchivos = DemostradorFuncionalidades.CrearEstructuraEjemplo();

                // Mostrar estructura en el TextBox
                var estructura = sistemaArchivos.MostrarEstructura();
                txtEstructura.Text = string.Join(Environment.NewLine, estructura);

                // Limpiar otros TextBox
                txtRecorridos.Text = "Seleccione un tipo de recorrido...";
                txtResultadoBusqueda.Text = "Ingrese un término de búsqueda...";
                txtResultadoAgregar.Text = "Configure los datos y agregue un elemento...";
                txtEstadisticas.Text = "Haga clic en 'Actualizar Estadísticas'...";

                // Mostrar mensaje de éxito
                MessageBox.Show("Estructura de ejemplo cargada exitosamente.",
                              "Éxito",
                              MessageBoxButtons.OK,
                              MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al cargar la estructura: {ex.Message}",
                              "Error",
                              MessageBoxButtons.OK,
                              MessageBoxIcon.Error);
            }
        }

        #endregion

        #region Eventos de Botones - Pestaña Recorridos

        /// <summary>
        /// Maneja el evento de clic del botón "Recorrido Preorden"
        /// </summary>
        private void btnRecorridoPreorden_Click(object sender, EventArgs e)
        {
            if (!ValidarSistemaInicializado()) return;

            try
            {
                var recorrido = sistemaArchivos.RecorridoPreorden();
                txtRecorridos.Text = "=== RECORRIDO PREORDEN ===" + Environment.NewLine +
                                   string.Join(Environment.NewLine, recorrido);
            }
            catch (Exception ex)
            {
                MostrarError("Error en recorrido preorden", ex);
            }
        }

        /// <summary>
        /// Maneja el evento de clic del botón "Recorrido Postorden"
        /// </summary>
        private void btnRecorridoPostorden_Click(object sender, EventArgs e)
        {
            if (!ValidarSistemaInicializado()) return;

            try
            {
                var recorrido = sistemaArchivos.RecorridoPostorden();
                txtRecorridos.Text = "=== RECORRIDO POSTORDEN ===" + Environment.NewLine +
                                   string.Join(Environment.NewLine, recorrido);
            }
            catch (Exception ex)
            {
                MostrarError("Error en recorrido postorden", ex);
            }
        }

        /// <summary>
        /// Maneja el evento de clic del botón "Recorrido en Anchura"
        /// </summary>
        private void btnRecorridoAnchura_Click(object sender, EventArgs e)
        {
            if (!ValidarSistemaInicializado()) return;

            try
            {
                var recorrido = sistemaArchivos.RecorridoAnchura();
                txtRecorridos.Text = "=== RECORRIDO EN ANCHURA ===" + Environment.NewLine +
                                   string.Join(Environment.NewLine, recorrido);
            }
            catch (Exception ex)
            {
                MostrarError("Error en recorrido en anchura", ex);
            }
        }

        #endregion

        #region Eventos de Botones - Pestaña Búsqueda

        /// <summary>
        /// Maneja el evento de clic del botón "Buscar por Nombre"
        /// </summary>
        private void btnBuscarNombre_Click(object sender, EventArgs e)
        {
            if (!ValidarSistemaInicializado()) return;

            try
            {
                string nombre = txtNombreBusqueda.Text.Trim();
                if (string.IsNullOrEmpty(nombre))
                {
                    MessageBox.Show("Por favor ingrese un nombre para buscar.",
                                  "Advertencia",
                                  MessageBoxButtons.OK,
                                  MessageBoxIcon.Warning);
                    return;
                }

                var nodo = sistemaArchivos.BuscarNodo(nombre);
                if (nodo != null)
                {
                    string ruta = sistemaArchivos.ObtenerRutaAbsoluta(nodo);
                    txtResultadoBusqueda.Text = $"=== BÚSQUEDA POR NOMBRE: '{nombre}' ===" + Environment.NewLine +
                                              $"✓ ENCONTRADO" + Environment.NewLine +
                                              $"Nombre: {nodo.Nombre}" + Environment.NewLine +
                                              $"Tipo: {nodo.Tipo}" + Environment.NewLine +
                                              $"Ruta: {ruta}" + Environment.NewLine +
                                              $"Es Carpeta: {(nodo.Tipo == TipoNodo.Carpeta ? "Sí" : "No")}" + Environment.NewLine;

                    if (nodo.Tipo == TipoNodo.Carpeta)
                    {
                        txtResultadoBusqueda.Text += $"Elementos hijos: {nodo.Hijos.Count}" + Environment.NewLine;
                    }
                }
                else
                {
                    txtResultadoBusqueda.Text = $"=== BÚSQUEDA POR NOMBRE: '{nombre}' ===" + Environment.NewLine +
                                              $"✗ NO ENCONTRADO" + Environment.NewLine +
                                              $"El elemento '{nombre}' no existe en el sistema.";
                }
            }
            catch (Exception ex)
            {
                MostrarError("Error en búsqueda por nombre", ex);
            }
        }

        /// <summary>
        /// Maneja el evento de clic del botón "Buscar por Ruta"
        /// </summary>
        private void btnBuscarRuta_Click(object sender, EventArgs e)
        {
            if (!ValidarSistemaInicializado()) return;

            try
            {
                string ruta = txtRutaBusqueda.Text.Trim();
                if (string.IsNullOrEmpty(ruta))
                {
                    MessageBox.Show("Por favor ingrese una ruta para buscar.",
                                  "Advertencia",
                                  MessageBoxButtons.OK,
                                  MessageBoxIcon.Warning);
                    return;
                }

                var nodo = sistemaArchivos.BuscarNodoPorRuta(ruta);
                if (nodo != null)
                {
                    txtResultadoBusqueda.Text = $"=== BÚSQUEDA POR RUTA: '{ruta}' ===" + Environment.NewLine +
                                              $"✓ ENCONTRADO" + Environment.NewLine +
                                              $"Nombre: {nodo.Nombre}" + Environment.NewLine +
                                              $"Tipo: {nodo.Tipo}" + Environment.NewLine +
                                              $"Ruta: {ruta}" + Environment.NewLine +
                                              $"Es Carpeta: {(nodo.Tipo == TipoNodo.Carpeta ? "Sí" : "No")}" + Environment.NewLine;

                    if (nodo.Tipo == TipoNodo.Carpeta)
                    {
                        txtResultadoBusqueda.Text += $"Elementos hijos: {nodo.Hijos.Count}" + Environment.NewLine;
                        if (nodo.Hijos.Count > 0)
                        {
                            txtResultadoBusqueda.Text += "Contenido:" + Environment.NewLine;
                            foreach (var hijo in nodo.Hijos)
                            {
                                txtResultadoBusqueda.Text += $"  - {hijo.Nombre} ({hijo.Tipo})" + Environment.NewLine;
                            }
                        }
                    }
                }
                else
                {
                    txtResultadoBusqueda.Text = $"=== BÚSQUEDA POR RUTA: '{ruta}' ===" + Environment.NewLine +
                                              $"✗ NO ENCONTRADO" + Environment.NewLine +
                                              $"La ruta '{ruta}' no existe en el sistema.";
                }
            }
            catch (Exception ex)
            {
                MostrarError("Error en búsqueda por ruta", ex);
            }
        }

        #endregion

        #region Eventos de Botones - Pestaña Agregar

        /// <summary>
        /// Maneja el evento de clic del botón "Agregar Elemento"
        /// </summary>
        private void btnAgregar_Click(object sender, EventArgs e)
        {
            if (!ValidarSistemaInicializado()) return;

            try
            {
                string rutaPadre = txtRutaPadr.Text.Trim();
                string nombreNuevo = txtNombreNuevo.Text.Trim();

                if (string.IsNullOrEmpty(rutaPadre) || string.IsNullOrEmpty(nombreNuevo))
                {
                    MessageBox.Show("Por favor complete todos los campos.",
                                  "Advertencia",
                                  MessageBoxButtons.OK,
                                  MessageBoxIcon.Warning);
                    return;
                }

                if (cmbTipoNodo.SelectedIndex == -1)
                {
                    MessageBox.Show("Por favor seleccione un tipo de nodo.",
                                  "Advertencia",
                                  MessageBoxButtons.OK,
                                  MessageBoxIcon.Warning);
                    return;
                }

                TipoNodo tipo = cmbTipoNodo.SelectedItem.ToString() == "Archivo" ? TipoNodo.Archivo : TipoNodo.Carpeta;

                bool exito = sistemaArchivos.AgregarNodo(rutaPadre, nombreNuevo, tipo);

                if (exito)
                {
                    string rutaCompleta = rutaPadre.TrimEnd('/') + "/" + nombreNuevo;
                    txtResultadoAgregar.Text = $"=== ELEMENTO AGREGADO EXITOSAMENTE ===" + Environment.NewLine +
                                             $"Nombre: {nombreNuevo}" + Environment.NewLine +
                                             $"Tipo: {tipo}" + Environment.NewLine +
                                             $"Ruta padre: {rutaPadre}" + Environment.NewLine +
                                             $"Ruta completa: {rutaCompleta}" + Environment.NewLine + Environment.NewLine +
                                             "=== ESTRUCTURA ACTUALIZADA ===" + Environment.NewLine +
                                             string.Join(Environment.NewLine, sistemaArchivos.MostrarEstructura());

                    // Actualizar la pestaña de estructura también
                    txtEstructura.Text = string.Join(Environment.NewLine, sistemaArchivos.MostrarEstructura());

                    MessageBox.Show($"Elemento '{nombreNuevo}' agregado exitosamente.",
                                  "Éxito",
                                  MessageBoxButtons.OK,
                                  MessageBoxIcon.Information);
                }
                else
                {
                    txtResultadoAgregar.Text = $"=== ERROR AL AGREGAR ELEMENTO ===" + Environment.NewLine +
                                             $"No se pudo agregar '{nombreNuevo}' en '{rutaPadre}'" + Environment.NewLine +
                                             "Posibles causas:" + Environment.NewLine +
                                             "- La ruta padre no existe" + Environment.NewLine +
                                             "- La ruta padre no es una carpeta" + Environment.NewLine +
                                             "- Ya existe un elemento con ese nombre";
                }
            }
            catch (Exception ex)
            {
                MostrarError("Error al agregar elemento", ex);
            }
        }

        #endregion

        #region Eventos de Botones - Pestaña Estadísticas

        /// <summary>
        /// Maneja el evento de clic del botón "Actualizar Estadísticas"
        /// </summary>
        private void btnActualizarEstadisticas_Click(object sender, EventArgs e)
        {
            if (!ValidarSistemaInicializado()) return;

            try
            {
                var estadisticas = sistemaArchivos.ObtenerEstadisticas();

                txtEstadisticas.Text = "=== ESTADÍSTICAS DEL SISTEMA DE ARCHIVOS ===" + Environment.NewLine + Environment.NewLine;

                foreach (var stat in estadisticas)
                {
                    txtEstadisticas.Text += $"{stat.Key}: {stat.Value}" + Environment.NewLine;
                }

                txtEstadisticas.Text += Environment.NewLine + "=== ANÁLISIS DE COMPLEJIDAD ===" + Environment.NewLine;
                txtEstadisticas.Text += "• Inserción: O(n) - donde n es el número de nodos" + Environment.NewLine;
                txtEstadisticas.Text += "• Búsqueda: O(n) - recorrido completo en peor caso" + Environment.NewLine;
                txtEstadisticas.Text += "• Recorridos: O(n) - visita cada nodo una vez" + Environment.NewLine;
                txtEstadisticas.Text += "• Espacio: O(n) - almacena n nodos" + Environment.NewLine;
            }
            catch (Exception ex)
            {
                MostrarError("Error al obtener estadísticas", ex);
            }
        }

        #endregion

        #region Métodos de Validación y Utilidad

        /// <summary>
        /// Valida que el sistema de archivos esté inicializado
        /// </summary>
        /// <returns>True si está inicializado, False en caso contrario</returns>
        private bool ValidarSistemaInicializado()
        {
            if (sistemaArchivos == null)
            {
                MessageBox.Show("Primero debe cargar una estructura de ejemplo.",
                              "Advertencia",
                              MessageBoxButtons.OK,
                              MessageBoxIcon.Warning);
                return false;
            }
            return true;
        }

        /// <summary>
        /// Muestra un mensaje de error al usuario
        /// </summary>
        /// <param name="titulo">Título del error</param>
        /// <param name="ex">Excepción ocurrida</param>
        private void MostrarError(string titulo, Exception ex)
        {
            MessageBox.Show($"{titulo}: {ex.Message}",
                          "Error",
                          MessageBoxButtons.OK,
                          MessageBoxIcon.Error);
        }

        #endregion

        private Label lblTitulo;

        private void InitializeComponent()
        {
            this.lblTitulo = new System.Windows.Forms.Label();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.txtEstructura = new System.Windows.Forms.TextBox();
            this.lblEstructura = new System.Windows.Forms.Label();
            this.btnCargarEjemplo = new System.Windows.Forms.Button();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.lblRecorridos = new System.Windows.Forms.Label();
            this.txtRecorridos = new System.Windows.Forms.TextBox();
            this.btnRecorridoAnchura = new System.Windows.Forms.Button();
            this.btnRecorridoPostorden = new System.Windows.Forms.Button();
            this.btnRecorridoPreorden = new System.Windows.Forms.Button();
            this.tabPage3 = new System.Windows.Forms.TabPage();
            this.lblResultadoBusqueda = new System.Windows.Forms.Label();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.txtRutaBusqueda = new System.Windows.Forms.TextBox();
            this.btnBuscarRuta = new System.Windows.Forms.Button();
            this.btnBuscarNombre = new System.Windows.Forms.Button();
            this.txtNombreBusqueda = new System.Windows.Forms.TextBox();
            this.lblRutaBusqueda = new System.Windows.Forms.Label();
            this.lblNombreBusqueda = new System.Windows.Forms.Label();
            this.tabPage4 = new System.Windows.Forms.TabPage();
            this.tabPage5 = new System.Windows.Forms.TabPage();
            this.lblRutaPadre = new System.Windows.Forms.Label();
            this.lblTipoNodo = new System.Windows.Forms.Label();
            this.txtRutaPadr = new System.Windows.Forms.Label();
            this.txtRutaPadre = new System.Windows.Forms.TextBox();
            this.cmbTipoNodo = new System.Windows.Forms.ComboBox();
            this.lblNombreNuevo = new System.Windows.Forms.Label();
            this.txtNombreNuevo = new System.Windows.Forms.TextBox();
            this.btnAgregar = new System.Windows.Forms.Button();
            this.lblResultadoAgregar = new System.Windows.Forms.Label();
            this.textBox2 = new System.Windows.Forms.TextBox();
            this.btnActualizarEstadisticas = new System.Windows.Forms.Button();
            this.lblEstadisticas = new System.Windows.Forms.Label();
            this.txtEstadisticas = new System.Windows.Forms.TextBox();
            this.tabControl1.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.tabPage2.SuspendLayout();
            this.tabPage3.SuspendLayout();
            this.tabPage4.SuspendLayout();
            this.tabPage5.SuspendLayout();
            this.SuspendLayout();
            // 
            // lblTitulo
            // 
            this.lblTitulo.AutoSize = true;
            this.lblTitulo.Location = new System.Drawing.Point(302, 9);
            this.lblTitulo.Name = "lblTitulo";
            this.lblTitulo.Size = new System.Drawing.Size(213, 13);
            this.lblTitulo.TabIndex = 0;
            this.lblTitulo.Text = "Sistema de Archivos Jerárquico - TAD Árbol";
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tabPage1);
            this.tabControl1.Controls.Add(this.tabPage2);
            this.tabControl1.Controls.Add(this.tabPage3);
            this.tabControl1.Controls.Add(this.tabPage4);
            this.tabControl1.Controls.Add(this.tabPage5);
            this.tabControl1.Location = new System.Drawing.Point(12, 40);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(853, 474);
            this.tabControl1.TabIndex = 1;
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.txtEstructura);
            this.tabPage1.Controls.Add(this.lblEstructura);
            this.tabPage1.Controls.Add(this.btnCargarEjemplo);
            this.tabPage1.Location = new System.Drawing.Point(4, 22);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(845, 448);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "Estructura";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // txtEstructura
            // 
            this.txtEstructura.Location = new System.Drawing.Point(18, 104);
            this.txtEstructura.Multiline = true;
            this.txtEstructura.Name = "txtEstructura";
            this.txtEstructura.ReadOnly = true;
            this.txtEstructura.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.txtEstructura.Size = new System.Drawing.Size(809, 324);
            this.txtEstructura.TabIndex = 2;
            // 
            // lblEstructura
            // 
            this.lblEstructura.AutoSize = true;
            this.lblEstructura.Location = new System.Drawing.Point(36, 66);
            this.lblEstructura.Name = "lblEstructura";
            this.lblEstructura.Size = new System.Drawing.Size(115, 13);
            this.lblEstructura.TabIndex = 1;
            this.lblEstructura.Text = "Estructura del Sistema:";
            this.lblEstructura.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // btnCargarEjemplo
            // 
            this.btnCargarEjemplo.Location = new System.Drawing.Point(18, 19);
            this.btnCargarEjemplo.Name = "btnCargarEjemplo";
            this.btnCargarEjemplo.Size = new System.Drawing.Size(151, 23);
            this.btnCargarEjemplo.TabIndex = 0;
            this.btnCargarEjemplo.Text = "Cargar Estructura";
            this.btnCargarEjemplo.UseVisualStyleBackColor = true;
            // 
            // tabPage2
            // 
            this.tabPage2.Controls.Add(this.lblRecorridos);
            this.tabPage2.Controls.Add(this.txtRecorridos);
            this.tabPage2.Controls.Add(this.btnRecorridoAnchura);
            this.tabPage2.Controls.Add(this.btnRecorridoPostorden);
            this.tabPage2.Controls.Add(this.btnRecorridoPreorden);
            this.tabPage2.Location = new System.Drawing.Point(4, 22);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage2.Size = new System.Drawing.Size(845, 448);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "Recorridos";
            this.tabPage2.UseVisualStyleBackColor = true;
            // 
            // lblRecorridos
            // 
            this.lblRecorridos.AutoSize = true;
            this.lblRecorridos.Location = new System.Drawing.Point(26, 91);
            this.lblRecorridos.Name = "lblRecorridos";
            this.lblRecorridos.Size = new System.Drawing.Size(124, 13);
            this.lblRecorridos.TabIndex = 4;
            this.lblRecorridos.Text = "Resultado del Recorrido:";
            // 
            // txtRecorridos
            // 
            this.txtRecorridos.Location = new System.Drawing.Point(26, 128);
            this.txtRecorridos.Multiline = true;
            this.txtRecorridos.Name = "txtRecorridos";
            this.txtRecorridos.ReadOnly = true;
            this.txtRecorridos.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.txtRecorridos.Size = new System.Drawing.Size(794, 301);
            this.txtRecorridos.TabIndex = 3;
            // 
            // btnRecorridoAnchura
            // 
            this.btnRecorridoAnchura.Location = new System.Drawing.Point(355, 29);
            this.btnRecorridoAnchura.Name = "btnRecorridoAnchura";
            this.btnRecorridoAnchura.Size = new System.Drawing.Size(131, 23);
            this.btnRecorridoAnchura.TabIndex = 2;
            this.btnRecorridoAnchura.Text = "Recorrido en Anchura";
            this.btnRecorridoAnchura.UseVisualStyleBackColor = true;
            // 
            // btnRecorridoPostorden
            // 
            this.btnRecorridoPostorden.Location = new System.Drawing.Point(189, 29);
            this.btnRecorridoPostorden.Name = "btnRecorridoPostorden";
            this.btnRecorridoPostorden.Size = new System.Drawing.Size(131, 23);
            this.btnRecorridoPostorden.TabIndex = 1;
            this.btnRecorridoPostorden.Text = "Recorrido Postorden";
            this.btnRecorridoPostorden.UseVisualStyleBackColor = true;
            // 
            // btnRecorridoPreorden
            // 
            this.btnRecorridoPreorden.Location = new System.Drawing.Point(26, 29);
            this.btnRecorridoPreorden.Name = "btnRecorridoPreorden";
            this.btnRecorridoPreorden.Size = new System.Drawing.Size(131, 23);
            this.btnRecorridoPreorden.TabIndex = 0;
            this.btnRecorridoPreorden.Text = "Recorrido Preorden";
            this.btnRecorridoPreorden.UseVisualStyleBackColor = true;
            // 
            // tabPage3
            // 
            this.tabPage3.Controls.Add(this.lblResultadoBusqueda);
            this.tabPage3.Controls.Add(this.textBox1);
            this.tabPage3.Controls.Add(this.txtRutaBusqueda);
            this.tabPage3.Controls.Add(this.btnBuscarRuta);
            this.tabPage3.Controls.Add(this.btnBuscarNombre);
            this.tabPage3.Controls.Add(this.txtNombreBusqueda);
            this.tabPage3.Controls.Add(this.lblRutaBusqueda);
            this.tabPage3.Controls.Add(this.lblNombreBusqueda);
            this.tabPage3.Location = new System.Drawing.Point(4, 22);
            this.tabPage3.Name = "tabPage3";
            this.tabPage3.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage3.Size = new System.Drawing.Size(845, 448);
            this.tabPage3.TabIndex = 2;
            this.tabPage3.Text = "Búsqueda";
            this.tabPage3.UseVisualStyleBackColor = true;
            // 
            // lblResultadoBusqueda
            // 
            this.lblResultadoBusqueda.AutoSize = true;
            this.lblResultadoBusqueda.Location = new System.Drawing.Point(22, 134);
            this.lblResultadoBusqueda.Name = "lblResultadoBusqueda";
            this.lblResultadoBusqueda.Size = new System.Drawing.Size(124, 13);
            this.lblResultadoBusqueda.TabIndex = 7;
            this.lblResultadoBusqueda.Text = "Resultado de Búsqueda:";
            // 
            // textBox1
            // 
            this.textBox1.Location = new System.Drawing.Point(25, 162);
            this.textBox1.Multiline = true;
            this.textBox1.Name = "textBox1";
            this.textBox1.ReadOnly = true;
            this.textBox1.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.textBox1.Size = new System.Drawing.Size(794, 263);
            this.textBox1.TabIndex = 6;
            // 
            // txtRutaBusqueda
            // 
            this.txtRutaBusqueda.Location = new System.Drawing.Point(312, 63);
            this.txtRutaBusqueda.Name = "txtRutaBusqueda";
            this.txtRutaBusqueda.Size = new System.Drawing.Size(145, 20);
            this.txtRutaBusqueda.TabIndex = 5;
            this.txtRutaBusqueda.Text = "/root/documentos/cv.docx";
            // 
            // btnBuscarRuta
            // 
            this.btnBuscarRuta.Location = new System.Drawing.Point(478, 63);
            this.btnBuscarRuta.Name = "btnBuscarRuta";
            this.btnBuscarRuta.Size = new System.Drawing.Size(75, 23);
            this.btnBuscarRuta.TabIndex = 4;
            this.btnBuscarRuta.Text = "Buscar";
            this.btnBuscarRuta.UseVisualStyleBackColor = true;
            // 
            // btnBuscarNombre
            // 
            this.btnBuscarNombre.Location = new System.Drawing.Point(193, 63);
            this.btnBuscarNombre.Name = "btnBuscarNombre";
            this.btnBuscarNombre.Size = new System.Drawing.Size(75, 23);
            this.btnBuscarNombre.TabIndex = 3;
            this.btnBuscarNombre.Text = "Buscar";
            this.btnBuscarNombre.UseVisualStyleBackColor = true;
            // 
            // txtNombreBusqueda
            // 
            this.txtNombreBusqueda.Location = new System.Drawing.Point(25, 66);
            this.txtNombreBusqueda.Name = "txtNombreBusqueda";
            this.txtNombreBusqueda.Size = new System.Drawing.Size(145, 20);
            this.txtNombreBusqueda.TabIndex = 2;
            this.txtNombreBusqueda.Text = "cv.docx";
            // 
            // lblRutaBusqueda
            // 
            this.lblRutaBusqueda.AutoSize = true;
            this.lblRutaBusqueda.Location = new System.Drawing.Point(309, 20);
            this.lblRutaBusqueda.Name = "lblRutaBusqueda";
            this.lblRutaBusqueda.Size = new System.Drawing.Size(87, 13);
            this.lblRutaBusqueda.TabIndex = 1;
            this.lblRutaBusqueda.Text = "Buscar por Ruta:";
            // 
            // lblNombreBusqueda
            // 
            this.lblNombreBusqueda.AutoSize = true;
            this.lblNombreBusqueda.Location = new System.Drawing.Point(22, 20);
            this.lblNombreBusqueda.Name = "lblNombreBusqueda";
            this.lblNombreBusqueda.Size = new System.Drawing.Size(101, 13);
            this.lblNombreBusqueda.TabIndex = 0;
            this.lblNombreBusqueda.Text = "Buscar por Nombre:";
            // 
            // tabPage4
            // 
            this.tabPage4.Controls.Add(this.textBox2);
            this.tabPage4.Controls.Add(this.lblResultadoAgregar);
            this.tabPage4.Controls.Add(this.btnAgregar);
            this.tabPage4.Controls.Add(this.txtNombreNuevo);
            this.tabPage4.Controls.Add(this.lblNombreNuevo);
            this.tabPage4.Controls.Add(this.cmbTipoNodo);
            this.tabPage4.Controls.Add(this.txtRutaPadre);
            this.tabPage4.Controls.Add(this.txtRutaPadr);
            this.tabPage4.Controls.Add(this.lblTipoNodo);
            this.tabPage4.Controls.Add(this.lblRutaPadre);
            this.tabPage4.Location = new System.Drawing.Point(4, 22);
            this.tabPage4.Name = "tabPage4";
            this.tabPage4.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage4.Size = new System.Drawing.Size(845, 448);
            this.tabPage4.TabIndex = 3;
            this.tabPage4.Text = "Agregar";
            this.tabPage4.UseVisualStyleBackColor = true;
            // 
            // tabPage5
            // 
            this.tabPage5.Controls.Add(this.txtEstadisticas);
            this.tabPage5.Controls.Add(this.lblEstadisticas);
            this.tabPage5.Controls.Add(this.btnActualizarEstadisticas);
            this.tabPage5.Location = new System.Drawing.Point(4, 22);
            this.tabPage5.Name = "tabPage5";
            this.tabPage5.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage5.Size = new System.Drawing.Size(845, 448);
            this.tabPage5.TabIndex = 4;
            this.tabPage5.Text = "Estadísticas";
            this.tabPage5.UseVisualStyleBackColor = true;
            // 
            // lblRutaPadre
            // 
            this.lblRutaPadre.AutoSize = true;
            this.lblRutaPadre.Location = new System.Drawing.Point(30, 21);
            this.lblRutaPadre.Name = "lblRutaPadre";
            this.lblRutaPadre.Size = new System.Drawing.Size(99, 13);
            this.lblRutaPadre.TabIndex = 0;
            this.lblRutaPadre.Text = "Ruta de la Carpeta:";
            // 
            // lblTipoNodo
            // 
            this.lblTipoNodo.AutoSize = true;
            this.lblTipoNodo.Location = new System.Drawing.Point(276, 21);
            this.lblTipoNodo.Name = "lblTipoNodo";
            this.lblTipoNodo.Size = new System.Drawing.Size(31, 13);
            this.lblTipoNodo.TabIndex = 1;
            this.lblTipoNodo.Text = "Tipo:";
            // 
            // txtRutaPadr
            // 
            this.txtRutaPadr.AutoSize = true;
            this.txtRutaPadr.Location = new System.Drawing.Point(33, 163);
            this.txtRutaPadr.Name = "txtRutaPadr";
            this.txtRutaPadr.Size = new System.Drawing.Size(0, 13);
            this.txtRutaPadr.TabIndex = 2;
            // 
            // txtRutaPadre
            // 
            this.txtRutaPadre.Location = new System.Drawing.Point(33, 53);
            this.txtRutaPadre.Name = "txtRutaPadre";
            this.txtRutaPadre.Size = new System.Drawing.Size(197, 20);
            this.txtRutaPadre.TabIndex = 3;
            this.txtRutaPadre.Text = "/root/documentos";
            // 
            // cmbTipoNodo
            // 
            this.cmbTipoNodo.FormattingEnabled = true;
            this.cmbTipoNodo.Items.AddRange(new object[] {
            "Archivo",
            "Carpeta"});
            this.cmbTipoNodo.Location = new System.Drawing.Point(279, 51);
            this.cmbTipoNodo.Name = "cmbTipoNodo";
            this.cmbTipoNodo.Size = new System.Drawing.Size(147, 21);
            this.cmbTipoNodo.TabIndex = 4;
            // 
            // lblNombreNuevo
            // 
            this.lblNombreNuevo.AutoSize = true;
            this.lblNombreNuevo.Location = new System.Drawing.Point(30, 103);
            this.lblNombreNuevo.Name = "lblNombreNuevo";
            this.lblNombreNuevo.Size = new System.Drawing.Size(111, 13);
            this.lblNombreNuevo.TabIndex = 5;
            this.lblNombreNuevo.Text = "Nombre del Elemento:";
            // 
            // txtNombreNuevo
            // 
            this.txtNombreNuevo.Location = new System.Drawing.Point(30, 131);
            this.txtNombreNuevo.Name = "txtNombreNuevo";
            this.txtNombreNuevo.Size = new System.Drawing.Size(197, 20);
            this.txtNombreNuevo.TabIndex = 6;
            // 
            // btnAgregar
            // 
            this.btnAgregar.Location = new System.Drawing.Point(33, 179);
            this.btnAgregar.Name = "btnAgregar";
            this.btnAgregar.Size = new System.Drawing.Size(194, 23);
            this.btnAgregar.TabIndex = 7;
            this.btnAgregar.Text = "Agregar Elemento";
            this.btnAgregar.UseVisualStyleBackColor = true;
            // 
            // lblResultadoAgregar
            // 
            this.lblResultadoAgregar.AutoSize = true;
            this.lblResultadoAgregar.Location = new System.Drawing.Point(33, 227);
            this.lblResultadoAgregar.Name = "lblResultadoAgregar";
            this.lblResultadoAgregar.Size = new System.Drawing.Size(58, 13);
            this.lblResultadoAgregar.TabIndex = 8;
            this.lblResultadoAgregar.Text = "Resultado:";
            // 
            // textBox2
            // 
            this.textBox2.Location = new System.Drawing.Point(33, 252);
            this.textBox2.Multiline = true;
            this.textBox2.Name = "textBox2";
            this.textBox2.ReadOnly = true;
            this.textBox2.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.textBox2.Size = new System.Drawing.Size(785, 181);
            this.textBox2.TabIndex = 9;
            // 
            // btnActualizarEstadisticas
            // 
            this.btnActualizarEstadisticas.Location = new System.Drawing.Point(19, 22);
            this.btnActualizarEstadisticas.Name = "btnActualizarEstadisticas";
            this.btnActualizarEstadisticas.Size = new System.Drawing.Size(219, 23);
            this.btnActualizarEstadisticas.TabIndex = 0;
            this.btnActualizarEstadisticas.Text = "Actualizar Estadísticas";
            this.btnActualizarEstadisticas.UseVisualStyleBackColor = true;
            // 
            // lblEstadisticas
            // 
            this.lblEstadisticas.AutoSize = true;
            this.lblEstadisticas.Location = new System.Drawing.Point(70, 68);
            this.lblEstadisticas.Name = "lblEstadisticas";
            this.lblEstadisticas.Size = new System.Drawing.Size(112, 13);
            this.lblEstadisticas.TabIndex = 1;
            this.lblEstadisticas.Text = "Estadísticas del Árbol:";
            // 
            // txtEstadisticas
            // 
            this.txtEstadisticas.Location = new System.Drawing.Point(19, 108);
            this.txtEstadisticas.Multiline = true;
            this.txtEstadisticas.Name = "txtEstadisticas";
            this.txtEstadisticas.ReadOnly = true;
            this.txtEstadisticas.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.txtEstadisticas.Size = new System.Drawing.Size(798, 316);
            this.txtEstadisticas.TabIndex = 2;
            // 
            // FormPrincipal
            // 
            this.ClientSize = new System.Drawing.Size(877, 526);
            this.Controls.Add(this.tabControl1);
            this.Controls.Add(this.lblTitulo);
            this.Name = "FormPrincipal";
            this.tabControl1.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.tabPage1.PerformLayout();
            this.tabPage2.ResumeLayout(false);
            this.tabPage2.PerformLayout();
            this.tabPage3.ResumeLayout(false);
            this.tabPage3.PerformLayout();
            this.tabPage4.ResumeLayout(false);
            this.tabPage4.PerformLayout();
            this.tabPage5.ResumeLayout(false);
            this.tabPage5.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        private TabControl tabControl1;
        private TabPage tabPage1;
        private TabPage tabPage2;
        private TabPage tabPage3;
        private TabPage tabPage4;
        private TabPage tabPage5;
        private Label lblEstructura;
        private Button btnCargarEjemplo;
        private TextBox txtEstructura;
        private Label lblRecorridos;
        private TextBox txtRecorridos;
        private Button btnRecorridoAnchura;
        private Button btnRecorridoPostorden;
        private Button btnRecorridoPreorden;
        private Label lblResultadoBusqueda;
        private TextBox textBox1;
        private TextBox txtRutaBusqueda;
        private Button btnBuscarRuta;
        private Button btnBuscarNombre;
        private TextBox txtNombreBusqueda;
        private Label lblRutaBusqueda;
        private Label lblNombreBusqueda;
        private TextBox txtRutaPadre;
        private Label txtRutaPadr;
        private Label lblTipoNodo;
        private Label lblRutaPadre;
        private TextBox textBox2;
        private Label lblResultadoAgregar;
        private Button btnAgregar;
        private TextBox txtNombreNuevo;
        private Label lblNombreNuevo;
        private ComboBox cmbTipoNodo;
        private TextBox txtEstadisticas;
        private Label lblEstadisticas;
        private Button btnActualizarEstadisticas;
    }
}