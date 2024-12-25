using System;
using System.Collections.Generic;
using System.IO;
using System.Globalization;

namespace Tyuiu.AvdeevAS.Sprint7.Project.V8.Lib
{
    public class DataService
    {
        /// <summary>
        /// Загружает данные из CSV файла.
        /// </summary>
        /// <param name="filePath">Путь к файлу.</param>
        /// <returns>Список строк данных.</returns>
        public List<string[]> LoadData(string filePath)
        {
            var data = new List<string[]>();
            using (var reader = new StreamReader(filePath))
            {
                string line;
                while ((line = reader.ReadLine()) != null)
                {
                    var values = line.Split(';');
                    data.Add(values);
                }
            }
            return data;
        }

        /// <summary>
        /// Сохраняет данные в CSV файл.
        /// </summary>
        /// <param name="filePath">Путь к файлу.</param>
        /// <param name="data">Данные для сохранения.</param>
        public void SaveData(string filePath, List<string[]> data)
        {
            using (var writer = new StreamWriter(filePath))
            {
                foreach (var row in data)
                {
                    writer.WriteLine(string.Join(";", row));
                }
            }
        }

        /// <summary>
        /// Добавляет новую строку в таблицу данных.
        /// </summary>
        /// <param name="data">Существующие данные.</param>
        /// <param name="newRow">Новая строка.</param>
        public void AddRow(List<string[]> data, string[] newRow)
        {
            data.Add(newRow);
        }

        /// <summary>
        /// Редактирует строку в таблице данных.
        /// </summary>
        /// <param name="data">Существующие данные.</param>
        /// <param name="rowIndex">Индекс редактируемой строки.</param>
        /// <param name="updatedRow">Обновленная строка.</param>
        public void EditRow(List<string[]> data, int rowIndex, string[] updatedRow)
        {
            if (rowIndex < 0 || rowIndex >= data.Count)
                throw new ArgumentOutOfRangeException(nameof(rowIndex), "Индекс строки вне допустимого диапазона.");

            data[rowIndex] = updatedRow;
        }

        /// <summary>
        /// Удаляет строку из таблицы данных.
        /// </summary>
        /// <param name="data">Существующие данные.</param>
        /// <param name="rowIndex">Индекс удаляемой строки.</param>
        public void DeleteRow(List<string[]> data, int rowIndex)
        {
            if (rowIndex < 0 || rowIndex >= data.Count)
                throw new ArgumentOutOfRangeException(nameof(rowIndex), "Индекс строки вне допустимого диапазона.");

            data.RemoveAt(rowIndex);
        }
    }
}
