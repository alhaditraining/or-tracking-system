using System.Text;
using ClosedXML.Excel;
using ORTrackingSystem.Models.ViewModels;

namespace ORTrackingSystem.Services;

public class ExportService
{
    private byte[] AddBomToCsv(string csvContent)
    {
        var preamble = Encoding.UTF8.GetPreamble();
        var csvBytes = Encoding.UTF8.GetBytes(csvContent);
        var result = new byte[preamble.Length + csvBytes.Length];
        Buffer.BlockCopy(preamble, 0, result, 0, preamble.Length);
        Buffer.BlockCopy(csvBytes, 0, result, preamble.Length, csvBytes.Length);
        return result;
    }
    
    private void StyleHeaderRow(IXLRow headerRow)
    {
        headerRow.Style.Font.Bold = true;
        headerRow.Style.Fill.BackgroundColor = XLColor.LightBlue;
    }
    
    public byte[] ExportORReportsToCsv(List<ORReportRow> data)
    {
        var csv = new StringBuilder();
        
        // Header
        csv.AppendLine("OR Number,Contract Name,Budget Type,Status,Total Value,Invoiced Total,Remaining,Created Date");
        
        // Data
        foreach (var row in data)
        {
            csv.AppendLine($"\"{row.ORNumber}\",\"{row.ContractName}\",\"{row.BudgetType}\",\"{row.Status}\",{row.TotalValue},{row.InvoicedTotal},{row.Remaining},\"{row.CreatedDate:yyyy-MM-dd}\"");
        }
        
        return AddBomToCsv(csv.ToString());
    }
    
    public byte[] ExportInvoiceReportsToCsv(List<InvoiceReportRow> data)
    {
        var csv = new StringBuilder();
        
        // Header
        csv.AppendLine("OR Number,Contract Name,Budget Type,Invoice Number,Amount,Paid,Paid Date,Created Date");
        
        // Data
        foreach (var row in data)
        {
            var paidStatus = row.IsPaid == true ? "Paid" : "Unpaid";
            var paidDate = row.PaidDate?.ToString("yyyy-MM-dd") ?? "";
            csv.AppendLine($"\"{row.ORNumber}\",\"{row.ContractName}\",\"{row.BudgetType}\",\"{row.InvoiceNumber}\",{row.Amount},\"{paidStatus}\",\"{paidDate}\",\"{row.CreatedDate:yyyy-MM-dd}\"");
        }
        
        return AddBomToCsv(csv.ToString());
    }
    
    public byte[] ExportStationeryReportsToCsv(List<StationeryInvoiceReportRow> data)
    {
        var csv = new StringBuilder();
        
        // Header
        csv.AppendLine("Invoice Number,Amount,Paid,Paid Date,Created Date");
        
        // Data
        foreach (var row in data)
        {
            var paidStatus = row.IsPaid == true ? "Paid" : "Unpaid";
            var paidDate = row.PaidDate?.ToString("yyyy-MM-dd") ?? "";
            csv.AppendLine($"\"{row.InvoiceNumber}\",{row.Amount},\"{paidStatus}\",\"{paidDate}\",\"{row.CreatedDate:yyyy-MM-dd}\"");
        }
        
        return AddBomToCsv(csv.ToString());
    }
    
    public byte[] ExportLMRequestsToCsv(List<LMRequestRow> data)
    {
        var csv = new StringBuilder();
        
        // Header
        csv.AppendLine("Request Number,Amount,Completed,Cancelled,Created Date");
        
        // Data
        foreach (var row in data)
        {
            var completedStatus = row.IsCompleted == true ? "Yes" : row.IsCompleted == false ? "No" : "-";
            var cancelledStatus = row.IsCancelled == true ? "Yes" : row.IsCancelled == false ? "No" : "-";
            csv.AppendLine($"\"{row.RequestNumber}\",{row.Amount},\"{completedStatus}\",\"{cancelledStatus}\",\"{row.CreatedDate:yyyy-MM-dd}\"");
        }
        
        return AddBomToCsv(csv.ToString());
    }
    
    public byte[] ExportInvoicesToCsv(List<InvoiceRow> data)
    {
        var csv = new StringBuilder();
        
        // Header
        csv.AppendLine("Invoice Number,Amount,Paid,Paid Date,Created Date,Delivery Notes,Invoice Images");
        
        // Data
        foreach (var row in data)
        {
            var paidStatus = row.IsPaid == true ? "Paid" : "Unpaid";
            var paidDate = row.PaidDate?.ToString("yyyy-MM-dd") ?? "";
            csv.AppendLine($"\"{row.InvoiceNumber}\",{row.Amount},\"{paidStatus}\",\"{paidDate}\",\"{row.CreatedDate:yyyy-MM-dd}\",{row.DeliveryNoteCount},{row.InvoiceImageCount}");
        }
        
        return AddBomToCsv(csv.ToString());
    }
    
    // Excel exports
    public byte[] ExportORReportsToExcel(List<ORReportRow> data)
    {
        using var workbook = new XLWorkbook();
        var worksheet = workbook.Worksheets.Add("OR Reports");
        
        // Header
        worksheet.Cell(1, 1).Value = "OR Number";
        worksheet.Cell(1, 2).Value = "Contract Name";
        worksheet.Cell(1, 3).Value = "Budget Type";
        worksheet.Cell(1, 4).Value = "Status";
        worksheet.Cell(1, 5).Value = "Total Value";
        worksheet.Cell(1, 6).Value = "Invoiced Total";
        worksheet.Cell(1, 7).Value = "Remaining";
        worksheet.Cell(1, 8).Value = "Created Date";
        
        // Style header
        StyleHeaderRow(worksheet.Row(1));
        
        // Data
        for (int i = 0; i < data.Count; i++)
        {
            var row = data[i];
            var rowNum = i + 2;
            
            worksheet.Cell(rowNum, 1).Value = row.ORNumber;
            worksheet.Cell(rowNum, 2).Value = row.ContractName;
            worksheet.Cell(rowNum, 3).Value = row.BudgetType.ToString();
            worksheet.Cell(rowNum, 4).Value = row.Status.ToString();
            worksheet.Cell(rowNum, 5).Value = row.TotalValue;
            worksheet.Cell(rowNum, 6).Value = row.InvoicedTotal;
            worksheet.Cell(rowNum, 7).Value = row.Remaining;
            worksheet.Cell(rowNum, 8).Value = row.CreatedDate.ToString("yyyy-MM-dd");
        }
        
        // Auto-fit columns
        worksheet.Columns().AdjustToContents();
        
        using var stream = new MemoryStream();
        workbook.SaveAs(stream);
        return stream.ToArray();
    }
    
    public byte[] ExportInvoiceReportsToExcel(List<InvoiceReportRow> data)
    {
        using var workbook = new XLWorkbook();
        var worksheet = workbook.Worksheets.Add("Invoice Reports");
        
        // Header
        worksheet.Cell(1, 1).Value = "OR Number";
        worksheet.Cell(1, 2).Value = "Contract Name";
        worksheet.Cell(1, 3).Value = "Budget Type";
        worksheet.Cell(1, 4).Value = "Invoice Number";
        worksheet.Cell(1, 5).Value = "Amount";
        worksheet.Cell(1, 6).Value = "Paid";
        worksheet.Cell(1, 7).Value = "Paid Date";
        worksheet.Cell(1, 8).Value = "Created Date";
        
        // Style header
        StyleHeaderRow(worksheet.Row(1));
        
        // Data
        for (int i = 0; i < data.Count; i++)
        {
            var row = data[i];
            var rowNum = i + 2;
            
            worksheet.Cell(rowNum, 1).Value = row.ORNumber;
            worksheet.Cell(rowNum, 2).Value = row.ContractName;
            worksheet.Cell(rowNum, 3).Value = row.BudgetType.ToString();
            worksheet.Cell(rowNum, 4).Value = row.InvoiceNumber;
            worksheet.Cell(rowNum, 5).Value = row.Amount;
            worksheet.Cell(rowNum, 6).Value = row.IsPaid == true ? "Paid" : "Unpaid";
            worksheet.Cell(rowNum, 7).Value = row.PaidDate?.ToString("yyyy-MM-dd") ?? "";
            worksheet.Cell(rowNum, 8).Value = row.CreatedDate.ToString("yyyy-MM-dd");
        }
        
        // Auto-fit columns
        worksheet.Columns().AdjustToContents();
        
        using var stream = new MemoryStream();
        workbook.SaveAs(stream);
        return stream.ToArray();
    }
    
    public byte[] ExportStationeryReportsToExcel(List<StationeryInvoiceReportRow> data)
    {
        using var workbook = new XLWorkbook();
        var worksheet = workbook.Worksheets.Add("Stationery Reports");
        
        // Header
        worksheet.Cell(1, 1).Value = "Invoice Number";
        worksheet.Cell(1, 2).Value = "Amount";
        worksheet.Cell(1, 3).Value = "Paid";
        worksheet.Cell(1, 4).Value = "Paid Date";
        worksheet.Cell(1, 5).Value = "Created Date";
        
        // Style header
        StyleHeaderRow(worksheet.Row(1));
        
        // Data
        for (int i = 0; i < data.Count; i++)
        {
            var row = data[i];
            var rowNum = i + 2;
            
            worksheet.Cell(rowNum, 1).Value = row.InvoiceNumber;
            worksheet.Cell(rowNum, 2).Value = row.Amount;
            worksheet.Cell(rowNum, 3).Value = row.IsPaid == true ? "Paid" : "Unpaid";
            worksheet.Cell(rowNum, 4).Value = row.PaidDate?.ToString("yyyy-MM-dd") ?? "";
            worksheet.Cell(rowNum, 5).Value = row.CreatedDate.ToString("yyyy-MM-dd");
        }
        
        // Auto-fit columns
        worksheet.Columns().AdjustToContents();
        
        using var stream = new MemoryStream();
        workbook.SaveAs(stream);
        return stream.ToArray();
    }
    
    public byte[] ExportLMRequestsToExcel(List<LMRequestRow> data)
    {
        using var workbook = new XLWorkbook();
        var worksheet = workbook.Worksheets.Add("LM Requests");
        
        // Header
        worksheet.Cell(1, 1).Value = "Request Number";
        worksheet.Cell(1, 2).Value = "Amount";
        worksheet.Cell(1, 3).Value = "Completed";
        worksheet.Cell(1, 4).Value = "Cancelled";
        worksheet.Cell(1, 5).Value = "Created Date";
        
        // Style header
        StyleHeaderRow(worksheet.Row(1));
        
        // Data
        for (int i = 0; i < data.Count; i++)
        {
            var row = data[i];
            var rowNum = i + 2;
            
            worksheet.Cell(rowNum, 1).Value = row.RequestNumber;
            worksheet.Cell(rowNum, 2).Value = row.Amount;
            worksheet.Cell(rowNum, 3).Value = row.IsCompleted == true ? "Yes" : row.IsCompleted == false ? "No" : "-";
            worksheet.Cell(rowNum, 4).Value = row.IsCancelled == true ? "Yes" : row.IsCancelled == false ? "No" : "-";
            worksheet.Cell(rowNum, 5).Value = row.CreatedDate.ToString("yyyy-MM-dd");
        }
        
        // Auto-fit columns
        worksheet.Columns().AdjustToContents();
        
        using var stream = new MemoryStream();
        workbook.SaveAs(stream);
        return stream.ToArray();
    }
    
    public byte[] ExportInvoicesToExcel(List<InvoiceRow> data)
    {
        using var workbook = new XLWorkbook();
        var worksheet = workbook.Worksheets.Add("Invoices");
        
        // Header
        worksheet.Cell(1, 1).Value = "Invoice Number";
        worksheet.Cell(1, 2).Value = "Amount";
        worksheet.Cell(1, 3).Value = "Paid";
        worksheet.Cell(1, 4).Value = "Paid Date";
        worksheet.Cell(1, 5).Value = "Created Date";
        worksheet.Cell(1, 6).Value = "Delivery Notes";
        worksheet.Cell(1, 7).Value = "Invoice Images";
        
        // Style header
        StyleHeaderRow(worksheet.Row(1));
        
        // Data
        for (int i = 0; i < data.Count; i++)
        {
            var row = data[i];
            var rowNum = i + 2;
            
            worksheet.Cell(rowNum, 1).Value = row.InvoiceNumber;
            worksheet.Cell(rowNum, 2).Value = row.Amount;
            worksheet.Cell(rowNum, 3).Value = row.IsPaid == true ? "Paid" : "Unpaid";
            worksheet.Cell(rowNum, 4).Value = row.PaidDate?.ToString("yyyy-MM-dd") ?? "";
            worksheet.Cell(rowNum, 5).Value = row.CreatedDate.ToString("yyyy-MM-dd");
            worksheet.Cell(rowNum, 6).Value = row.DeliveryNoteCount;
            worksheet.Cell(rowNum, 7).Value = row.InvoiceImageCount;
        }
        
        // Auto-fit columns
        worksheet.Columns().AdjustToContents();
        
        using var stream = new MemoryStream();
        workbook.SaveAs(stream);
        return stream.ToArray();
    }
}
