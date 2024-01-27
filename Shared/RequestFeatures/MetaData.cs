namespace Shared.RequestFeatures;

public class MetaData
{
    public int CurrentPage { get; set; } // Hangi sayfada bulunduğumuz
    public int TotalPages { get; set; } // Toplam sayfa sayısı
    public int PageSize { get; set; } // Bir sayfada kaç item bulunabileceği
    public int TotalCount { get; set; } // Toplam item sayısı.

    public bool HasPrevious => CurrentPage > 1;
    public bool HasNext => CurrentPage < TotalPages;    
}
