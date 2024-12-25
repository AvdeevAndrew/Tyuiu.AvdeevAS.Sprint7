using System;
using System.Windows.Forms;

namespace Tyuiu.AvdeevAS.Sprint7.Project.V8
{
    public partial class EditForm_AAS : Form
    {
        // Свойство для хранения данных строки
        public string[] RowData { get; set; }

        public EditForm_AAS(string[] rowData = null)
        {
            InitializeComponent();

            // Если переданы данные для редактирования, заполняем поля формы
            if (rowData != null)
            {
                RowData = rowData;
                FillFormFields();
            }
            else
            {
                // Если данные не переданы, создаем пустую строку для новой записи
                RowData = new string[7];
            }
        }

        private void InitializeComponent()
        {
            this.Text = "Редактирование записи";
            this.StartPosition = FormStartPosition.CenterParent;
            this.Size = new System.Drawing.Size(400, 400);

            // Создание и настройка элементов управления на форме
            var labelLicensePlate = new Label() { Text = "Номерной знак", Location = new System.Drawing.Point(20, 20) };
            var textBoxLicensePlate = new TextBox() { Name = "textBoxLicensePlate", Location = new System.Drawing.Point(150, 20), Width = 200 };
            var labelBrand = new Label() { Text = "Марка", Location = new System.Drawing.Point(20, 60) };
            var textBoxBrand = new TextBox() { Name = "textBoxBrand", Location = new System.Drawing.Point(150, 60), Width = 200 };
            var labelCondition = new Label() { Text = "Состояние", Location = new System.Drawing.Point(20, 100) };
            var textBoxCondition = new TextBox() { Name = "textBoxCondition", Location = new System.Drawing.Point(150, 100), Width = 200 };
            var labelLocation = new Label() { Text = "Местонахождение", Location = new System.Drawing.Point(20, 140) };
            var textBoxLocation = new TextBox() { Name = "textBoxLocation", Location = new System.Drawing.Point(150, 140), Width = 200 };
            var labelSpeed = new Label() { Text = "Средняя скорость", Location = new System.Drawing.Point(20, 180) };
            var textBoxSpeed = new TextBox() { Name = "textBoxSpeed", Location = new System.Drawing.Point(150, 180), Width = 200 };
            var labelCapacity = new Label() { Text = "Грузоподъемность", Location = new System.Drawing.Point(20, 220) };
            var textBoxCapacity = new TextBox() { Name = "textBoxCapacity", Location = new System.Drawing.Point(150, 220), Width = 200 };
            var labelFuelConsumption = new Label() { Text = "Расход топлива", Location = new System.Drawing.Point(20, 260) };
            var textBoxFuelConsumption = new TextBox() { Name = "textBoxFuelConsumption", Location = new System.Drawing.Point(150, 260), Width = 200 };

            // Кнопки сохранения и отмены
            var buttonSave = new Button() { Text = "Сохранить", Location = new System.Drawing.Point(150, 300), Width = 100 };
            buttonSave.Click += ButtonSave_Click;

            var buttonCancel = new Button() { Text = "Отмена", Location = new System.Drawing.Point(260, 300), Width = 100 };
            buttonCancel.Click += (sender, e) => this.DialogResult = DialogResult.Cancel;

            // Добавление элементов управления на форму
            this.Controls.AddRange(new Control[] {
                labelLicensePlate, textBoxLicensePlate,
                labelBrand, textBoxBrand,
                labelCondition, textBoxCondition,
                labelLocation, textBoxLocation,
                labelSpeed, textBoxSpeed,
                labelCapacity, textBoxCapacity,
                labelFuelConsumption, textBoxFuelConsumption,
                buttonSave, buttonCancel
            });
        }

        // Метод для заполнения полей формы при редактировании записи
        private void FillFormFields()
        {
            // Привязка данных из RowData к полям формы
            var controls = this.Controls;
            controls["textBoxLicensePlate"].Text = RowData[0];
            controls["textBoxBrand"].Text = RowData[1];
            controls["textBoxCondition"].Text = RowData[2];
            controls["textBoxLocation"].Text = RowData[3];
            controls["textBoxSpeed"].Text = RowData[4];
            controls["textBoxCapacity"].Text = RowData[5];
            controls["textBoxFuelConsumption"].Text = RowData[6];
        }

        // Метод для сохранения данных из формы в RowData
        private void SaveFormData()
        {
            var controls = this.Controls;
            RowData[0] = controls["textBoxLicensePlate"].Text;
            RowData[1] = controls["textBoxBrand"].Text;
            RowData[2] = controls["textBoxCondition"].Text;
            RowData[3] = controls["textBoxLocation"].Text;
            RowData[4] = controls["textBoxSpeed"].Text;
            RowData[5] = controls["textBoxCapacity"].Text;
            RowData[6] = controls["textBoxFuelConsumption"].Text;
        }

        // Обработчик кнопки "Сохранить"
        private void ButtonSave_Click(object sender, EventArgs e)
        {
            SaveFormData();
            this.DialogResult = DialogResult.OK;
        }
    }
}
