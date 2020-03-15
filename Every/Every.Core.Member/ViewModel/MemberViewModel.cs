using Every.Core.Member.Model;
using Every.Core.Member.Service;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Every.Core.Member.ViewModel
{
    public class MemberViewModel : BindableBase
    {
        MemberService memberService = new MemberService();

        #region Properties
        // 학생 IDX로 회원조회 정보 저장
        private ObservableCollection<MemberInformation> _studentMemberInfoItems = new ObservableCollection<MemberInformation>();
        public ObservableCollection<MemberInformation> StudentMemberInfoItems
        {
            get => _studentMemberInfoItems;
            set
            {
                SetProperty(ref _studentMemberInfoItems, value);
            }
        }
        #endregion

        public async Task GetStudentMemberInfo(int idx)
        {
            if (StudentMemberInfoItems != null)
            {
                StudentMemberInfoItems.Clear();
            }

            var resp = await memberService.GetStudentMemberInformation(idx);

            if (resp != null && resp.Data != null && resp.Status == 200)
            {
                try
                {
                    MemberInformation memberInfo = new MemberInformation();

                    memberInfo.Idx = resp.Data.MemberInformations.Idx;
                    memberInfo.Email = resp.Data.MemberInformations.Email;
                    memberInfo.Name = resp.Data.MemberInformations.Name;
                    memberInfo.Phone = resp.Data.MemberInformations.Phone;
                    memberInfo.Birth_Year = resp.Data.MemberInformations.Birth_Year;
                    memberInfo.School_Id = resp.Data.MemberInformations.School_Id;

                    StudentMemberInfoItems.Add((MemberInformation)memberInfo.Clone());
                }
                catch (Exception e)
                {
                    Debug.WriteLine(e.StackTrace);
                }
            }
        }
    }
}
