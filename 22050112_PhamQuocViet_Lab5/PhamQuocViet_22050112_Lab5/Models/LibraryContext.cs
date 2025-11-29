using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace Lab5.Models
{
    public class LibraryContext : DbContext
    {
        // Constructor - trỏ tới chuỗi kết nối trong Web.config
        public LibraryContext() : base("name=LibraryConnection")
        {
        }

        // Tạo bảng Books trong database
        public DbSet<Book> Books { get; set; }
    }
}