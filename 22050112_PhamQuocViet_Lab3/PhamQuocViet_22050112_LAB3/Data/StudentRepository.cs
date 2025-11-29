using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Web.Hosting;
using YourNamespace.Models;

namespace YourNamespace.Data
{
    public static class StudentRepository
    {
        private static readonly string FilePath = HostingEnvironment.MapPath("~/App_Data/students.txt");
        private static readonly object FileLock = new object();

        private static void EnsureFileExists()
        {
            if (FilePath == null)
                throw new InvalidOperationException("Không tìm thấy App_Data path.");

            var dir = Path.GetDirectoryName(FilePath);
            if (!Directory.Exists(dir))
                Directory.CreateDirectory(dir);

            if (!File.Exists(FilePath))
            {
                // tạo file rỗng
                File.WriteAllText(FilePath, string.Empty, Encoding.UTF8);
            }
        }

        public static List<Student> GetAll()
        {
            EnsureFileExists();
            lock (FileLock)
            {
                var lines = File.ReadAllLines(FilePath, Encoding.UTF8)
                                .Where(l => !string.IsNullOrWhiteSpace(l))
                                .ToArray();

                var list = new List<Student>();
                foreach (var line in lines)
                {
                    var parts = line.Split(new[] { '|' }, StringSplitOptions.None);
                    if (parts.Length < 7) continue;

                    DateTime dt;
                    DateTime? dob = DateTime.TryParseExact(parts[3], "yyyy-MM-dd", CultureInfo.InvariantCulture, DateTimeStyles.None, out dt) ? dt : (DateTime?)null;

                    decimal fee = 0m;
                    decimal.TryParse(parts[4], NumberStyles.Any, CultureInfo.InvariantCulture, out fee);

                    list.Add(new Student
                    {
                        StudentId = parts[0],
                        FullName = parts[1],
                        Gender = parts[2],
                        DateOfBirth = dob,
                        TuitionFee = fee,
                        ImageFileName = parts[5],
                        Notes = parts[6]
                    });
                }
                return list;
            }
        }

        public static Student GetById(string id)
        {
            if (string.IsNullOrEmpty(id)) return null;
            return GetAll().FirstOrDefault(s => string.Equals(s.StudentId, id, StringComparison.OrdinalIgnoreCase));
        }

        // Tạo mã tiếp theo S001, S002 ...
        public static string GenerateNextStudentId()
        {
            EnsureFileExists();
            lock (FileLock)
            {
                var list = GetAll();
                var max = list.Select(s =>
                {
                    var digits = new string((s.StudentId ?? "").Where(char.IsDigit).ToArray());
                    return int.TryParse(digits, out var n) ? n : 0;
                }).DefaultIfEmpty(0).Max();
                int next = max + 1;
                return $"S{next.ToString("D3")}";
            }
        }

        public static void Insert(Student s)
        {
            if (s == null) throw new ArgumentNullException(nameof(s));
            EnsureFileExists();
            lock (FileLock)
            {
                var list = GetAll();
                if (string.IsNullOrWhiteSpace(s.StudentId))
                    s.StudentId = GenerateNextStudentId();

                if (list.Any(x => string.Equals(x.StudentId, s.StudentId, StringComparison.OrdinalIgnoreCase)))
                    throw new InvalidOperationException("StudentId đã tồn tại.");

                var line = ToLine(s);
                File.AppendAllLines(FilePath, new[] { line }, Encoding.UTF8);
            }
        }

        public static void Update(Student s)
        {
            if (s == null) throw new ArgumentNullException(nameof(s));
            EnsureFileExists();
            lock (FileLock)
            {
                var lines = File.ReadAllLines(FilePath, Encoding.UTF8).ToList();
                bool replaced = false;
                for (int i = 0; i < lines.Count; i++)
                {
                    var parts = lines[i].Split(new[] { '|' }, StringSplitOptions.None);
                    if (parts.Length > 0 && string.Equals(parts[0], s.StudentId, StringComparison.OrdinalIgnoreCase))
                    {
                        lines[i] = ToLine(s);
                        replaced = true;
                        break;
                    }
                }

                if (!replaced)
                    throw new InvalidOperationException("Không tìm thấy học viên cần cập nhật.");

                File.WriteAllLines(FilePath, lines, Encoding.UTF8);
            }
        }

        public static void Delete(string id)
        {
            if (string.IsNullOrEmpty(id)) throw new ArgumentNullException(nameof(id));
            EnsureFileExists();
            lock (FileLock)
            {
                var lines = File.ReadAllLines(FilePath, Encoding.UTF8).ToList();
                var newLines = lines.Where(l =>
                {
                    var parts = l.Split(new[] { '|' }, StringSplitOptions.None);
                    return !(parts.Length > 0 && string.Equals(parts[0], id, StringComparison.OrdinalIgnoreCase));
                }).ToList();

                File.WriteAllLines(FilePath, newLines, Encoding.UTF8);
            }
        }

        private static string ToLine(Student s)
        {
            // đảm bảo không chứa ký tự '|' trong dữ liệu - bạn có thể replace nếu cần
            string notesSafe = (s.Notes ?? "").Replace("|", " ");
            string nameSafe = (s.FullName ?? "").Replace("|", " ");
            string imageSafe = s.ImageFileName ?? "";
            string dob = s.DateOfBirth.HasValue ? s.DateOfBirth.Value.ToString("yyyy-MM-dd") : "";
            return string.Join("|", new[]
            {
                s.StudentId,
                nameSafe,
                s.Gender ?? "",
                dob,
                s.TuitionFee.ToString(CultureInfo.InvariantCulture),
                imageSafe,
                notesSafe
            });
        }
    }
}
