using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System.Data;

namespace Task_1
{
    public static class Processor
    {
        public static List<Category> BuildTreeEF()
        {
            using var db = new AppDbContext();
            var allCategories = db.Categories.AsNoTracking().ToList();

            var lookup = allCategories.ToDictionary(c => c.Id);

            List<Category> roots = new();
            foreach (var cat in allCategories)
            {
                if (cat.ParentId == null)
                {
                    roots.Add(cat);
                }
                else
                {
                    lookup[cat.ParentId.Value].Children.Add(cat);
                }
            }
            return roots;
        }

        public static List<Category> BuildTreeSP()
        {
            List<Category> flatList = new();
            var config = new ConfigurationBuilder().AddJsonFile("appsettings.json", optional: false, reloadOnChange: true).Build();
            string connectionString = config.GetConnectionString("DefaultConnection");

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                SqlCommand cmd = new SqlCommand("GetCategoryTree", conn);
                cmd.CommandType = CommandType.StoredProcedure;

                conn.Open();
                using var reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    flatList.Add(new Category
                    {
                        Id = (int)reader["Id"],
                        ParentId = reader["ParentId"] == DBNull.Value ? null : (int?)reader["ParentId"],
                        CategoryName = reader["CategoryName"].ToString()
                    });
                }
            }

            var lookup = flatList.ToDictionary(c => c.Id);
            List<Category> roots = new();

            foreach (var cat in flatList)
            {
                if (cat.ParentId == null)
                    roots.Add(cat);
                else
                    lookup[cat.ParentId.Value].Children.Add(cat);
            }

            return roots;
        }

        public static void PrintTree(List<Category> categories, List<int> numbering = null)
        {
            numbering ??= new List<int>();

            for (int i = 0; i < categories.Count; i++)
            {
                var currentNumbering = new List<int>(numbering) { i + 1 };

                string numberPrefix = string.Join('.', currentNumbering);

                Console.WriteLine($"{new string(' ', (currentNumbering.Count - 1) * 4)}{numberPrefix}. {categories[i].CategoryName}");

                PrintTree(categories[i].Children, currentNumbering);
            }
        }

        public static Guid Base64UrlToGuid(ReadOnlySpan<char> base64Url)
        {
            if (base64Url.Length != 22)
                throw new FormatException("Base64Url string must be exactly 22 characters.");
            Span<char> base64 = stackalloc char[24];

            for (int i = 0; i < 22; i++)
            {
                char c = base64Url[i];
                base64[i] = c switch
                {
                    '-' => '+',
                    '_' => '/',
                    _ => c
                };
            }
            base64[22] = '=';
            base64[23] = '=';

            Span<byte> bytes = stackalloc byte[16];
            if (!Convert.TryFromBase64Chars(base64, bytes, out int bytesWritten) || bytesWritten != 16)
                throw new FormatException("Invalid base64 string.");

            return new Guid(bytes);
        }

    }
}
