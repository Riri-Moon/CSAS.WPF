using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Spreadsheet;
using System.Data;

namespace CSAS.Services
{
	public class ExcelService : IExcelService
	{
		private SpreadsheetDocument? _excelFile;
		private UInt32 _sheetId = 0;

		public void AddSheet(string sheetName)
		{
			WorksheetPart worksheetPart = _excelFile.WorkbookPart.AddNewPart<WorksheetPart>();
			worksheetPart.Worksheet = new Worksheet(new SheetData());

			Sheet sheet = new()
			{
				Id = _excelFile.WorkbookPart.GetIdOfPart(worksheetPart),
				SheetId = ++_sheetId,
				Name = sheetName
			};

			_excelFile.WorkbookPart.Workbook.Sheets.Append(sheet);
		}

		public void CreateFile(string filePath)
		{
			_excelFile = SpreadsheetDocument.Create(filePath, SpreadsheetDocumentType.MacroEnabledWorkbook);

			WorkbookPart workbookPart = _excelFile.AddWorkbookPart();
			workbookPart.Workbook = new Workbook();

			_excelFile.WorkbookPart.Workbook.AppendChild<Sheets>(new Sheets());
		}
		public void CreateSpreadsheetWorkbook(string filepath, string sheetName)
		{

			_excelFile = SpreadsheetDocument.
				Create(filepath, SpreadsheetDocumentType.Workbook);

			// Add a WorkbookPart to the document.
			WorkbookPart workbookpart = _excelFile.AddWorkbookPart();
			workbookpart.Workbook = new Workbook();

			// Add a WorksheetPart to the WorkbookPart.
			WorksheetPart worksheetPart = workbookpart.AddNewPart<WorksheetPart>();
			worksheetPart.Worksheet = new Worksheet(new SheetData());

			Sheets sheets = _excelFile.WorkbookPart.Workbook.
				AppendChild<Sheets>(new Sheets());

			Sheet sheet = new()
			{
				Id = _excelFile.WorkbookPart.
				GetIdOfPart(worksheetPart),
				SheetId = 1,
				Name = sheetName
			};
			sheets.Append(sheet);

			workbookpart.Workbook.Save();
		}
		public void OpenDocument(string filePath)
		{
			_excelFile = SpreadsheetDocument.Open(filePath, true);
		}

		public void RenameSheet(string oldSheetName, string newSheetName)
		{
			var sheet = _excelFile.WorkbookPart.Workbook.Sheets.Descendants<Sheet>().Single(s => s.Name == oldSheetName);
			sheet.Name = newSheetName;
		}

		public void SaveFile()
		{
			_excelFile.Save();
			_excelFile.Close();
		}

		public void SaveFileAs(string filePath)
		{
			_excelFile.SaveAs(filePath).Close();
			_excelFile.Close();
		}

		public void WriteRow(string sheetName, int rowIndex, bool overwriteStyle, params string[] values)
		{
			var sheet = _excelFile.WorkbookPart.Workbook.Sheets.Descendants<Sheet>().Where(s => s.Name == sheetName).Single();
			var worksheetPart = (WorksheetPart)_excelFile.WorkbookPart.GetPartById(sheet.Id);

			int columnIndex = 1;
			foreach (var value in values)
			{
				var cell = InsertCellInWorksheet(GetExcelColumnName(columnIndex), (uint)rowIndex, worksheetPart );
				cell.CellValue = new CellValue(value);
				cell.DataType = new EnumValue<CellValues>(CellValues.String);
				columnIndex++;
			}
		}

		public List<string> ReadRow()
		{
			List<string> rows = new();
			SheetData sheetData = _excelFile.WorkbookPart.WorksheetParts.FirstOrDefault().Worksheet.Elements<SheetData>().First();
			foreach (Row r in sheetData.Elements<Row>())
			{
				foreach (Cell c in r.Elements<Cell>())
				{
					if (c.CellValue != null)
					{
						rows.Add(c.CellValue.Text);
					}
				}
			}

			return rows;
		}

		public void RemoveRow()
		{
			Worksheet worksheet = _excelFile.WorkbookPart.WorksheetParts.FirstOrDefault().Worksheet;
			SheetData sheetData = worksheet.GetFirstChild<SheetData>();

			if (sheetData.Elements<Row>().Any())
			{
				sheetData.Elements<Row>().First().Remove();
			}
		}

		private static Cell InsertCellInWorksheet(string columnName, uint rowIndex, WorksheetPart worksheetPart)
		{
			Worksheet worksheet = worksheetPart.Worksheet;
			SheetData sheetData = worksheet.GetFirstChild<SheetData>();
			string cellReference = columnName + rowIndex;

			Row row;
			if (sheetData.Elements<Row>().Where(r => r.RowIndex == rowIndex).Any())
			{
				row = sheetData.Elements<Row>().Where(r => r.RowIndex == rowIndex).Single();
			}
			else
			{
				row = new Row() { RowIndex = rowIndex };
				sheetData.Append(row);
			}

			Cell refCell = row.Descendants<Cell>().LastOrDefault();
			Cell newCell = new() { CellReference = cellReference };
			row.InsertAfter(newCell, refCell);

			return newCell;

		}

		private static string GetCellValue(WorkbookPart wbPart, List<Cell> theCells, string cellColumnReference)
		{
			Cell theCell = null;
			string value = "";
			foreach (Cell cell in theCells)
			{
				if (cell.CellReference.Value.StartsWith(cellColumnReference))
				{
					theCell = cell;
					break;
				}
			}
			if (theCell != null)
			{
				value = theCell.InnerText;

				if (theCell.DataType != null)
				{
					switch (theCell.DataType.Value)
					{
						case CellValues.SharedString:
							var stringTable = wbPart.GetPartsOfType<SharedStringTablePart>().FirstOrDefault();
							if (stringTable != null)
							{
								value = stringTable.SharedStringTable.ElementAt(int.Parse(value)).InnerText;
							}
							break;
						case CellValues.Boolean:
							value = value switch
							{
								"0" => "FALSE",
								_ => "TRUE",
							};
							break;
					}
				}
			}
			return value;
		}

		private static string GetCellValue(WorkbookPart wbPart, List<Cell> theCells, int index)
		{
			return GetCellValue(wbPart, theCells, GetExcelColumnName(index));
		}

		private static string GetExcelColumnName(int columnNumber)
		{
			int dividend = columnNumber;
			string columnName = String.Empty;
			int modulo;
			while (dividend > 0)
			{
				modulo = (dividend - 1) % 26;
				columnName = Convert.ToChar(65 + modulo).ToString() + columnName;
				dividend = (int)((dividend - modulo) / 26);
			}
			return columnName;
		}

		//Only xlsx files
		public static DataTable GetDataTableFromExcelFile(string filePath, string sheetName = "")
		{
			DataTable dt = new();
			try
			{
				using SpreadsheetDocument document = SpreadsheetDocument.Open(filePath, false);
				WorkbookPart wbPart = document.WorkbookPart;
				IEnumerable<Sheet> sheets = document.WorkbookPart.Workbook.GetFirstChild<Sheets>().Elements<Sheet>();
				string sheetId = sheetName != "" ? sheets.Where(q => q.Name == sheetName).First().Id.Value : sheets.First().Id.Value;
				WorksheetPart wsPart = (WorksheetPart)wbPart.GetPartById(sheetId);
				SheetData sheetdata = wsPart.Worksheet.Elements<SheetData>().FirstOrDefault();
				int totalHeaderCount = sheetdata.Descendants<Row>().ElementAt(5).Descendants<Cell>().Count();

				var rowsToSkip = new List<int>();
				//Get the header                    
				for (int i = 1; i <= totalHeaderCount; i++)
				{
					var value = GetCellValue(wbPart, sheetdata.Descendants<Row>().ElementAt(0).Elements<Cell>().ToList(), i);
					if (!dt.Columns.Contains(value))
					{
						dt.Columns.Add(value);
					}
					else
					{
						rowsToSkip.Add(i);
					}
				}
				foreach (Row r in sheetdata.Descendants<Row>())
				{
					if (r.RowIndex > 1)
					{
						DataRow tempRow = dt.NewRow();

						//Always get from the header count, because the index of the row changes where empty cell is not counted
						for (int i = 0; i <= dt.Columns.Count - 1; i++)
						{
							if (!rowsToSkip.Contains(i))
							{
								try
								{
									tempRow[i] = GetCellValue(wbPart, r.Elements<Cell>().ToList(), i);
								}
								catch (Exception ex)
								{
									var x = "sa";
								}
							}
						}
						dt.Rows.Add(tempRow);
					}
				}
			}
			catch (Exception ex)
			{

			}
			return dt;
		}
	}
}

