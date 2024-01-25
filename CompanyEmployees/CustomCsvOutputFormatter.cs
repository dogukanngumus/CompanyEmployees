using System.Text;
using Microsoft.AspNetCore.Mvc.Formatters;
using Shared;

namespace CompanyEmployees;

public class CustomCsvOutputFormatter : TextOutputFormatter
{
    public CustomCsvOutputFormatter()
    {
        SupportedMediaTypes.Add("text/csv");
        SupportedEncodings.Add(Encoding.UTF8);
        SupportedEncodings.Add(Encoding.Unicode);
    }
    protected override bool CanWriteType(Type? type)
    {
        if(typeof(IEnumerable<CompanyDto>).IsAssignableFrom(type) || typeof(CompanyDto).IsAssignableFrom(type))
        {
            return true;
        }
        return false;
    }
    public override async Task WriteResponseBodyAsync(OutputFormatterWriteContext context, Encoding selectedEncoding)
    {
        var response = context.HttpContext.Response;
        StringBuilder buffer = new StringBuilder();
        if(context.Object is IEnumerable<CompanyDto>)
        {
            foreach (var company in (IEnumerable<CompanyDto>)context.Object)
            {
                FormatCsv(buffer, company);
            }
        }
        else
        {
             FormatCsv(buffer, (CompanyDto)context.Object);
        }

        await response.WriteAsync(buffer.ToString());
    }

    private static void FormatCsv(StringBuilder buffer, CompanyDto company)
    {
        buffer.AppendLine($"{company.Id},\"{company.Name},\"{company.FullAddress}\"");
    }
}
