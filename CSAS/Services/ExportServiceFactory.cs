namespace CSAS.Services
{
	public class ExportServiceFactory
	{
		public static IExportService GetExportService(bool isExcel)
		{
			return isExcel ? new ExportExcelService() : new ExportPDFService();
		}
	}
}
