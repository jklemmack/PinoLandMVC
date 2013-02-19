using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Spreadsheet;


namespace OpenXMLHelpers
{
    class OpenXMLDataTableHelper
    {

        private struct Reference
        {
            public int Column;
            public int Row;

            public Reference(string reference)
            {
                this.Column = GetIndexFromColumn(new Regex(@"[A-Za-z]+").Match(reference).Value);
                this.Row = Int32.Parse(new Regex(@"\d+").Match(reference).Value);
            }

            public Reference(int row, int column)
            {
                this.Column = column;
                this.Row = row;
            }


        }

        private static string GetCellValue(SpreadsheetDocument document, Cell cell)
        {
            string value = null;
            if (cell.CellValue != null)
                value = cell.CellValue.InnerXml;

            if (cell.DataType != null)
                if (cell.DataType.Value == CellValues.SharedString)
                {
                    SharedStringTablePart sharedStringTablePart = document.WorkbookPart.SharedStringTablePart;
                    return sharedStringTablePart.SharedStringTable.ChildElements[Int32.Parse(value)].InnerText;
                }

            return value;
        }

        private static int GetIndexFromColumn(string column)
        {
            int result = 0;
            foreach (char c in column)
                result += result * 26 + (c - 64);
            return result;
        }

        private static string GetColumnFromIndex(long columnNumber)
        {
            long dividend = columnNumber;
            string columnName = String.Empty;
            long modulo;

            while (dividend > 0)
            {
                modulo = (dividend - 1) % 26;
                columnName = Convert.ToChar(65 + modulo).ToString() + columnName;
                dividend = (long)((dividend - modulo) / 26);
            }

            return columnName;
        }

        private static Reference GetCellReference(string cellName)
        {
            return new Reference(cellName);
        }

        public static DataTable GetDataTable(SpreadsheetDocument document, string sheetName, string topLeftCell, DataTable dataTable)
        {
            IEnumerable<Sheet> sheets = document.WorkbookPart.Workbook.Sheets.Descendants<Sheet>().Where(s => s.Name == sheetName);
            if (sheetName.Count() == 0)
                throw new ArgumentException(string.Format("Sheet '{0}' not found.", sheetName));

            WorksheetPart worksheetPart = (WorksheetPart)document.WorkbookPart.GetPartById(sheets.First().Id);
            SheetData sheetData = worksheetPart.Worksheet.Elements<SheetData>().First();

            //Assume first row has headers
            // Build a column mapping using the first row as headers
            Reference topLeft = GetCellReference(topLeftCell);
            Row firstRow = sheetData.Descendants<Row>().Skip(topLeft.Row - 1).First();
            Dictionary<string, string> dataToSpreadMap = new Dictionary<string, string>();

            foreach (DataColumn dc in dataTable.Columns)
            {
                Cell cell = firstRow.Descendants<Cell>().Where(c => GetCellValue(document, c) == dc.ColumnName).FirstOrDefault();
                if (cell != null)
                    dataToSpreadMap.Add(dc.ColumnName, GetColumnFromIndex(new Reference(cell.CellReference).Column));
                else
                {
                    //Error?
                }
            }

            //Now keep going while we have contiguous data
            uint lastRowIndex = firstRow.RowIndex;
            Row currentRow = firstRow.NextSibling<Row>();
            while (currentRow != null && currentRow.RowIndex == lastRowIndex + 1)
            {
                DataRow tempRow = dataTable.NewRow();
                bool dataAdded = false;
                foreach (string dCol in dataToSpreadMap.Keys)
                {
                    Cell cell = currentRow.Descendants<Cell>().Where(c => c.CellReference == dataToSpreadMap[dCol] + currentRow.RowIndex.ToString()).SingleOrDefault();
                    if (cell != null)
                    {
                        object value = GetCellValue(document, cell);
                        if (value != null && value != DBNull.Value)
                        {
                            tempRow[dCol] = value;
                            dataAdded = true;
                        }
                    }
                }

                if (!dataAdded)
                    break;

                dataTable.Rows.Add(tempRow);

                lastRowIndex = currentRow.RowIndex;
                currentRow = currentRow.NextSibling<Row>();
            }

            return dataTable;

        }

        public static DataTable GetDataTable(SpreadsheetDocument document, string sheetName, string topLeftCell)
        {
            DataTable dataTable = new DataTable(sheetName);

            IEnumerable<Sheet> sheets = document.WorkbookPart.Workbook.Sheets.Descendants<Sheet>().Where(s => s.Name == sheetName);
            if (sheetName.Count() == 0)
                throw new ArgumentException(string.Format("Sheet '{0}' not found.", sheetName));

            WorksheetPart worksheetPart = (WorksheetPart)document.WorkbookPart.GetPartById(sheets.First().Id);
            SheetData sheetData = worksheetPart.Worksheet.Elements<SheetData>().First();

            //Assume first row has headers
            // Build a column mapping using the first row as headers
            Reference topLeft = GetCellReference(topLeftCell);
            Row firstRow = sheetData.Descendants<Row>().Skip(topLeft.Row - 1).First();
            Dictionary<string, string> dataToSpreadMap = new Dictionary<string, string>();

            //Iterate over contiguous cells
            long lastColumn = topLeft.Column - 1;
            Cell currentCell = (Cell)firstRow.FirstChild;
            do
            {
                Reference r = GetCellReference(currentCell.CellReference);
                //Check cells are contiguous
                if (r.Column != lastColumn + 1)
                    break;
                string name = GetCellValue(document, currentCell);
                dataTable.Columns.Add(name, typeof(string));
                dataToSpreadMap.Add(name, GetColumnFromIndex(r.Column));
                lastColumn = r.Column;
                currentCell = currentCell.NextSibling<Cell>();

            } while (currentCell != null);



            //Now read row data while we have contiguous data
            uint lastRowIndex = firstRow.RowIndex;
            Row currentRow = firstRow.NextSibling<Row>();
            while (currentRow != null && currentRow.RowIndex == lastRowIndex + 1)
            {
                DataRow tempRow = dataTable.NewRow();
                bool dataAdded = false;
                foreach (string dCol in dataToSpreadMap.Keys)
                {
                    Cell cell = currentRow.Descendants<Cell>().Where(c => c.CellReference == dataToSpreadMap[dCol] + currentRow.RowIndex.ToString()).SingleOrDefault();
                    if (cell != null)
                    {
                        tempRow[dCol] = GetCellValue(document, cell);
                        dataAdded = true;
                    }
                }

                if (!dataAdded)
                    break;

                dataTable.Rows.Add(tempRow);

                lastRowIndex = currentRow.RowIndex;             //Mark where we were
                currentRow = currentRow.NextSibling<Row>();     //And advance

            }

            return dataTable;

        }

    }
}
