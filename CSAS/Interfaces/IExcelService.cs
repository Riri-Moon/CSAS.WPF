using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSAS.Interfaces
{
    public interface IExcelService
    {
        void CreateFile(string filePath);
        void AddSheet(string sheetName);
        void RenameSheet(string oldSheetName, string newSheetName);
        void WriteRow(string sheetName, int rowIndex, bool overwriteStyle, params string[] values);
        void SaveFile();
        void SaveFileAs(string filePath);
    }
}
