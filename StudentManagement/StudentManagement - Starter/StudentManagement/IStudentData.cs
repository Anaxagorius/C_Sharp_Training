using System.Collections.Generic;

namespace StudentManagement
{
    /// <summary>
    /// Interface defining persistence operations for student data.
    /// 
    /// Interface = contract: "these methods must exist", but implementation can vary
    /// (file, database, API, etc.).
    /// </summary>
    public interface IStudentData
    {
        List<FirstYearStudent> Load();
        void Save(List<FirstYearStudent> students);
    }
}