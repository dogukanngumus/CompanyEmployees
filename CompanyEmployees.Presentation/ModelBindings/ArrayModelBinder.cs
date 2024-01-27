using System.ComponentModel;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace CompanyEmployees.Presentation.ModelBindings;

public class ArrayModelBinder : IModelBinder
{
    public Task BindModelAsync(ModelBindingContext bindingContext)
    {
        if(!bindingContext.ModelMetadata.IsEnumerableType)
        {
            bindingContext.Result = ModelBindingResult.Failed();
            return Task.CompletedTask;
        }

        var providedValue = bindingContext.ValueProvider.GetValue(bindingContext.ModelName).ToString();
        if(string.IsNullOrEmpty(providedValue))
        {
            bindingContext.Result = ModelBindingResult.Success(null);
            return Task.CompletedTask;
        }

        var genericType = bindingContext.ModelType.GetGenericArguments()[0];
        var splittedArray = providedValue.Split(",",StringSplitOptions.RemoveEmptyEntries);
        var converter = TypeDescriptor.GetConverter(genericType);
        var objectArray = splittedArray.Select(x=> converter.ConvertFromString(x)).ToArray();

        var guidArray = Array.CreateInstance(genericType,objectArray.Length);
        objectArray.CopyTo(guidArray,0);
        bindingContext.Model = guidArray;

        bindingContext.Result = ModelBindingResult.Success(bindingContext.Model);
        return Task.CompletedTask;
    }
}
