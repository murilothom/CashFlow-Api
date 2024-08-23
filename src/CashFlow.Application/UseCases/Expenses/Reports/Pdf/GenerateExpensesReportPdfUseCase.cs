using System.Reflection;
using CashFlow.Application.UseCases.Expenses.Reports.Pdf.Colors;
using CashFlow.Application.UseCases.Expenses.Reports.Pdf.Fonts;
using CashFlow.Domain.Extensions;
using CashFlow.Domain.Reports;
using CashFlow.Domain.Repositories.Expenses;
using CashFlow.Domain.Services.LoggedUser;
using MigraDoc.DocumentObjectModel;
using MigraDoc.DocumentObjectModel.Tables;
using MigraDoc.Rendering;
using PdfSharp.Fonts;

namespace CashFlow.Application.UseCases.Expenses.Reports.Pdf;

public class GenerateExpensesReportPdfUseCase : IGenerateExpensesReportPdfUseCase
{
    private const string CURRENCY_SYMBOL = "$";
    private const int ROW_HEIGHT = 25;
    
    private readonly IExpensesRepository _repository;
    private readonly ILoggedUser _loggedUser;

    public GenerateExpensesReportPdfUseCase(
        IExpensesRepository repository,
        ILoggedUser loggedUser)
    {
        _repository = repository;
        _loggedUser = loggedUser;

        GlobalFontSettings.FontResolver = new ExpensesReportFontResolver();
    }

    public async Task<byte[]> Execute(DateOnly month)
    {
        var user = await _loggedUser.Get();

        var expenses = await _repository.FilterByMonth(month, user.Id);

        var document = CreateDocument(user.Name, month);
        var section = CreateSection(document);

        CreateHeaderWithLogoAndName(user.Name, section);

        var totalExpenses = expenses.Sum(expense => expense.Amount);
        CreateTotalExpenseSection(section, month, totalExpenses);

        foreach (var expense in expenses)
        {
            var table = CreateExpenseTable(section);

            // Primeira linha
            var firstRow = table.AddRow();
            firstRow.Height = ROW_HEIGHT;

            // Primeira coluna da primeira linha
            AddExpenseTitle(firstRow.Cells[0], expense.Title);
            
            // Quarta coluna da primeira linha
            AddExpenseHeaderAmount(firstRow.Cells[3]);

            // Segunda linha
            var secondRow = table.AddRow();
            secondRow.Height = ROW_HEIGHT;

            // Primeira coluna da segunda linha
            secondRow.Cells[0].AddParagraph(expense.Date.ToString("D"));
            SetBaseStyleExpenseInformation(secondRow.Cells[0]);
            secondRow.Cells[0].Format.LeftIndent = 20;

            // Segunda coluna da segunda linha
            secondRow.Cells[1].AddParagraph(expense.Date.ToString("t"));
            SetBaseStyleExpenseInformation(secondRow.Cells[1]);
            
            // Terceira coluna da segunda linha
            secondRow.Cells[2].AddParagraph(expense.PaymentType.PaymentTypeToString());
            SetBaseStyleExpenseInformation(secondRow.Cells[2]);

            // Quarta coluna da segunda linha
            AddExpenseAmount(secondRow.Cells[3], expense.Amount);

            if (string.IsNullOrWhiteSpace(expense.Description) is false)
            {
                // Terceira linha
                var thirdRow = table.AddRow();
                thirdRow.Height = ROW_HEIGHT;

                // Primeira coluna da terceira linha
                AddDescription(thirdRow.Cells[0], expense.Description);
            }

            // Espaçamento entre as linahs (gap)
            AddWhiteSpace(table);
        }
        
        return RenderDocument(document);
    }

    private Document CreateDocument(string name, DateOnly month)
    {
        var document = new Document();
        document.Info.Title = $"{ResourceReportGenerationMessage.EXPENSES_FOR} {month:Y}";
        document.Info.Author = $"Hey, {name}";

        var style = document.Styles["Normal"];
        style!.Font.Name = FontHelper.RALEWAY_REGULAR;

        return document;
    }

    private Section CreateSection(Document document)
    {
        var section = document.AddSection();
        section.PageSetup = document.DefaultPageSetup.Clone();
        
        section.PageSetup.PageFormat = PageFormat.A4;
        section.PageSetup.RightMargin = 40;
        section.PageSetup.LeftMargin = 40;
        section.PageSetup.TopMargin = 80;
        section.PageSetup.BottomMargin = 80;

        return section;
    }

    private void CreateHeaderWithLogoAndName(string name, Section section)
    {
        var table = section.AddTable();
        table.AddColumn();
        table.AddColumn("300");

        var row = table.AddRow();

        var assembly = Assembly.GetExecutingAssembly();
        var directoryName = Path.GetDirectoryName(assembly.Location);
        var pathFile = Path.Combine(directoryName!, "Logo", "Logo-62x62.png");

        row.Cells[0].AddImage(pathFile);

        row.Cells[1].AddParagraph($"Hey, {name}");
        row.Cells[1].Format.Font = new Font { Name = FontHelper.RALEWAY_BLACK, Size = 16 };
        row.Cells[1].VerticalAlignment = VerticalAlignment.Center;
    }

    private void CreateTotalExpenseSection(Section section, DateOnly month, decimal totalExpenses)
    {
        var paragraph = section.AddParagraph();
        var title = string.Format(ResourceReportGenerationMessage.TOTAL_SPENT_IN, month.ToString("Y"));

        paragraph.AddFormattedText(title, new Font { Name = FontHelper.RALEWAY_REGULAR, Size = 15 });
        paragraph.Format.SpaceBefore = "40";
        paragraph.Format.SpaceAfter = "40";
        
        paragraph.AddLineBreak();

        paragraph.AddFormattedText($"{CURRENCY_SYMBOL} {totalExpenses}", new Font { Name = FontHelper.WORKSANS_BLACK, Size = 50 });
    }

    private Table CreateExpenseTable(Section section)
    {
        var table = section.AddTable();
        table.AddColumn("195").Format.Alignment = ParagraphAlignment.Left;
        table.AddColumn("80").Format.Alignment = ParagraphAlignment.Center;
        table.AddColumn("120").Format.Alignment = ParagraphAlignment.Center;
        table.AddColumn("120").Format.Alignment = ParagraphAlignment.Right;

        return table;
    }

    private void AddExpenseTitle(Cell cell, string title)
    {
        cell.AddParagraph(title);
        cell.Format.Font = new Font { Name = FontHelper.RALEWAY_BLACK, Size = 14, Color = ColorsHelper.BLACK };
        cell.Shading.Color = ColorsHelper.RED_LIGHT;
        cell.VerticalAlignment = VerticalAlignment.Center;
        cell.MergeRight = 2;
        cell.Format.LeftIndent = 20;
    }

    private void AddExpenseHeaderAmount(Cell cell)
    {
        cell.AddParagraph(ResourceReportGenerationMessage.AMOUNT);
        cell.Format.Font = new Font { Name = FontHelper.RALEWAY_BLACK, Size = 14, Color = ColorsHelper.WHITE };
        cell.Shading.Color = ColorsHelper.RED_DARK;
        cell.VerticalAlignment = VerticalAlignment.Center;
    }

    private void SetBaseStyleExpenseInformation(Cell cell)
    {
        cell.Format.Font = new Font { Name = FontHelper.WORKSANS_REGULAR, Size = 12, Color = ColorsHelper.BLACK };
        cell.Shading.Color = ColorsHelper.GREEN_DARK;
        cell.VerticalAlignment = VerticalAlignment.Center;
    }

    private void AddExpenseAmount(Cell cell, decimal amount)
    {
        cell.AddParagraph($"- {CURRENCY_SYMBOL} {amount}");
        cell.Format.Font = new Font { Name = FontHelper.WORKSANS_REGULAR, Size = 14, Color = ColorsHelper.BLACK };
        cell.Shading.Color = ColorsHelper.WHITE;
        cell.VerticalAlignment = VerticalAlignment.Center;
    }

    private void AddWhiteSpace(Table table)
    {
        var rowSpacing = table.AddRow();
        rowSpacing.Height = 30;
        rowSpacing.Borders.Visible = false;
    }

    private void AddDescription(Cell cell, string description)
    {
        cell.AddParagraph(description);
        cell.Format.Font = new Font { Name = FontHelper.WORKSANS_REGULAR, Size = 10, Color = ColorsHelper.BLACK };
        cell.Shading.Color = ColorsHelper.GREEN_LIGHT;
        cell.VerticalAlignment = VerticalAlignment.Center;
        cell.MergeRight = 2;
        cell.Format.LeftIndent = 20;
    }

    private byte[] RenderDocument(Document document)
    {
        var renderer = new PdfDocumentRenderer
        {
            Document = document
        };
        
        renderer.RenderDocument();

        using var file = new MemoryStream();
        renderer.PdfDocument.Save(file);

        return file.ToArray();
    }
}