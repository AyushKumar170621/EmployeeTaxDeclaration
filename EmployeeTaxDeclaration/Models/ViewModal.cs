namespace EmployeeTaxDeclaration.Models
{
    public class ViewModal
    {
        public IQueryable<TaxForm> Form { get; set; }
        public int PageSize { get; set; }
        public int CurrentPage { get; set; }
        public int TotalPage { get; set; }

        public string finacialYear { get; set; } 
        public string employeeId { get; set; } 

        public string employeeName { get; set; }

        public Dictionary<string ,int > IdMap { get; set; }
        public string declarationStatus { get; set;}
    }
}
