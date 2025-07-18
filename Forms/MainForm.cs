using ClosedXML.Excel;
using System;
using System.Windows.Forms;
using System.Linq;
using System.Collections.Generic;
using QRCoder; // ¡Asegúrate de agregar este using!
using System.Drawing; // Para trabajar con imágenes
using System.IO; // Para guardar el archivo de imagen

public class MainForm : Form
{
    private Button btnIngresar;
    private Button btnRetirar;
    private Button btnExportar;
    private TextBox txtPlaca;
    private ComboBox cmbTipo;
    private ListBox lstPlacas;
    private Button btnCerrar;
    private Label lblVehiculosActivos;
    private Label lblTotalRecaudado;
    private Label lblPromedioPermanencia;
    private Panel panelEstadisticas;
    private ListBox lstEspacios;
    private Panel panelGrillaEspacios;
    private Label[] lblEspaciosArray;

    private Estacionamiento sistema;
    private string[] espacios = new string[]
    {
        "A1", "A2", "A3", "A4", "A5",
        "B1", "B2", "B3", "B4", "B5",
        "C1", "C2", "C3", "C4", "C5"
    };
    private Dictionary<string, string> espaciosOcupados = new Dictionary<string, string>();

    // Control para mostrar el QR (opcional, si quieres verlo en la interfaz)
    private PictureBox pbQRDisplay; // Agregamos un PictureBox para mostrar el QR

    public MainForm()
    {
        this.Text = "FAST PARKING";
        this.Width = 750;
        this.Height = 520;

        sistema = new Estacionamiento(new TarifaPorTipoVehiculo(), new HistorialSalida());

        Label lblPlaca = new Label() { Text = "Placa:", Top = 20, Left = 40, Width = 50 };
        txtPlaca = new TextBox()
        {
            Top = 20,
            Left = 100,
            Width = 200,
            BackColor = System.Drawing.Color.White,
            ForeColor = System.Drawing.Color.Black
        };
        txtPlaca.MaxLength = 6;

        Label lblTipo = new Label() { Text = "Tipo:", Top = 60, Left = 40, Width = 50 };
        cmbTipo = new ComboBox()
        {
            Top = 60,
            Left = 100,
            Width = 200,
            DropDownStyle = ComboBoxStyle.DropDownList,
            BackColor = System.Drawing.Color.White,
            ForeColor = System.Drawing.Color.Black
        };
        cmbTipo.Items.AddRange(Enum.GetNames(typeof(TipoVehiculo)));
        cmbTipo.SelectedIndex = 0;

        btnIngresar = new Button() { Text = "Ingresar", Top = 100, Left = 20 };
        btnRetirar = new Button() { Text = "Retirar", Top = 100, Left = 120 };
        btnExportar = new Button() { Text = "Exportar Historial", Top = 475, Left = 400, Width = 180 };
        btnCerrar = new Button()
        {
            Text = "Cerrar",
            Top = 475,
            Left = 600,
            Width = 100,
            Height = 35
        };
        btnCerrar.Click += BtnCerrar_Click;
        Controls.Add(btnCerrar);


        btnIngresar.Click += BtnIngresar_Click;
        btnRetirar.Click += BtnRetirar_Click;
        btnExportar.Click += BtnExportar_Click;

        lstPlacas = new ListBox() { Top = 140, Left = 20, Width = 340, Height = 100 };

        // Lista de espacios de estacionamiento
        Label lblEspacios = new Label() { Text = "Espacios ocupados:", Top = 250, Left = 20, Width = 120 };
        lstEspacios = new ListBox() { Top = 275, Left = 20, Width = 340, Height = 80 };

        // Panel de grilla visual de espacios
        CrearGrillaEspacios();

        // Panel de estadísticas
        panelEstadisticas = new Panel()
        {
            Top = 20,
            Left = 400,
            Width = 300,
            Height = 200,
            BackColor = System.Drawing.Color.LightSteelBlue,
            BorderStyle = BorderStyle.FixedSingle
        };

        // Labels dentro del panel
        lblVehiculosActivos = new Label() { Top = 20, Left = 10, Width = 280, Text = "Vehículos activos: 0", Font = new System.Drawing.Font("Arial", 9, System.Drawing.FontStyle.Bold) };
        lblTotalRecaudado = new Label() { Top = 60, Left = 10, Width = 280, Text = "Total recaudado: S/ 0.00", Font = new System.Drawing.Font("Arial", 9, System.Drawing.FontStyle.Bold) };
        lblPromedioPermanencia = new Label() { Top = 100, Left = 10, Width = 280, Text = "Promedio permanencia: 0 min", Font = new System.Drawing.Font("Arial", 9, System.Drawing.FontStyle.Bold) };

        // Agregar label para espacios disponibles
        Label lblEspaciosDisponibles = new Label() { Top = 140, Left = 10, Width = 280, Text = $"Espacios disponibles: {espacios.Length}", Font = new System.Drawing.Font("Arial", 9, System.Drawing.FontStyle.Bold) };

        panelEstadisticas.Controls.Add(lblVehiculosActivos);
        panelEstadisticas.Controls.Add(lblTotalRecaudado);
        panelEstadisticas.Controls.Add(lblPromedioPermanencia);
        panelEstadisticas.Controls.Add(lblEspaciosDisponibles);

        // Panel informativo lateral derecho
        Panel panelInfo = new Panel()
        {
            Top = 240,
            Left = 400,
            Width = 300,
            Height = 220,
            BackColor = System.Drawing.Color.LightBlue,
            BorderStyle = BorderStyle.FixedSingle
        };

        // Título del panel
        Label lblTituloInfo = new Label()
        {
            Text = "INFORMACIÓN DEL SISTEMA",
            Top = 10,
            Left = 10,
            Width = 280,
            Height = 25,
            Font = new System.Drawing.Font("Arial", 10, System.Drawing.FontStyle.Bold),
            TextAlign = System.Drawing.ContentAlignment.MiddleCenter,
            BackColor = System.Drawing.Color.DarkBlue,
            ForeColor = System.Drawing.Color.White
        };

        // Información del sistema
        Label lblCapacidad = new Label()
        {
            Text = "Capacidad Total: 15 espacios",
            Top = 45,
            Left = 10,
            Width = 280,
            Height = 20,
            Font = new System.Drawing.Font("Arial", 8, System.Drawing.FontStyle.Regular)
        };

        Label lblHorario = new Label()
        {
            Text = "Horario: 24/7",
            Top = 70,
            Left = 10,
            Width = 280,
            Height = 20,
            Font = new System.Drawing.Font("Arial", 8, System.Drawing.FontStyle.Regular)
        };

        Label lblTarifas = new Label()
        {
            Text = "TARIFAS:",
            Top = 100,
            Left = 10,
            Width = 280,
            Height = 20,
            Font = new System.Drawing.Font("Arial", 9, System.Drawing.FontStyle.Bold)
        };

        Label lblTarifaAuto = new Label()
        {
            Text = "• Auto: S/ 2.00/hora",
            Top = 125,
            Left = 10,
            Width = 280,
            Height = 20,
            Font = new System.Drawing.Font("Arial", 8, System.Drawing.FontStyle.Regular)
        };

        Label lblTarifaMoto = new Label()
        {
            Text = "• Moto: S/ 1.00/hora",
            Top = 150,
            Left = 10,
            Width = 280,
            Height = 20,
            Font = new System.Drawing.Font("Arial", 8, System.Drawing.FontStyle.Regular)
        };

        Label lblTarifaCamion = new Label()
        {
            Text = "• Camión: S/ 3.00/hora",
            Top = 175,
            Left = 10,
            Width = 280,
            Height = 20,
            Font = new System.Drawing.Font("Arial", 8, System.Drawing.FontStyle.Regular)
        };

        Label lblInstrucciones = new Label()
        {
            Text = "INSTRUCCIONES:",
            Top = 180, // Ajusta esta posición si es necesario
            Left = 10,
            Width = 280,
            Height = 20,
            Font = new System.Drawing.Font("Arial", 9, System.Drawing.FontStyle.Bold)
        };

        Label lblInst1 = new Label()
        {
            Text = "1. Ingrese placa (6 caracteres)",
            Top = 200,
            Left = 10,
            Width = 280,
            Height = 15,
            Font = new System.Drawing.Font("Arial", 7, System.Drawing.FontStyle.Regular)
        };

        // Las instrucciones 2 y 3 no se muestran en el panelInfo debido a la altura de 220.
        // Podrías ajustar el Height del panelInfo o reorganizar el contenido.
        // Aquí ajusto el Top para que sean visibles si el panelInfo fuera más alto:
        // Label lblInst2 = new Label()
        // {
        //     Text = "2. Seleccione tipo de vehículo",
        //     Top = 215,
        //     Left = 10,
        //     Width = 280,
        //     Height = 15,
        //     Font = new System.Drawing.Font("Arial", 7, System.Drawing.FontStyle.Regular)
        // };

        // Label lblInst3 = new Label()
        // {
        //     Text = "3. Haga clic en 'Ingresar'",
        //     Top = 230,
        //     Left = 10,
        //     Width = 280,
        //     Height = 15,
        //     Font = new System.Drawing.Font("Arial", 7, System.Drawing.FontStyle.Regular)
        // };

        // Agregar controles al panel informativo
        panelInfo.Controls.Add(lblTituloInfo);
        panelInfo.Controls.Add(lblCapacidad);
        panelInfo.Controls.Add(lblHorario);
        panelInfo.Controls.Add(lblTarifas);
        panelInfo.Controls.Add(lblTarifaAuto);
        panelInfo.Controls.Add(lblTarifaMoto);
        panelInfo.Controls.Add(lblTarifaCamion);
        //panelInfo.Controls.Add(lblInstrucciones); // Si las instrucciones son muy largas, considera un TextBox read-only o LinkLabel
        //panelInfo.Controls.Add(lblInst1);
        //panelInfo.Controls.Add(lblInst2);
        //panelInfo.Controls.Add(lblInst3);


        Controls.Add(lblPlaca);
        Controls.Add(txtPlaca);
        Controls.Add(lblTipo);
        Controls.Add(cmbTipo);
        Controls.Add(btnIngresar);
        Controls.Add(btnRetirar);
        Controls.Add(btnExportar);
        Controls.Add(lstPlacas);
        Controls.Add(lblEspacios);
        Controls.Add(lstEspacios);
        Controls.Add(panelGrillaEspacios);
        Controls.Add(panelEstadisticas);
        Controls.Add(panelInfo);

        // Inicializar y añadir el PictureBox para el QR
        pbQRDisplay = new PictureBox()
        {
            Top = 10,  // Ajusta la posición según tu diseño
            Left = 250, // Ajusta la posición
            Width = 100, // Tamaño del QR
            Height = 100,
            BorderStyle = BorderStyle.FixedSingle,
            SizeMode = PictureBoxSizeMode.Zoom // Para que el QR se ajuste al tamaño del PictureBox
        };
        // Puedes agregar el PictureBox a un panel existente o directamente al formulario
        // Por ahora lo dejaré comentado, piensa dónde te gustaría que aparezca.
        // Controls.Add(pbQRDisplay);
        // Si quieres que aparezca en el panel de información, por ejemplo:
        // panelInfo.Controls.Add(pbQRDisplay); // Ajusta Top/Left si lo agregas a un panel

        ActualizarEstadisticas();
    }

    private void ActualizarEstadisticas()
    {
        int vehiculosActivos = sistema.ObtenerPlacasActuales().Count();
        int espaciosDisponibles = espacios.Length - espaciosOcupados.Count;

        lblVehiculosActivos.Text = $"Vehículos activos: {vehiculosActivos} / {espacios.Length}";

        var historial = sistema.ObtenerHistorial();
        decimal total = historial.Sum(h => h.Monto);
        lblTotalRecaudado.Text = $"Total recaudado: S/ {total:F2}";

        double promedio = historial.Any() ? historial.Average(h => h.Tiempo.TotalMinutes) : 0;
        lblPromedioPermanencia.Text = $"Promedio permanencia: {promedio:F1} min";

        // Actualizar grilla visual
        ActualizarGrillaEspacios();
    }

    private void BtnIngresar_Click(object sender, EventArgs e)
    {
        string placa = txtPlaca.Text.Trim().ToUpper();
        if (!System.Text.RegularExpressions.Regex.IsMatch(placa, @"^[A-Z0-9]{6}$"))
        {
            MessageBox.Show("La placa debe tener exactamente 6 letras o números (sin espacios).");
            return;
        }

        if (sistema.ObtenerPlacasActuales().Contains(placa))
        {
            MessageBox.Show("La placa ya fue registrada.");
            return;
        }

        // Verificar si hay espacios disponibles
        if (espaciosOcupados.Count >= espacios.Length)
        {
            MessageBox.Show("El estacionamiento está lleno. No hay espacios disponibles.");
            return;
        }

        // Encontrar el primer espacio disponible
        string espacioAsignado = espacios.FirstOrDefault(e => !espaciosOcupados.ContainsKey(e));

        if (espacioAsignado == null)
        {
            MessageBox.Show("El estacionamiento está lleno. No hay espacios disponibles.");
            return;
        }

        TipoVehiculo tipo = (TipoVehiculo)Enum.Parse(typeof(TipoVehiculo), cmbTipo.SelectedItem.ToString());
        sistema.IngresarVehiculo(new Vehiculo(placa, tipo, espacioAsignado));

        // Registrar el espacio ocupado
        espaciosOcupados[espacioAsignado] = placa;

        lstPlacas.Items.Add($"{placa} - {espacioAsignado}");
        lstEspacios.Items.Add($"{espacioAsignado}: {placa}");

        // --- Generación del Código QR ---
        // El contenido del QR podría ser la placa, o una cadena JSON con más info.
        // Para simplicidad, usaremos la placa.
        string qrContent = placa;
        GenerateAndShowQR(qrContent, placa); // Llama a la nueva función de generación de QR
        // --- Fin Generación del Código QR ---

        txtPlaca.Clear();
        ActualizarEstadisticas();
    }

    private void BtnRetirar_Click(object sender, EventArgs e)
    {
        if (lstPlacas.SelectedItem == null) return;

        string placaConEspacio = lstPlacas.SelectedItem.ToString();
        string placa = placaConEspacio.Split(' ')[0]; // Extraer solo la placa

        // Encontrar el espacio asignado
        string espacioLiberado = espaciosOcupados.FirstOrDefault(x => x.Value == placa).Key;

        TimeSpan duracion;
        decimal monto = sistema.RetirarVehiculo(placa, out duracion);

        // Buscar el registro correspondiente en el historial
        var registro = sistema.ObtenerHistorial().LastOrDefault(r => r.Placa == placa);

        // Generar recibo en txt
        if (registro != null)
        {
            // Modificado: Agrega timestamp al nombre del recibo
            string timestamp = DateTime.Now.ToString("yyyyMMdd_HHmmss");
            string nombreArchivo = $"recibo_{placa}_{timestamp}.txt"; // Nombre de archivo único
            string recibo = $"********** RECIBO FAST PARKING **********\r\n" +
                            $"Placa: {registro.Placa}\r\n" +
                            $"Tipo: {registro.Tipo}\r\n" +
                            $"Espacio: {espacioLiberado}\r\n" +
                            $"Hora de ingreso: {registro.HoraIngreso:dd/MM/yyyy HH:mm}\r\n" +
                            $"Hora de salida: {registro.HoraSalida:dd/MM/yyyy HH:mm}\r\n" +
                            $"Duración: {registro.Tiempo.TotalMinutes:F1} minutos\r\n" +
                            $"Monto: S/.{registro.Monto}\r\n" +
                            $"*****************************************";
            System.IO.File.WriteAllText(nombreArchivo, recibo);
            MessageBox.Show($"Recibo generado: {nombreArchivo}"); // Mensaje para indicar que se creó el recibo
        }

        MessageBox.Show($"Tiempo: {duracion.TotalMinutes:F1} minutos. Monto: S/.{monto}");

        // Liberar el espacio
        if (espacioLiberado != null)
        {
            espaciosOcupados.Remove(espacioLiberado);
            // Actualizar la lista de espacios
            lstEspacios.Items.Remove($"{espacioLiberado}: {placa}");
        }

        lstPlacas.Items.Remove(placaConEspacio);
        ActualizarEstadisticas();
    }


    private void BtnExportar_Click(object sender, EventArgs e)
    {
        var historial = sistema.ObtenerHistorial().ToList();

        using (var workbook = new XLWorkbook())
        {
            var ws = workbook.Worksheets.Add("Historial");

            // Encabezados
            ws.Cell(1, 1).Value = "Cantidad";
            ws.Cell(1, 2).Value = "Placa";
            ws.Cell(1, 3).Value = "Tipo";
            ws.Cell(1, 4).Value = "Duración (min)"; // Cambiado para claridad
            ws.Cell(1, 5).Value = "Hora Ingreso";
            ws.Cell(1, 6).Value = "Hora Salida";
            ws.Cell(1, 7).Value = "Monto S/";

            int row = 2;
            int contador = 1;
            foreach (var r in historial)
            {
                ws.Cell(row, 1).Value = contador;
                ws.Cell(row, 2).Value = r.Placa;
                ws.Cell(row, 3).Value = r.Tipo.ToString();
                ws.Cell(row, 4).Value = r.Tiempo.TotalMinutes.ToString("F2");
                ws.Cell(row, 5).Value = r.HoraIngreso.ToString("dd/MM/yyyy HH:mm");
                ws.Cell(row, 6).Value = r.HoraSalida.ToString("dd/MM/yyyy HH:mm");
                ws.Cell(row, 7).Value = r.Monto;
                row++;
                contador++;
            }

            // Fila de totales
            ws.Cell(row, 6).Value = "Total";
            ws.Cell(row, 7).FormulaA1 = $"SUM(G2:G{row - 1})"; // Suma la columna G

            // Ajustar ancho de columnas
            ws.Columns().AdjustToContents();

            workbook.SaveAs("historial_estacionamiento.xlsx");
        }

        MessageBox.Show("Historial exportado exitosamente.");
    }
    private void BtnCerrar_Click(object sender, EventArgs e)
    {
        var historial = sistema.ObtenerHistorial();
        int cantidad = historial.Count();
        decimal total = historial.Sum(r => r.Monto);

        MessageBox.Show($"Se retiraron {cantidad} vehículos.\nTotal ganado: S/.{total}", "Resumen final");

        Application.Exit(); // Cierra la app
        // ActualizarEstadisticas(); // Ya no es necesario aquí al cerrar la aplicación
    }
    private void CrearGrillaEspacios()
    {
        panelGrillaEspacios = new Panel()
        {
            Top = 365,
            Left = 20,
            Width = 340,
            Height = 90,
            BorderStyle = BorderStyle.FixedSingle,
            BackColor = System.Drawing.Color.LightGray
        };

        // Agregar título
        Label lblTitulo = new Label()
        {
            Text = "Estado de Espacios (Verde: Disponible, Rojo: Ocupado)",
            Top = 5,
            Left = 10,
            Width = 320,
            Font = new System.Drawing.Font("Arial", 8, System.Drawing.FontStyle.Bold),
            TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        };
        panelGrillaEspacios.Controls.Add(lblTitulo);

        lblEspaciosArray = new Label[espacios.Length];

        // Crear labels para cada espacio en una grilla 3x5
        for (int i = 0; i < espacios.Length; i++)
        {
            int fila = i / 5;  // 5 espacios por fila
            int columna = i % 5;

            lblEspaciosArray[i] = new Label()
            {
                Text = espacios[i],
                Top = 25 + (fila * 22),
                Left = 10 + (columna * 65),
                Width = 60,
                Height = 20,
                BackColor = System.Drawing.Color.LightGreen,
                ForeColor = System.Drawing.Color.Black,
                Font = new System.Drawing.Font("Arial", 8, System.Drawing.FontStyle.Bold),
                TextAlign = System.Drawing.ContentAlignment.MiddleCenter,
                BorderStyle = BorderStyle.FixedSingle
            };

            panelGrillaEspacios.Controls.Add(lblEspaciosArray[i]);
        }
    }

    private void ActualizarGrillaEspacios()
    {
        for (int i = 0; i < espacios.Length; i++)
        {
            string espacio = espacios[i];
            if (espaciosOcupados.ContainsKey(espacio))
            {
                // Espacio ocupado - color rojo
                lblEspaciosArray[i].BackColor = System.Drawing.Color.Red;
                lblEspaciosArray[i].ForeColor = System.Drawing.Color.White;
                // Ajusta el texto para que la placa se vea bien, quizás truncándola o reduciendo la fuente si el espacio es pequeño.
                // Por ahora, solo muestra la placa.
                lblEspaciosArray[i].Text = $"{espacio}\n{espaciosOcupados[espacio]}";
            }
            else
            {
                // Espacio disponible - color verde
                lblEspaciosArray[i].BackColor = System.Drawing.Color.LightGreen;
                lblEspaciosArray[i].ForeColor = System.Drawing.Color.Black;
                lblEspaciosArray[i].Text = espacio;
            }
        }
    }

    // --- Nueva Función para Generar y Mostrar QR ---
    private void GenerateAndShowQR(string content, string placa)
    {
        try
        {
            using (QRCodeGenerator qrGenerator = new QRCodeGenerator())
            {
                // Nivel de corrección de error (High, Medium, Low, Quartile)
                QRCodeData qrCodeData = qrGenerator.CreateQrCode(content, QRCodeGenerator.ECCLevel.Q);
                using (QRCode qrCode = new QRCode(qrCodeData))
                {
                    // Crear la imagen del QR
                    Bitmap qrCodeImage = qrCode.GetGraphic(20); // 20 es el tamaño de los "módulos" (cuadritos) del QR

                    // 1. Mostrar el QR en un PictureBox (si lo agregaste al formulario)
                    // if (pbQRDisplay != null)
                    // {
                    //     pbQRDisplay.Image = qrCodeImage;
                    // }

                    // 2. Guardar el QR en un archivo (por ejemplo, en la carpeta del proyecto)
                    string timestamp = DateTime.Now.ToString("yyyyMMdd_HHmmss");
                    string fileName = $"QR_{placa}_{timestamp}.png";
                    string filePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, fileName);
                    qrCodeImage.Save(filePath, System.Drawing.Imaging.ImageFormat.Png);
                    MessageBox.Show($"QR generado y guardado en: {fileName}", "QR Generado");

                    // 3. (Opcional) Abrir el archivo de imagen automáticamente
                    // System.Diagnostics.Process.Start(filePath);

                    // 4. (Opcional) Mostrar el QR en una ventana emergente temporal
                    // Si el QR debe imprimirse, esta sería una previsualización.
                    ShowQrPopup(qrCodeImage, placa);
                }
            }
        }
        catch (Exception ex)
        {
            MessageBox.Show($"Error al generar el QR: {ex.Message}", "Error de QR", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }

    private void ShowQrPopup(Bitmap qrImage, string placa)
    {
        Form qrForm = new Form();
        qrForm.Text = $"Ticket de Acceso - {placa}";
        qrForm.StartPosition = FormStartPosition.CenterScreen;
        qrForm.Size = new Size(350, 450); // Ajusta el tamaño de la ventana
        qrForm.FormBorderStyle = FormBorderStyle.FixedDialog; // Sin maximizar/minimizar
        qrForm.MaximizeBox = false;
        qrForm.MinimizeBox = false;

        Label lblTitulo = new Label()
        {
            Text = "¡Bienvenido a Fast Parking!",
            Top = 10,
            Left = 10,
            Width = 300,
            Height = 25,
            Font = new Font("Arial", 12, FontStyle.Bold),
            TextAlign = ContentAlignment.MiddleCenter
        };

        Label lblPlacaInfo = new Label()
        {
            Text = $"Placa: {placa}",
            Top = 40,
            Left = 10,
            Width = 300,
            Height = 20,
            Font = new Font("Arial", 10, FontStyle.Regular),
            TextAlign = ContentAlignment.MiddleCenter
        };

        PictureBox pbPopupQR = new PictureBox()
        {
            Image = qrImage,
            SizeMode = PictureBoxSizeMode.Zoom,
            Top = 70,
            Left = (qrForm.Width - 200) / 2 - 10, // Centrar el QR
            Width = 200,
            Height = 200,
            BorderStyle = BorderStyle.FixedSingle
        };

        Label lblInstruccionSalida = new Label()
        {
            Text = "Escanee este QR al salir",
            Top = 280,
            Left = 10,
            Width = 300,
            Height = 20,
            Font = new Font("Arial", 10, FontStyle.Bold),
            TextAlign = ContentAlignment.MiddleCenter
        };

        Button btnImprimir = new Button()
        {
            Text = "Imprimir Ticket",
            Top = 320,
            Left = (qrForm.Width - 150) / 2 - 10, // Centrar el botón
            Width = 150,
            Height = 30
        };
        btnImprimir.Click += (s, ev) =>
        {
            // Lógica para imprimir el QR
            MessageBox.Show("Funcionalidad de impresión no implementada. ¡Puedes añadirla aquí!", "Imprimir");
            // Para imprimir, necesitarías usar System.Drawing.Printing
            // y configurar un PrintDocument y PrintPreviewDialog.
        };

        qrForm.Controls.Add(lblTitulo);
        qrForm.Controls.Add(lblPlacaInfo);
        qrForm.Controls.Add(pbPopupQR);
        qrForm.Controls.Add(lblInstruccionSalida);
        qrForm.Controls.Add(btnImprimir);

        qrForm.ShowDialog(); // Muestra la ventana de forma modal
    }
}
// Las clases Vehiculo, Estacionamiento, TarifaPorTipoVehiculo, HistorialSalida,
// y el enum TipoVehiculo deben estar definidas en tu proyecto.
// Asegúrate de que esas clases existan y sean accesibles.

/*
// EJEMPLO de cómo podrían ser tus otras clases (si no las tienes ya):

public enum TipoVehiculo { Auto, Moto, Camion }

public class Vehiculo
{
    public string Placa { get; set; }
    public TipoVehiculo Tipo { get; set; }
    public DateTime HoraIngreso { get; set; }
    public string EspacioAsignado { get; set; }

    public Vehiculo(string placa, TipoVehiculo tipo, string espacioAsignado)
    {
        Placa = placa;
        Tipo = tipo;
        HoraIngreso = DateTime.Now;
        EspacioAsignado = espacioAsignado;
    }
}

public class RegistroSalida
{
    public string Placa { get; set; }
    public TipoVehiculo Tipo { get; set; }
    public DateTime HoraIngreso { get; set; }
    public DateTime HoraSalida { get; set; }
    public TimeSpan Tiempo { get; set; }
    public decimal Monto { get; set; }
    public string EspacioAsignado { get; set; } // Añadido para el recibo

    public RegistroSalida(string placa, TipoVehiculo tipo, DateTime horaIngreso, DateTime horaSalida, TimeSpan tiempo, decimal monto, string espacioAsignado)
    {
        Placa = placa;
        Tipo = tipo;
        HoraIngreso = horaIngreso;
        HoraSalida = horaSalida;
        Tiempo = tiempo;
        Monto = monto;
        EspacioAsignado = espacioAsignado;
    }
}

public interface ITarifa
{
    decimal CalcularCosto(TimeSpan duracion, TipoVehiculo tipo);
}

public class TarifaPorTipoVehiculo : ITarifa
{
    public decimal CalcularCosto(TimeSpan duracion, TipoVehiculo tipo)
    {
        decimal tarifaPorHora;
        switch (tipo)
        {
            case TipoVehiculo.Auto:
                tarifaPorHora = 2.00m;
                break;
            case TipoVehiculo.Moto:
                tarifaPorHora = 1.00m;
                break;
            case TipoVehiculo.Camion:
                tarifaPorHora = 3.00m;
                break;
            default:
                tarifaPorHora = 0.00m;
                break;
        }

        // Calcula el costo redondeando al minuto superior para el cálculo por hora
        // Si dura 1 hora y 1 minuto, cobra 2 horas.
        int horasCompletas = (int)Math.Ceiling(duracion.TotalHours);
        return horasCompletas * tarifaPorHora;
    }
}

public class Estacionamiento
{
    private Dictionary<string, Vehiculo> vehiculosActuales;
    private List<RegistroSalida> historialSalidas;
    private ITarifa _tarifa;
    private HistorialSalida _historialSalidaManager; // Renombrado para evitar conflicto

    public Estacionamiento(ITarifa tarifa, HistorialSalida historialSalidaManager)
    {
        vehiculosActuales = new Dictionary<string, Vehiculo>();
        historialSalidas = new List<RegistroSalida>(); // Se mantiene para el reporte y cálculo
        _tarifa = tarifa;
        _historialSalidaManager = historialSalidaManager;
    }

    public void IngresarVehiculo(Vehiculo vehiculo)
    {
        vehiculosActuales.Add(vehiculo.Placa, vehiculo);
    }

    public decimal RetirarVehiculo(string placa, out TimeSpan duracion)
    {
        if (vehiculosActuales.ContainsKey(placa))
        {
            Vehiculo vehiculo = vehiculosActuales[placa];
            DateTime horaSalida = DateTime.Now;
            duracion = horaSalida - vehiculo.HoraIngreso;
            decimal monto = _tarifa.CalcularCosto(duracion, vehiculo.Tipo);

            RegistroSalida registro = new RegistroSalida(
                vehiculo.Placa,
                vehiculo.Tipo,
                vehiculo.HoraIngreso,
                horaSalida,
                duracion,
                monto,
                vehiculo.EspacioAsignado // Guarda el espacio asignado en el historial
            );
            historialSalidas.Add(registro); // Añadir al historial local
            _historialSalidaManager.AgregarRegistro(registro); // Y al manager si lo usas para persistencia
            vehiculosActuales.Remove(placa);
            return monto;
        }
        duracion = TimeSpan.Zero;
        return 0;
    }

    public IEnumerable<string> ObtenerPlacasActuales()
    {
        return vehiculosActuales.Keys;
    }

    public IEnumerable<RegistroSalida> ObtenerHistorial()
    {
        return historialSalidas;
    }
}

// Clase para manejar el historial de salidas (si no lo tienes)
// Esta clase podría ser la responsable de guardar los recibos a disco si es necesario.
public class HistorialSalida
{
    private List<RegistroSalida> _registros;

    public HistorialSalida()
    {
        _registros = new List<RegistroSalida>();
    }

    public void AgregarRegistro(RegistroSalida registro)
    {
        _registros.Add(registro);
        // Aquí podrías agregar lógica para guardar el registro en una base de datos
        // o en un archivo persistente si lo deseas, aparte de la generación de recibo TXT.
    }

    public IEnumerable<RegistroSalida> ObtenerTodosLosRegistros()
    {
        return _registros;
    }
}
*/