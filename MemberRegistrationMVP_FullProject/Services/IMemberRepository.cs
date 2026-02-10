
using System.Collections.Generic;
using MemberRegistrationMVP.Models;

namespace MemberRegistrationMVP.Services
{
    public interface IMemberRepository
    {
        List<Member> Load();
        void Save(List<Member> members);
    }
}
