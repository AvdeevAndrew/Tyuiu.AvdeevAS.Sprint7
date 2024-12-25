// ���������� ������ Windows Forms � ����������� UI/UX

using System;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using Tyuiu.AvdeevAS.Sprint7.Project.V8;
using Tyuiu.AvdeevAS.Sprint7.Project.V8.Lib;

namespace Tyuiu.AvdeevAS.Sprint7.Project.V08
{
    public partial class MainForm_AAS : Form
    {
        private DataService dataService;
        private ToolTip toolTip;
        private TextBox searchBox;

        public MainForm_AAS()
        {
            InitializeComponent();
            InitializeDataGrid();
            InitializeToolTips();
            dataService = new DataService();
            Resize += MainForm_AAS_Resize;
        }

        private void InitializeComponent()
        {
            this.menuStripMain_AAS = new MenuStrip();
            this.toolStripMain_AAS = new ToolStrip();
            this.dataGridViewData_AAS = new DataGridView();

            // ���������� ���������-����� ����
            this.BackColor = Color.FromArgb(240, 240, 240);
            this.ForeColor = Color.FromArgb(50, 50, 50);

            // ����
            var fileMenu = new ToolStripMenuItem("����") { ForeColor = Color.Black };
            fileMenu.DropDownItems.AddRange(new ToolStripItem[]
            {
                new ToolStripMenuItem("�������", null, OnOpenFileClicked) { ToolTipText = "������� ���� � �������" },
                new ToolStripMenuItem("���������", null, OnSaveFileClicked) { ToolTipText = "��������� ������ � ����" },
                new ToolStripMenuItem("�����", null, OnExitClicked) { ToolTipText = "������� ����������" }
            });

            var helpMenu = new ToolStripMenuItem("�������") { ForeColor = Color.Black };
            helpMenu.DropDownItems.AddRange(new ToolStripItem[]
            {
                new ToolStripMenuItem("� ���������", null, OnAboutClicked) { ToolTipText = "���������� � ����������" },
                new ToolStripMenuItem("�����������", null, OnUserGuideClicked) { ToolTipText = "����������� ������������" }
            });

            this.menuStripMain_AAS.Items.AddRange(new ToolStripItem[] { fileMenu, helpMenu });
            this.menuStripMain_AAS.BackColor = Color.FromArgb(220, 220, 220);

            // ���������������� ������
            this.toolStripMain_AAS.Items.AddRange(new ToolStripItem[]
            {
                CreateToolStripButton("icons/add_icon.png", OnAddButtonClicked, "�������� ����� ������"),
                CreateToolStripButton("icons/edit_icon.png", OnEditButtonClicked, "������������� ������"),
                CreateToolStripButton("icons/delete_icon.png", OnDeleteButtonClicked, "������� ������")
            });
            this.toolStripMain_AAS.BackColor = Color.FromArgb(220, 220, 220);

            // ���� ������
            this.searchBox = new TextBox
            {
                PlaceholderText = "����� �� ��������� �����...",
                Width = 200
            };
            var searchBoxHost = new ToolStripControlHost(this.searchBox)
            {
                Alignment = ToolStripItemAlignment.Right
            };
            this.toolStripMain_AAS.Items.Add(searchBoxHost);
            this.searchBox.TextChanged += OnSearchTextChanged;

            // ������� ������
            this.dataGridViewData_AAS.Dock = DockStyle.Fill;
            this.dataGridViewData_AAS.AllowUserToAddRows = false;
            this.dataGridViewData_AAS.AllowUserToDeleteRows = false;
            this.dataGridViewData_AAS.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            this.dataGridViewData_AAS.BackgroundColor = Color.FromArgb(240, 240, 240);
            this.dataGridViewData_AAS.DefaultCellStyle.BackColor = Color.White;
            this.dataGridViewData_AAS.DefaultCellStyle.ForeColor = Color.Black;
            this.dataGridViewData_AAS.AlternatingRowsDefaultCellStyle.BackColor = Color.FromArgb(220, 220, 220);
            this.dataGridViewData_AAS.AlternatingRowsDefaultCellStyle.ForeColor = Color.Black;

            Controls.AddRange(new Control[] { dataGridViewData_AAS, toolStripMain_AAS, menuStripMain_AAS });

            this.MainMenuStrip = this.menuStripMain_AAS;
            this.Text = "������� ����";
            this.StartPosition = FormStartPosition.CenterScreen;
            this.Size = new System.Drawing.Size(1024, 768);
        }

        private void InitializeDataGrid()
        {
            this.dataGridViewData_AAS.Columns.Add("LicensePlate", "�������� ����");
            this.dataGridViewData_AAS.Columns.Add("Brand", "�����");
            this.dataGridViewData_AAS.Columns.Add("Condition", "���������");
            this.dataGridViewData_AAS.Columns.Add("Location", "���������������");
            this.dataGridViewData_AAS.Columns.Add("Speed", "������� ��������");
            this.dataGridViewData_AAS.Columns.Add("Capacity", "����������������");
            this.dataGridViewData_AAS.Columns.Add("FuelConsumption", "������ �������");
        }

        private void InitializeToolTips()
        {
            toolTip = new ToolTip();
            toolTip.SetToolTip(this.dataGridViewData_AAS, "������� ������ ����������������� �����������");
        }

        private ToolStripButton CreateToolStripButton(string imagePath, EventHandler onClick, string toolTipText)
        {
            var button = new ToolStripButton
            {
                Image = ResizeImage(Image.FromFile(imagePath), 104, 104), // ����������� ������ ������ ��������
                DisplayStyle = ToolStripItemDisplayStyle.Image,
                ToolTipText = toolTipText
            };
            button.Click += onClick;
            return button;
        }

        private Image ResizeImage(Image image, int width, int height)
        {
            var resized = new Bitmap(width, height);
            using (var graphics = Graphics.FromImage(resized))
            {
                graphics.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;
                graphics.DrawImage(image, 0, 0, width, height);
            }
            return resized;
        }

        private void OnOpenFileClicked(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog
            {
                Filter = "CSV Files (*.csv)|*.csv",
                Title = "������� ���� ������"
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
                Title = "��������� ���� ������"
            };

            if (saveFileDialog.ShowDialog() == DialogResult.OK)
            {
                var data = GetDataFromGrid();
                dataService.SaveData(saveFileDialog.FileName, data);
            }
        }

        private void OnExitClicked(object sender, EventArgs e)
        {
            this.Close();
        }

        private void OnAboutClicked(object sender, EventArgs e)
        {
            MessageBox.Show("���������� ��� ������ � ���������������� ������������.\n�����: ������ �.�.", "� ���������", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void OnUserGuideClicked(object sender, EventArgs e)
        {
            MessageBox.Show("������� ����������� ������������:\n1. ��� �������� ������ ����������� ����� ���� '���� -> �������'.\n2. ��� ���������� ������ ����������� '���� -> ���������'.\n3. ����������� ������ �� ������ ������������ ��� ����������, �������������� ��� �������� �������.", "����������� ������������", MessageBoxButtons.OK, MessageBoxIcon.Information);
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

        private void OnSearchTextChanged(object sender, EventArgs e)
        {
            string searchValue = searchBox.Text.ToLower();
            foreach (DataGridViewRow row in dataGridViewData_AAS.Rows)
            {
                row.Visible = string.IsNullOrWhiteSpace(searchValue) ||
                              row.Cells[0].Value?.ToString().ToLower().Contains(searchValue) == true;
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

        private void MainForm_AAS_Resize(object sender, EventArgs e)
        {
            foreach (ToolStripItem item in toolStripMain_AAS.Items)
            {
                if (item is ToolStripButton button && button.Image != null)
                {
                    button.Image = ResizeImage(button.Image, Math.Max(104, toolStripMain_AAS.Height - 10), Math.Max(104, toolStripMain_AAS.Height - 10));
                }
            }
        }

        private MenuStrip menuStripMain_AAS;
        private ToolStrip toolStripMain_AAS;
        private DataGridView dataGridViewData_AAS;
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
