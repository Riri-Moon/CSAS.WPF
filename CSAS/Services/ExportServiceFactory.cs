namespace CSAS.Services
{
	public class ExportServiceFactory
	{
		public static IExportService GetExportService(bool isExcel)
		{
			if (isExcel)
			{
				return new ExportExcelService();
			}
			else
			{
				return new ExportPDFService();
			}
		}
	}
}
