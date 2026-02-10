
using System;
using System.Collections.Generic;
using MemberRegistrationMVP.Models;

namespace MemberRegistrationMVP.Views
{
    /// <summary>
    /// View contract for MVP.
    /// </summary>
    public interface IMemberView
    {
        event EventHandler RegisterClicked;
        event EventHandler UpdateClicked;
        event EventHandler DeleteClicked;
        event EventHandler SaveClicked;
        event EventHandler LoadClicked;
        event EventHandler SelectionChanged;

        string FirstNameText { get; set; }
        string LastNameText { get; set; }
        string PostalCodeText { get; set; }
        string GenderValue { get; set; }
        string MemberTypeValue { get; set; }
        DateTime MemberSinceValue { get; set; }

        int SelectedIndex { get; }

        void SetReadOnly(bool readOnly);
        void ShowUpdateDelete(bool visible);
        void DisplayMembers(List<Member> members);
        void ClearForm();

        bool Confirm(string message, string title);
        void ShowInfo(string message, string title);
        void ShowError(string message, string title);
    }
}
