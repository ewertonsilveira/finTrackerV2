using Blazorcrud.Server.Services;
using Blazorcrud.Shared.Models;
using Bogus;

namespace Blazorcrud.Server.Models
{
    public class DataGenerator
    {
        public static async Task Initialize(AppDbContext appDbContext, FileRepository fileRepository)
        {
            Randomizer.Seed = new Random(32321);
            appDbContext.Database.EnsureDeleted();
            appDbContext.Database.EnsureCreated();
            if (!(appDbContext.Categories.Any()))
            {
                var categories = await fileRepository.ReadFile<List<Category>>("Data/categories.json");
                if (categories != null) await appDbContext.Categories.AddRangeAsync(categories);
            }

            // var parentCategory = new Category
            // {
            //     ExternalIdentifier = 18838809,
            //     Order = 11,
            //     Title = "Car",
            //     PrefixedTitle= "Car",
            //     Colour= "#2196f3",
            //     IsBill = false,
            //     IsTransfer= false,
            //     DateCreated = DateTime.Now,
            // };
            //
            // var category = new Category
            // {
            //     ExternalIdentifier = 3782884,
            //     Order = 14,
            //     ParentCategory = parentCategory,
            //     Title = "Fuel",
            //     PrefixedTitle = "Car |: Fuel",
            //     Colour = "#2196f3",
            //     IsBill = false,
            //     IsTransfer = false,
            //     DateCreated = DateTime.Now,
            // };

            // appDbContext.Categories.Add(parentCategory);
            // appDbContext.Categories.Add(category);
            //
            // var mk = new MerchantKeyword
            // {
            //     Description = "Waitomo",
            //     DateCreated = DateTime.Now,
            // };
            //
            // appDbContext.MerchantKeywords.Add(mk);
            //
            //
            // var mkCat = new MerchantKeywordsCategory()
            // {
            //     Keyword = mk,
            //     Category = category,
            //     DateCreated = DateTime.Now,
            // };
            //
            // appDbContext.MerchantKeywordsCategories.Add(mkCat);

            if (!(appDbContext.Users.Any()))
            {
                var testUsers = new Faker<User>()
                    .RuleFor(u => u.FirstName, u => u.Name.FirstName())
                    .RuleFor(u => u.LastName, u => u.Name.LastName())
                    .RuleFor(u => u.Username, u => u.Internet.UserName())
                    .RuleFor(u => u.Password, u => u.Internet.Password());

                var users = testUsers.Generate(0);

                var customUser = new User()
                {
                    FirstName = "Ewerton",
                    LastName = "Silveira",
                    Username = "ewerton",
                    Password = "Brass45xk"
                };

                users.Add(customUser);

                foreach (var u in users)
                {
                    u.PasswordHash = BCrypt.Net.BCrypt.HashPassword(u.Password);
                    u.Password = "**********";
                    appDbContext.Users.Add(u);
                }

                await appDbContext.SaveChangesAsync();
            }
        }
    }
}




            // if (!(appDbContext.People.Any()))
            // {
            //     //Create test addresses
            //     var testAddresses = new Faker<Address>()
            //         .RuleFor(a => a.Street, f => f.Address.StreetAddress())
            //         .RuleFor(a => a.City, f => f.Address.City())
            //         .RuleFor(a => a.State, f => f.Address.State())
            //         .RuleFor(a => a.ZipCode, f => f.Address.ZipCode());
            //
            //     // Create new people
            //     var testPeople = new Faker<Blazorcrud.Shared.Models.Person>()
            //         .RuleFor(p => p.FirstName, f => f.Name.FirstName())
            //         .RuleFor(p => p.LastName, f => f.Name.LastName())
            //         .RuleFor(p => p.Gender, f => f.PickRandom<Gender>())
            //         .RuleFor(p => p.PhoneNumber, f => f.Phone.PhoneNumber())
            //         .RuleFor(p => p.Addresses, f => testAddresses.Generate(2).ToList());
            //
            //     var people = testPeople.Generate(2);
            //
            //     foreach (Blazorcrud.Shared.Models.Person p in people)
            //     {
            //         appDbContext.People.Add(p);
            //     }
            //
            //     appDbContext.SaveChanges();
            // }
            //
            //
            // if (!(appDbContext.Uploads.Any()))
            // {
            //     // string jsonRecord = @"[{""FirstName"": ""Tim"",""LastName"": ""Bucktooth"",""Gender"": 1,""PhoneNumber"": ""717-211-3211"",
            //     //     ""Addresses"": [{""Street"": ""415 McKee Place"",""City"": ""Pittsburgh"",""State"": ""Pennsylvania"",""ZipCode"": ""15140""
            //     //     },{ ""Street"": ""315 Gunpowder Road"",""City"": ""Mechanicsburg"",""State"": ""Pennsylvania"",""ZipCode"": ""17101""  }]}]";
            //     // var testUploads = new Faker<Upload>()
            //     //     .RuleFor(u => u.FileName, u => u.Lorem.Word()+".json")
            //     //     .RuleFor(u => u.UploadTimestamp, u => u.Date.Past(1, DateTime.Now))
            //     //     .RuleFor(u => u.ProcessedTimestamp, u => u.Date.Future(1, DateTime.Now))
            //     //     .RuleFor(u => u.FileContent, Convert.ToBase64String(System.Text.Encoding.UTF8.GetBytes(jsonRecord)));
            //     // var uploads = testUploads.Generate(3);
            //
            //     // foreach (Upload u in uploads)
            //     // {
            //     //     appDbContext.Uploads.Add(u);
            //     // }
            //     // appDbContext.SaveChanges();
            // }