
using System.Collections.Generic;
using System.IO;
using MemberRegistrationMVP.Models;

namespace MemberRegistrationMVP.Services
{
    /// <summary>
    /// File-based repository.
    /// </summary>
    public class FileMemberRepository : IMemberRepository
    {
        private readonly string _filePath;

        public FileMemberRepository(string filePath)
        {
            _filePath = filePath;
        }

        public List<Member> Load()
        {
            List<Member> members = new List<Member>();
            if (!File.Exists(_filePath))
                return members;

            string[] lines = File.ReadAllLines(_filePath);
            foreach (string line in lines)
            {
                if (string.IsNullOrWhiteSpace(line))
                    continue;

                members.Add(Member.FromFileLine(line));
            }

            return members;
        }

        public void Save(List<Member> members)
        {
            if (members == null)
                members = new List<Member>();

            List<string> lines = new List<string>();
            foreach (Member m in members)
            {
                lines.Add(m.ToFileLine());
            }

            File.WriteAllLines(_filePath, lines);
        }
    }
}
