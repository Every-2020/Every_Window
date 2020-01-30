using BIND.Core.Member.Model;
using BIND.Core.Member.Service;
using BIND.Core.Member.ViewModel;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BIND.Core.Member
{
    public class MemberData
    {
        public MemberViewModel memberViewModel = new MemberViewModel();

        #region ASYNCHRONOUS METHOD

        /// <summary>
        /// Member 객체 리스트를 설정하는 메소드
        /// </summary>
        /// <returns></returns>
        public async Task SetMemberList()
        {
            await memberViewModel.InitClasses();
            await memberViewModel.SetMember();
        }

        #endregion



        #region GET MEMBER METHOD

        /// <summary>
        /// Member 객체 리스트를 받아오는 메소드
        /// </summary>
        /// <returns>ObservableCollection<Member></returns>
        public ObservableCollection<Model.Member> GetMemberItems()
        {
            return memberViewModel.GetMembers();
        }

        /// <summary>
        /// Student 객체 리스트를 받아오는 메소드
        /// </summary>
        /// <returns>ObservableCollection<Student></returns>
        public ObservableCollection<Student> GetStudentItems()
        {
            return memberViewModel.GetStudents();
        }

        /// <summary>
        /// Teacher 객체 리스트를 받아오는 메소드
        /// </summary>
        /// <returns>ObservableCollection<Teacher></returns>
        public ObservableCollection<Teacher> GetTeacherItems()
        {
            return memberViewModel.GetTeachers();
        }

        /// <summary>
        /// Parent 객체 리스트를 받아오는 메소드
        /// </summary>
        /// <returns>ObservableCollection<Parent></returns>
        public ObservableCollection<Parent> GetParentItems()
        {
            return memberViewModel.GetParents();
        }


        /// <summary>
        /// 멤버 객체 중 자기 자신을 찾는 메소드
        /// </summary>
        /// <returns>Member</returns>
        public Model.Member FindMe(string id)
        {
            return memberViewModel.FindMe(id);
        }

        /// <summary>
        /// ID를 이용하여 멤버 객체를 찾는 메소드, where절로 처음 발견되는 값을 리턴함
        /// </summary>
        /// <param name="id"></param>
        /// <returns>Member</returns>
        public Model.Member GetMemberById(string id)
        {
            return memberViewModel.GetMemberById(id);
        }

        /// <summary>
        /// Index를 이용하여 멤버 객체를 찾는 메소드, where절로 처음 발견되는 값을 리턴함
        /// </summary>
        /// <param name="idx"></param>
        /// <returns>Student</returns>
        public Model.Member GetMemberByIdx(int idx)
        {
            return memberViewModel.GetMemberByIdx(idx);
        }

        /// <summary>
        /// Index를 이용하여 학생 멤버 객체를 찾는 메소드, where절로 처음 발견되는 값을 리턴함
        /// </summary>
        /// <param name="idx"></param>
        /// <returns>Student</returns>
        public Student GetStudentByIdx(int idx)
        {
            return memberViewModel.GetStudentByIdx(idx);
        }

        /// <summary>
        /// Index를 이용하여 교사 멤버 객체를 찾는 메소드, where절로 처음 발견되는 값을 리턴함
        /// </summary>
        /// <param name="idx"></param>
        /// <returns></returns>
        public Teacher GetTeacherByIdx(int idx)
        {
            return memberViewModel.GetTeacherByIdx(idx);
        }

        /// <summary>
        /// Index를 이용하여 학부모 멤버 객체를 찾는 메소드, where절로 처음 발견되는 값을 리턴함
        /// </summary>
        /// <param name="idx"></param>
        /// <returns></returns>
        public Parent GetParentByIdx(int idx)
        {
            return memberViewModel.GetParentByIdx(idx);
        }
        #endregion
    }
}

