namespace Blazorcrud.Shared.Models
{
    public class Category
    {
        public int Id { get; set; }
        
        public int ExternalIdentifier { get; set; }
        public Category? ParentCategory { get; set;}
        public int Order { get; set; }
        public string Title { get; set; }
        public string PrefixedTitle { get; set; }
        public string Colour { get; set; }
        public bool IsBill { get; set; }
        public bool IsTransfer { get; set; }
        
        public DateTime DateCreated {get; set;} = default!;
        
        public DateTime? DateDeleted {get; set;} = default!;
    }
    
    public class MerchantKeyword 
    {
        public int Id { get; set; }
        public string Keyword { get; set; }
        public DateTime DateCreated {get; set;}
        
        public DateTime? DateDeleted {get; set;}
    }

    public class Tag
    {
        public int Id { get; set; }
        public string Value {get; set;} = default!;
        public DateTime DateCreated {get; set;}
        public DateTime? DateDeleted {get; set;}
    }

    public class MerchantKeywordsCategory
    {
        public int Id { get; set; }
        public MerchantKeyword Keyword { get; set;} = default!;
        public Category Category { get; set;} = default!;
        public List<Tag> Tags { get; set;} = default!;
        public bool IsDisabled {get; set;} = default!;
        public DateTime DateCreated {get; set;}
        public DateTime? DateDeleted {get; set;}
        
    }
}