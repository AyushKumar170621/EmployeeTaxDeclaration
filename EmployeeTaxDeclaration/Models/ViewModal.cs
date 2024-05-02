namespace EmployeeTaxDeclaration.Models
{
    public class ViewModal
    {
        public IQueryable<TaxForm> Form { get; set; }
        public int PageSize { get; set; }
        public int CurrentPage { get; set; }
        public int TotalPage { get; set; }
    }
}
