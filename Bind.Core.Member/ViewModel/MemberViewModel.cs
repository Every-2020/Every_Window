using BIND.Core.Member.Model;
using BIND.Core.Member.Service;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Windows.Data;
using TNetwork.Common;
using TNetwork.Data;

namespace BIND.Core.Member.ViewModel
{
    public class MemberViewModel : INotifyPropertyChanged
    {
 #region Properties
        private readonly object _lock = new object();
        private MemberService memberService = new MemberService();

        private bool IsDataLoaded { get; set; }

        public ObservableCollection<Model.Member> Items { get; } = new ObservableCollection<Model.Member>();
        public ObservableCollection<Student> StudentItems { get; } = new ObservableCollection<Student>();
        public ObservableCollection<Teacher> TeacherItems { get; } = new ObservableCollection<Teacher>();
        public ObservableCollection<Parent> ParentItems { get; } = new ObservableCollection<Parent>();

        public List<Department> Departments { get; set; }
        public List<ClassRoom> ClassRooms { get; set; }
        #endregion


        public MemberViewModel()
        {
            // You must lock the collection when modifying it from another thread.
            BindingOperations.EnableCollectionSynchronization(Items, _lock);
        }

        #region ADD METHOD

        internal void AddMember(Model.Member member)
        {
            lock (_lock)
            {
                // once locked, you can manipulate the collection safely from another thread
                Items.Add((Model.Member)member.Clone());
            }
        }

        internal void AddMembers(List<Model.Member> members)
        {
            // TODO : 내부 디비 연동
            foreach (var member in members)
            {
                AddMember(member);
            }
        }

        internal void AddStudent(Student student)
        {
            lock (_lock)
            {
                // Once locked, you can manipulate the collection safely from another thread
                StudentItems.Add((Student)student.Clone());
            }
        }
        internal void AddParent(Parent parent)
        {
            lock (_lock)
            {
                // Once locked, you can manipulate the collection safely from another thread
                ParentItems.Add((Parent)parent.Clone());
            }
        }

        internal void AddTeacher(Teacher teacher)
        {
            lock (_lock)
            {
                // Once locked, you can manipulate the collection safely from another thread
                TeacherItems.Add((Teacher)teacher.Clone());
            }
        }

        internal void AddUsers(MemberResponse data)
        {
            foreach (Parent parent in data.Parents)
            {
                AddMember(parent);
                AddParent(parent);
            }
            foreach (Student student in data.Students)
            {
                SetStudentClassroom(student);
                SetParentInfo(student);
                AddMember(student);
                AddStudent(student);
            }
            foreach (Teacher teacher in data.Teachers)
            {
                AddMember(teacher);
                AddTeacher(teacher);
            }
        }
        #endregion

        #region SET METHOD

        internal async Task SetMember()
        {
            var memberResp = await memberService.GetUsers();
            if (memberResp != null && memberResp.Status == 200)
            {
                if (memberResp.Data != null)
                {
                    AddUsers(memberResp.Data);
                }
            }
        }

        public async Task InitClasses()
        {
            var classResp = await memberService.GetClasses();
            SetClassRooms(classResp?.Data.ClassRooms);
        }

        public void SetDeaprtments(List<Department> ResponseDepartments)
        {
            Departments = ResponseDepartments;
        }

        public void SetClassRooms(List<ClassRoom> ResponseClasses)
        {
            ClassRooms = ResponseClasses;
        }


        internal void SetStudentClassroom(Student student)
        {
            if (ClassRooms == null)
                return;

            ClassRoom item = ClassRooms.FirstOrDefault(x => x.Idx == student.ClassIdx);

            if (item != null)
            {
                student.Classroom = item;
            }
        }

        internal void SetParentInfo(Student student)
        {
            foreach (ParentLog item in student.ParentsLogs)
            {
                Parent parentInfo = ParentItems.FirstOrDefault(x => x.Idx == item.ParentIdx);
                if (parentInfo != null)
                {
                    item.Name = parentInfo.Name;
                    item.PhoneNum = parentInfo.Mobile;
                    item.Image = parentInfo.ProfileImage;
                }
            }

        }

        public async Task<TResponse<Nothing>> UpdateMemberAsync(Model.Member member)
        {
            return await memberService.UpdateMember(member);
        }

        #endregion

        #region GET MEMBER METHOD

        internal ObservableCollection<Model.Member> GetMembers()
        {
            return Items;
        }

        internal ObservableCollection<Student> GetStudents()
        {
            return StudentItems;
        }

        internal ObservableCollection<Teacher> GetTeachers()
        {
            return TeacherItems;
        }

        internal ObservableCollection<Parent> GetParents()
        {
            return ParentItems;
        }

        internal Model.Member FindMe(string id)
        {
            if(Items == null || Items.Count <= 0)
            {
                return null;
            } 
            Model.Member member = Items.FirstOrDefault(x => x.Id == id);

            if(member != null)
            {
                member.IsMe = true;

                switch(member.Auth)
                {
                    case AuthType.STUDENT:
                        Student student = StudentItems.Single(x => x.Id == id);
                        student.IsMe = true;
                        break;

                    case AuthType.TEACHER:
                        Teacher teacher = TeacherItems.Single(x => x.Id == id);
                        teacher.IsMe = true;
                        break;

                    case AuthType.PARENT:
                        Parent parent = ParentItems.Single(x => x.Id == id);
                        parent.IsMe = true;
                        break;
                }

                return member;
            }

            return new Model.Member();
        }

        internal Model.Member GetMemberById(string id)
        {
            if (Items == null || Items.Count <= 0) return null;
            return Items.Where(x => x.Id == id).FirstOrDefault();
        }

        internal Model.Member GetMemberByIdx(int idx)
        {
            if (Items == null || Items.Count <= 0) return null;
            // Debug.WriteLine($"Member Idx = {idx}");
            return Items.Where(x => x.Idx == idx).FirstOrDefault();
        }

        internal Student GetStudentByIdx(int idx)
        {
            if (StudentItems == null || StudentItems.Count <= 0) return null;
            return StudentItems.Where(x => x.Idx == idx).FirstOrDefault();
        }

        internal Teacher GetTeacherByIdx(int idx)
        {
            if (TeacherItems == null || TeacherItems.Count <= 0) return null;
            return TeacherItems.Where(x => x.Idx == idx).FirstOrDefault();
        }

        internal Parent GetParentByIdx(int idx)
        {
            if (ParentItems == null || ParentItems.Count <= 0) return null;
            return ParentItems.Where(x => x.Idx == idx).FirstOrDefault();
        }

        #endregion

        #region GET OTHERS METHOD

        internal List<Model.Member> GetReceiver(List<Receiver> receivers)
        {
            List<Model.Member> lsMember = new List<Model.Member>();

            foreach (Receiver receiver in receivers)
            {
                lsMember.Add(GetMemberById(receiver.Id));
            }

            return lsMember;
        }

        internal ClassRoom GetClassByClassIdx(int classIdx)
        {
            return ClassRooms.Single(x => x.Idx == classIdx);
        }

        #endregion

        internal void ChangeStatus(Model.Member member, int status)
        {
            Model.Member memberItem = Items.Where(x => x.Id == member.Id).FirstOrDefault();
            memberItem.Status = status;
        }

        public async void ChangeProfileImage(Dictionary<string, object> dicPost, Model.Member member, string path, Guid guid)
        {
            var response = await memberService.UploadFile(dicPost, "image");
            if (response.StatusCode == HttpStatusCode.OK)
            {
                var fileName = guid.ToString().Replace("-", string.Empty) + System.IO.Path.GetExtension(path);
                //Dispatcher.Invoke(() =>
                member.ProfileImage = Options.serverUrl + "/" + MimeMapping.GetMimeMapping(path) + "/" + fileName;
                Debug.WriteLine("이미지 올라감 : " + member.ProfileImage);

                var updateResp = await memberService.UpdateMember(member);
                if (updateResp.Status == 200)
                {
                    Debug.WriteLine("프로필 업데이트 됨 : " + member.ProfileImage);
                }
                else
                {
                    Debug.WriteLine("프로필 업데이트 실패함 : " + updateResp.Message);
                    // TODO : 실패 메시지 박스 띄워주기
                }
            }
            else
            {
                Debug.WriteLine("이미지 업로드실패");
                // TODO : 실패 메시지 박스 띄워주기
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        private void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
