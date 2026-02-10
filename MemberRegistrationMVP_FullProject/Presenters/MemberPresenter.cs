
using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using MemberRegistrationMVP.Models;
using MemberRegistrationMVP.Services;
using MemberRegistrationMVP.Views;

namespace MemberRegistrationMVP.Presenters
{
    /// <summary>
    /// Presenter: contains business logic and workflow.
    /// </summary>
    public class MemberPresenter
    {
        private readonly IMemberView _view;
        private readonly IMemberRepository _repo;

        private List<Member> _members;
        private bool _editEnabled;

        public MemberPresenter(IMemberView view, IMemberRepository repo)
        {
            _view = view;
            _repo = repo;

            _members = new List<Member>();
            _editEnabled = false;

            _view.RegisterClicked += delegate { Register(); };
            _view.UpdateClicked += delegate { Update(); };
            _view.DeleteClicked += delegate { Delete(); };
            _view.SaveClicked += delegate { Save(); };
            _view.LoadClicked += delegate { Load(); };
            _view.SelectionChanged += delegate { OnSelectionChanged(); };

            _view.ShowUpdateDelete(false);
            _view.SetReadOnly(false);
        }

        private void Load()
        {
            try
            {
                _members = _repo.Load();
                _view.DisplayMembers(_members);
                _view.ClearForm();
                _view.ShowUpdateDelete(false);
                _view.SetReadOnly(false);
                _editEnabled = false;
                _view.ShowInfo("Loaded successfully.", "Load");
            }
            catch (Exception ex)
            {
                _view.ShowError(ex.Message, "Load Error");
            }
        }

        private void Save()
        {
            try
            {
                _repo.Save(_members);
                _view.ShowInfo("Saved successfully.", "Save");
            }
            catch (Exception ex)
            {
                _view.ShowError(ex.Message, "Save Error");
            }
        }

        private void Register()
        {
            try
            {
                Member m;
                if (!TryBuildMember(out m))
                    return;

                _members.Add(m);
                _view.DisplayMembers(_members);

                // auto-save for convenience
                _repo.Save(_members);

                _view.ShowInfo("Member registered and saved.", "Success");
                _view.ClearForm();
                _view.ShowUpdateDelete(false);
                _view.SetReadOnly(false);
                _editEnabled = false;
            }
            catch (Exception ex)
            {
                _view.ShowError(ex.Message, "Register Error");
            }
        }

        private void OnSelectionChanged()
        {
            int index = _view.SelectedIndex;
            if (index < 0 || index >= _members.Count)
            {
                _view.ShowUpdateDelete(false);
                _view.SetReadOnly(false);
                _editEnabled = false;
                return;
            }

            Member selected = _members[index];

            _view.FirstNameText = selected.FirstName;
            _view.LastNameText = selected.LastName;
            _view.PostalCodeText = selected.PostalCode;
            _view.GenderValue = selected.Gender;
            _view.MemberTypeValue = selected.MemberType;
            _view.MemberSinceValue = selected.MemberSince;

            _view.SetReadOnly(true);
            _view.ShowUpdateDelete(true);
            _editEnabled = false;

            bool wantsEdit = _view.Confirm("Do you want to update this record?", "Update?");
            if (wantsEdit)
            {
                _view.SetReadOnly(false);
                _editEnabled = true;
            }
            else
            {
                _view.SetReadOnly(true);
                _editEnabled = false;
            }
        }

        private void Update()
        {
            int index = _view.SelectedIndex;
            if (index < 0 || index >= _members.Count)
            {
                _view.ShowInfo("Select a record first.", "Info");
                return;
            }

            if (!_editEnabled)
            {
                _view.ShowInfo("Editing is disabled. Select a record and click 'Yes' on the update prompt.", "Info");
                return;
            }

            try
            {
                Member updated;
                if (!TryBuildMember(out updated))
                    return;

                _members[index] = updated;
                _view.DisplayMembers(_members);
                _repo.Save(_members);

                _view.ShowInfo("Updated and saved successfully.", "Updated");
                _view.SetReadOnly(true);
                _editEnabled = false;
            }
            catch (Exception ex)
            {
                _view.ShowError(ex.Message, "Update Error");
            }
        }

        private void Delete()
        {
            int index = _view.SelectedIndex;
            if (index < 0 || index >= _members.Count)
            {
                _view.ShowInfo("Select a record first.", "Info");
                return;
            }

            bool confirm = _view.Confirm("Are you sure you want to delete this record?", "Confirm Delete");
            if (!confirm)
                return;

            try
            {
                _members.RemoveAt(index);
                _view.DisplayMembers(_members);
                _repo.Save(_members);

                _view.ShowInfo("Deleted successfully.", "Deleted");
                _view.ClearForm();
                _view.ShowUpdateDelete(false);
                _view.SetReadOnly(false);
                _editEnabled = false;
            }
            catch (Exception ex)
            {
                _view.ShowError(ex.Message, "Delete Error");
            }
        }

        private bool TryBuildMember(out Member member)
        {
            member = new Member();

            member.FirstName = (_view.FirstNameText ?? "").Trim();
            member.LastName = (_view.LastNameText ?? "").Trim();
            member.PostalCode = (_view.PostalCodeText ?? "").Trim();
            member.Gender = _view.GenderValue;
            member.MemberType = _view.MemberTypeValue;
            member.MemberSince = _view.MemberSinceValue;

            if (string.IsNullOrWhiteSpace(member.FirstName))
            {
                _view.ShowError("First Name is required.", "Validation");
                return false;
            }

            if (string.IsNullOrWhiteSpace(member.LastName))
            {
                _view.ShowError("Last Name is required.", "Validation");
                return false;
            }

            if (string.IsNullOrWhiteSpace(member.PostalCode))
            {
                _view.ShowError("Postal Code is required.", "Validation");
                return false;
            }

            // Basic Canadian postal code pattern
            Regex rx = new Regex(@"^[A-Za-z]\d[A-Za-z]\s?\d[A-Za-z]\d$");
            if (!rx.IsMatch(member.PostalCode))
            {
                _view.ShowError("Postal Code format should look like A1A 1A1.", "Validation");
                return false;
            }

            if (string.IsNullOrWhiteSpace(member.MemberType))
            {
                _view.ShowError("Member Type is required.", "Validation");
                return false;
            }

            return true;
        }
    }
}
