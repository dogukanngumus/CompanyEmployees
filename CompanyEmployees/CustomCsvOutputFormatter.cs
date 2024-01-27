using System.Dynamic;
using System.Reflection;
using System.Text;
using Microsoft.AspNetCore.Mvc.Formatters;
using Shared.DataTransferObjects;

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
        if(typeof(IEnumerable<IDto>).IsAssignableFrom(type) || typeof(IDto).IsAssignableFrom(type))
        {
            return true;
        }
        return false;
    }
    public override async Task WriteResponseBodyAsync(OutputFormatterWriteContext context, Encoding selectedEncoding)
    {
        var response = context.HttpContext.Response;
        
        StringBuilder buffer = new StringBuilder();
        if(context.Object is IEnumerable<object> enumerableObjects)
        {    
            foreach(var item in enumerableObjects)
            {
                FormatCsv(buffer, item);
            }       
        }
        else
        {
            FormatCsv(buffer, context.Object);
        }

        await response.WriteAsync(buffer.ToString());
    }

    private void FormatCsv(StringBuilder buffer, object item){
        var typeOfObject = item.GetType();
        var result = typeOfObject.GetProperties().Select(x=> x.GetValue(item)).ToList();
        buffer.AppendLine(String.Join(',',result));
    }
}
