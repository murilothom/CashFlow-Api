using System.Reflection;
using MigraDoc.DocumentObjectModel;
using PdfSharp.Fonts;

namespace CashFlow.Application.UseCases.Expenses.Reports.Pdf.Fonts;

public class ExpensesReportFontResolver : IFontResolver
{
    public byte[]? GetFont(string faceName)
    {
        var stream = ReadFontFile(faceName) ?? ReadFontFile(FontHelper.DEFAULT);
        // stream ??= ReadFontFile(FontHelper.DEFAULT);

        var length = stream!.Length;

        var data = new byte[length];

        stream.Read(buffer: data, offset: 0, count: (int)length);

        return data;
    }

    public FontResolverInfo? ResolveTypeface(string familyName, bool bold, bool italic)
    {
        return new FontResolverInfo(familyName);
    }

    private Stream? ReadFontFile(string faceName)
    {
        var assembly = Assembly.GetExecutingAssembly();

        return assembly.GetManifestResourceStream($"CashFlow.Application.UseCases.Expenses.Reports.Pdf.Fonts.{faceName}.ttf");
    }
}
