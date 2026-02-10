
using System;

namespace MemberRegistrationMVP.Models
{
    /// <summary>
    /// Model: one member record.
    /// </summary>
    public class Member
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string PostalCode { get; set; }
        public string Gender { get; set; }
        public string MemberType { get; set; }
        public DateTime MemberSince { get; set; }

        public Member()
        {
            FirstName = "";
            LastName = "";
            PostalCode = "";
            Gender = "Male";
            MemberType = "Standard";
            MemberSince = DateTime.Today;
        }

        public override string ToString()
        {
            return string.Format("{0} {1} | {2} | {3} | {4} | Since: {5:yyyy-MM-dd}",
                FirstName, LastName, PostalCode, Gender, MemberType, MemberSince);
        }

        public string ToFileLine()
        {
            return string.Format("{0},{1},{2},{3},{4},{5}",
                Escape(FirstName), Escape(LastName), Escape(PostalCode), Escape(Gender), Escape(MemberType), MemberSince.ToString("O"));
        }

        public static Member FromFileLine(string line)
        {
            // Simple CSV split (fields are not expected to contain commas in this assignment)
            string[] parts = line.Split(',');
            if (parts.Length != 6)
                throw new FormatException("Invalid record format.");

            Member m = new Member();
            m.FirstName = Unescape(parts[0].Trim());
            m.LastName = Unescape(parts[1].Trim());
            m.PostalCode = Unescape(parts[2].Trim());
            m.Gender = Unescape(parts[3].Trim());
            m.MemberType = Unescape(parts[4].Trim());
            m.MemberSince = DateTime.Parse(parts[5].Trim());
            return m;
        }

        private static string Escape(string value)
        {
            return value == null ? "" : value.Replace("\n", " ").Replace("\r", " ");
        }

        private static string Unescape(string value)
        {
            return value == null ? "" : value;
        }
    }
}
