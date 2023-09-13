using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Windows.Forms;


namespace StudentDiary
{
    public partial class AddEditStudent : Form
    {
        private int _studentId;
        private Student _student;
        private FileHelper<List<Student>> _fileHelper = new FileHelper<List<Student>>(Program.FilePath);
        public AddEditStudent(int id = 0)
        {
            InitializeComponent();
            foreach (var item in Program.groups)
                cbGroup.Items.Add(item);
            
            _studentId = id;
            GetStudentData();
            
            tbFirstName.Select();
        }
        
        private void GetStudentData()
        {
            if (_studentId != 0)
            {
                Text = "Edytowanie danych ucznia";
                var students = _fileHelper.DeserializeFromFile();
                _student = students.FirstOrDefault(x => x.Id == _studentId);
                if (_student == null)
                    throw new Exception("Brak użytkownika o podanym Id.");
                FillTextBoxes();

            }
        }
        private void FillTextBoxes()
        {
            tbId.Text = _student.Id.ToString();
            tbFirstName.Text = _student.FirstName;
            tbLastName.Text = _student.LastName;
            tbMath.Text = _student.Match;
            tbTechnology.Text = _student.Technology;
            tbPhysics.Text = _student.Physics;
            tbPolishLang.Text = _student.PolishLang;
            tbForeginLang.Text = _student.ForeginLang;
            rtbComments.Text = _student.Comments;
            cbExtraActivities.Checked = _student.ExtraActivities;
            cbGroup.SelectedIndex = _student.GroupId;
        }
        private void btnConfirm_Click(object sender, EventArgs e)
        {
            var students = _fileHelper.DeserializeFromFile();

            if (_studentId != 0)
                students.RemoveAll(x => x.Id == _studentId);
            else
                AssignIdToNewStudent(students);
            AddNewUserToList(students);
            _fileHelper.SerializeToFile(students);
            Close();

        }
        
        private void AddNewUserToList(List<Student> students)
        {
            var student = new Student
            {
                Id = _studentId,
                FirstName = tbFirstName.Text,
                LastName = tbLastName.Text,
                Match = tbMath.Text,
                Physics = tbPhysics.Text,
                Technology = tbTechnology.Text,
                PolishLang = tbPolishLang.Text,
                ForeginLang = tbForeginLang.Text,
                Comments = rtbComments.Text,
                ExtraActivities = cbExtraActivities.Checked,
                GroupId = cbGroup.SelectedIndex
            };
            students.Add(student);
                
        }
        private void AssignIdToNewStudent(List<Student> students)
        {
            var studentWithHighestId = students.OrderByDescending(x => x.Id).FirstOrDefault();
            //var studentId = 0;
            if (studentWithHighestId == null)
                _studentId = 1;
            else
                _studentId = studentWithHighestId.Id + 1;
        }
        private void btnCancel_Click(object sender, EventArgs e)
        {
            Close();
        }
    }
}
