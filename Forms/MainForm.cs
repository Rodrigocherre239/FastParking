using ClosedXML.Excel;
using System;
using System.Windows.Forms;
using System.Linq;


public class MainForm : Form
{
    private Button btnIngresar;
    private Button btnRetirar;
    private Button btnExportar;
    private TextBox txtPlaca;
    private ComboBox cmbTipo;
    private ListBox lstPlacas;
    private Button btnCerrar;


    private Estacionamiento sistema;

    public MainForm()
    {
        this.Text = "FAST PARKING";
        this.Width = 400;
        this.Height = 400;

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
        btnExportar = new Button() { Text = "Exportar Historial", Top = 250, Left = 100, Width = 180 };
        btnCerrar = new Button()
{
    Text = "Cerrar",
    Top = 300,
    Left = 150,
    Width = 100,
    Height = 35
};
btnCerrar.Click += BtnCerrar_Click;
Controls.Add(btnCerrar);


        btnIngresar.Click += BtnIngresar_Click;
        btnRetirar.Click += BtnRetirar_Click;
        btnExportar.Click += BtnExportar_Click;

        lstPlacas = new ListBox() { Top = 140, Left = 20, Width = 340, Height = 100 };

        Controls.Add(lblPlaca);
        Controls.Add(txtPlaca);
        Controls.Add(lblTipo);
        Controls.Add(cmbTipo);
        Controls.Add(btnIngresar);
        Controls.Add(btnRetirar);
        Controls.Add(btnExportar);
        Controls.Add(lstPlacas);
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

        TipoVehiculo tipo = (TipoVehiculo)Enum.Parse(typeof(TipoVehiculo), cmbTipo.SelectedItem.ToString());
        sistema.IngresarVehiculo(new Vehiculo(placa, tipo));
        lstPlacas.Items.Add(placa);
        txtPlaca.Clear();
    }

    private void BtnRetirar_Click(object sender, EventArgs e)
{
    if (lstPlacas.SelectedItem == null) return;

    string placa = lstPlacas.SelectedItem.ToString();

    TimeSpan duracion;
    decimal monto = sistema.RetirarVehiculo(placa, out duracion);
    MessageBox.Show($"Tiempo: {duracion.TotalMinutes:F1} minutos. Monto: S/.{monto}");

    lstPlacas.Items.Remove(placa);
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
            ws.Cell(1, 4).Value = "Duración";
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
            ws.Cell(row, 7).FormulaA1 = $"SUM(G2:G{row - 1})";

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
}

}