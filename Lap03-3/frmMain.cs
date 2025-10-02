using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Lap03_3
{
    public partial class frmMain : Form
    {
        private readonly List<Student> students = new List<Student>();

        public frmMain()
        {
            InitializeComponent();
        }

        private void frmMain_Load(object sender, EventArgs e)
        {
            dgvStudents.AutoGenerateColumns = false;
            dgvStudents.AllowUserToAddRows = false;
            dgvStudents.ReadOnly = true;
            dgvStudents.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            // khởi tạo lưới trống
            RefreshGrid(students);
        }

        private void tsbAddNew_Click(object sender, EventArgs e)
        {
            try
            {
                frmAddStudent addStudentFrm = new frmAddStudent();

                // Gán delegate -> trỏ về hàm AddStudent
                addStudentFrm.OnAddStudent = new AddStudentDelegate(AddStudent);

                // Gán hàm kiểm tra trùng MSSV (cho form con)
                addStudentFrm.CheckStudentIdExists = delegate (string id)
                {
                    return students.Any(s => string.Equals(s.StudentID, id, StringComparison.OrdinalIgnoreCase));
                };

                // Show form con
                addStudentFrm.ShowDialog();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // Hàm nhận dữ liệu từ form con
        private void AddStudent(string studentID, string fullName, double averageScore, string faculty)
        {
            try
            {
                bool exists = students.Any(s => string.Equals(s.StudentID, studentID, StringComparison.OrdinalIgnoreCase));
                if (exists)
                {
                    MessageBox.Show("Mã số sinh viên đã tồn tại.", "Trùng MSSV",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                // Tạo đối tượng Student mới
                Student stu = new Student();
                stu.Number = students.Count + 1; // STT tự tăng
                stu.StudentID = studentID;
                stu.FullName = fullName;
                //stu.AverageScore = double.Parse(averageScore); // đã validate ở form con
                stu.AverageScore = averageScore;
                stu.Faculty = faculty;

                // Thêm vào list
                students.Add(stu);

                // Refresh theo trạng thái filter hiện tại (để search không bị reset)
                ApplySearchFilter();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void txtSearch_TextChanged(object sender, EventArgs e) => ApplySearchFilter();

        private void ApplySearchFilter()
        {
            string kw = txtSearch.Text == null ? string.Empty : txtSearch.Text.Trim();

            if (string.IsNullOrEmpty(kw))
            {
                RefreshGrid(students);
                return;
            }

            // Lọc không phân biệt hoa/thường
            List<Student> filtered = students
                .Where(s => (s.FullName ?? string.Empty)
                .IndexOf(kw, StringComparison.OrdinalIgnoreCase) >= 0)
                .ToList();

            RefreshGrid(filtered);
        }

        private void RefreshGrid(List<Student> source)
        {
            dgvStudents.DataSource = null;
            dgvStudents.DataSource = source;
            dgvStudents.Refresh();
        }

        private void mniAddNew_Click(object sender, EventArgs e) => tsbAddNew_Click(sender, e);
    }
}
