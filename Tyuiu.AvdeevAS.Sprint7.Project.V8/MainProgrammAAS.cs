using System;
using System.Windows.Forms;
using Tyuiu.AvdeevAS.Sprint7.Project.V8;
using Tyuiu.AvdeevAS.Sprint7.Project.V8.Lib;

namespace Tyuiu.AvdeevAS.Sprint7.Project.V08
{
    public partial class MainForm_AAS : Form
    {
        private DataService dataService;
        private ToolTip toolTip;

        public MainForm_AAS()
        {
            InitializeComponent();
            InitializeDataGrid();
            InitializeToolTips();
            dataService = new DataService();
        }

        private void InitializeComponent()
        {
            this.menuStripMain_AAS = new MenuStrip();
            this.toolStripMain_AAS = new ToolStrip();
            this.dataGridViewData_AAS = new DataGridView();
            this.panelMain_AAS = new Panel();

            // Меню
            var fileMenu = new ToolStripMenuItem("Файл");
            fileMenu.DropDownItems.AddRange(new ToolStripItem[]
            {
                new ToolStripMenuItem("Открыть", null, OnOpenFileClicked) { ToolTipText = "Открыть файл с данными" },
                new ToolStripMenuItem("Сохранить", null, OnSaveFileClicked) { ToolTipText = "Сохранить данные в файл" },
                new ToolStripMenuItem("Выход", null, OnExitClicked) { ToolTipText = "Закрыть приложение" }
            });

            var helpMenu = new ToolStripMenuItem("Справка");
            helpMenu.DropDownItems.AddRange(new ToolStripItem[]
            {
                new ToolStripMenuItem("О программе", null, OnAboutClicked) { ToolTipText = "Информация о приложении" },
                new ToolStripMenuItem("Руководство", null, OnUserGuideClicked) { ToolTipText = "Руководство пользователя" }
            });

            this.menuStripMain_AAS.Items.AddRange(new ToolStripItem[] { fileMenu, helpMenu });

            // Инструментальная панель
            this.toolStripMain_AAS.Items.AddRange(new ToolStripItem[]
            {
                new ToolStripButton("Добавить", null, OnAddButtonClicked) { ToolTipText = "Добавить новую запись" },
                new ToolStripButton("Редактировать", null, OnEditButtonClicked) { ToolTipText = "Редактировать выбранную запись" },
                new ToolStripButton("Удалить", null, OnDeleteButtonClicked) { ToolTipText = "Удалить выбранную запись" }
            });

            this.dataGridViewData_AAS.Dock = DockStyle.Fill;
            this.dataGridViewData_AAS.AllowUserToAddRows = false;
            this.dataGridViewData_AAS.AllowUserToDeleteRows = false;
            this.dataGridViewData_AAS.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;

            this.panelMain_AAS.Dock = DockStyle.Bottom;
            this.panelMain_AAS.Height = 50;

            Controls.AddRange(new Control[] { dataGridViewData_AAS, panelMain_AAS, toolStripMain_AAS, menuStripMain_AAS });

            this.MainMenuStrip = this.menuStripMain_AAS;
            this.Text = "Автотранспортное предприятие";
            this.StartPosition = FormStartPosition.CenterScreen;
            this.Size = new System.Drawing.Size(1024, 768);
        }

        private void InitializeDataGrid()
        {
            this.dataGridViewData_AAS.Columns.Add("LicensePlate", "Номерной знак");
            this.dataGridViewData_AAS.Columns.Add("Brand", "Марка");
            this.dataGridViewData_AAS.Columns.Add("Condition", "Состояние");
            this.dataGridViewData_AAS.Columns.Add("Location", "Местонахождение");
            this.dataGridViewData_AAS.Columns.Add("Speed", "Средняя скорость");
            this.dataGridViewData_AAS.Columns.Add("Capacity", "Грузоподъемность");
            this.dataGridViewData_AAS.Columns.Add("FuelConsumption", "Расход топлива");
        }

        private void InitializeToolTips()
        {
            toolTip = new ToolTip();
            toolTip.SetToolTip(this.dataGridViewData_AAS, "Таблица данных автотранспортного предприятия");
        }

        private void OnOpenFileClicked(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog
            {
                Filter = "CSV Files (*.csv)|*.csv",
                Title = "Открыть файл данных"
            };

            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                var data = dataService.LoadData(openFileDialog.FileName);
                UpdateDataGrid(data);
            }
        }

        private void OnSaveFileClicked(object sender, EventArgs e)
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog
            {
                Filter = "CSV Files (*.csv)|*.csv",
                Title = "Сохранить файл данных"
            };

            if (saveFileDialog.ShowDialog() == DialogResult.OK)
            {
                var data = GetDataFromGrid();
                dataService.SaveData(saveFileDialog.FileName, data);
            }
        }

        private void OnExitClicked(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void OnAboutClicked(object sender, EventArgs e)
        {
            MessageBox.Show("Приложение для работы с автотранспортным предприятием.\nАвтор: Авдеев А.С.", "О программе", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void OnUserGuideClicked(object sender, EventArgs e)
        {
            MessageBox.Show("Краткое руководство пользователя:\n1. Для загрузки данных используйте пункт меню 'Файл -> Открыть'.\n2. Для сохранения данных используйте 'Файл -> Сохранить'.\n3. Используйте кнопки на панели инструментов для добавления, редактирования или удаления записей.", "Руководство пользователя", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void OnAddButtonClicked(object sender, EventArgs e)
        {
            using (var form = new EditForm_AAS())
            {
                if (form.ShowDialog() == DialogResult.OK)
                {
                    var row = form.RowData;
                    dataGridViewData_AAS.Rows.Add(row);
                }
            }
        }

        private void OnEditButtonClicked(object sender, EventArgs e)
        {
            if (dataGridViewData_AAS.SelectedRows.Count > 0)
            {
                var selectedRow = dataGridViewData_AAS.SelectedRows[0];

                using (var form = new EditForm_AAS(GetRowData(selectedRow)))
                {
                    if (form.ShowDialog() == DialogResult.OK)
                    {
                        var row = form.RowData;
                        UpdateRowData(selectedRow, row);
                    }
                }
            }
        }

        private void OnDeleteButtonClicked(object sender, EventArgs e)
        {
            if (dataGridViewData_AAS.SelectedRows.Count > 0)
            {
                dataGridViewData_AAS.Rows.Remove(dataGridViewData_AAS.SelectedRows[0]);
            }
        }

        private void UpdateDataGrid(List<string[]> data)
        {
            dataGridViewData_AAS.Rows.Clear();

            foreach (var row in data)
            {
                dataGridViewData_AAS.Rows.Add(row);
            }
        }

        private List<string[]> GetDataFromGrid()
        {
            var data = new List<string[]>();

            foreach (DataGridViewRow row in dataGridViewData_AAS.Rows)
            {
                if (!row.IsNewRow)
                {
                    var values = new List<string>();
                    foreach (DataGridViewCell cell in row.Cells)
                    {
                        values.Add(cell.Value?.ToString() ?? string.Empty);
                    }
                    data.Add(values.ToArray());
                }
            }

            return data;
        }

        private string[] GetRowData(DataGridViewRow row)
        {
            var values = new List<string>();
            foreach (DataGridViewCell cell in row.Cells)
            {
                values.Add(cell.Value?.ToString() ?? string.Empty);
            }
            return values.ToArray();
        }

        private void UpdateRowData(DataGridViewRow row, string[] values)
        {
            for (int i = 0; i < values.Length; i++)
            {
                row.Cells[i].Value = values[i];
            }
        }

        private MenuStrip menuStripMain_AAS;
        private ToolStrip toolStripMain_AAS;
        private DataGridView dataGridViewData_AAS;
        private Panel panelMain_AAS;
    }

    static class Program
    {
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new MainForm_AAS());
        }
    }
}
