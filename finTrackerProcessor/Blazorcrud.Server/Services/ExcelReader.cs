using System.Collections.Generic;
using System.Data;

namespace Blazorcrud.Server.Services
{
    public interface IExcelReader
    {
        object GetEntries(string filePath);
    }

    public class ExcelReader : IExcelReader
    {
        private readonly IFileRepository _myFileRepository;


        public ExcelReader(IFileRepository newFileRepository)
        {
            _myFileRepository = newFileRepository;
        }

        public object GetEntries(string filePath)
        {
            var dataTable = _myFileRepository.ReadExcelFile(filePath);

            return GetEntries(dataTable);
        }

        private object GetEntries(DataTable? table)
        {
            //     ICategory parentCat = null;
            //     SingleExpense singleRecord = null;
            //     CategoryTree categoryTree = new CategoryTree();
            //
            //     var entryRecords = new Expenses
            //     {
            //         Entries = new List<SingleExpense>(),
            //         CategoryTree = categoryTree
            //     };
            //
            //     if (table?.Rows == null) {
            //         return entryRecords;
            //     }
            //
            //     foreach (DataRow dr in table.Rows)
            //     {
            //         singleRecord = new SingleExpense();
            //         
            //         singleRecord.Date = SqlHelper.GetDateTime(dr["Date"]);
            //         singleRecord.Amount = Math.Abs(SqlHelper.GetDecimal(dr["Amount"]));
            //         singleRecord.Merchant = SqlHelper.GetString(dr["Merchant"]);
            //         
            //         var accountTypeStr = SqlHelper.GetString(dr["Account"]);
            //         singleRecord.SetAccountType(accountTypeStr);
            //
            //         var transTypeStr = SqlHelper.GetString(dr["Transaction Type"]);
            //         singleRecord.SetTransactionType(transTypeStr);
            //         
            //         var catName = SqlHelper.GetString(dr["Parent Categories"]);
            //         if(!string.IsNullOrEmpty(catName))
            //         {
            //             parentCat = new Category { Name = catName };
            //             singleRecord.ParentCategory = parentCat;
            //         }
            //
            //         var cat = new Category { Name = SqlHelper.GetString(dr["Category"]), ParentCategory = parentCat };
            //         categoryTree[cat] = cat;
            //         singleRecord.Category = cat;                    
            //         
            //         entryRecords.Entries.Add(singleRecord);
            //     }
            return null;
        }
    }
}