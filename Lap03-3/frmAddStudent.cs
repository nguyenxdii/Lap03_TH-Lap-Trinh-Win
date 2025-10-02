using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Lap03_3
{
    public delegate void AddStudentDelegate(string studentID, string fullName, double averageScore, string faculty);

    public partial class frmAddStudent : Form
    {
        public AddStudentDelegate OnAddStudent;

        public Func<string, bool> CheckStudentIdExists; // delegate kiểm tra trùng MSSV

        public frmAddStudent()
        {
            InitializeComponent();
        }

        private void frmAddStudent_Load(object sender, EventArgs e)
        {

        }
        private void btnAddStudent_Click(object sender, EventArgs e)
        {
            try
            {
                string studentID = txtStudentID.Text.Trim();
                string fullName = txtFullName.Text.Trim();
                string scoreTextRaw = txtAverageScore.Text.Trim();
                string faculty = cmbFaculty.SelectedItem?.ToString(); // Lấy tên khoa đã chọn tránh null

                // Bắt buộc nhập
                //studentID.selectAll() -> boi den
                //studentID.focus() -> con tro o do
                if (string.IsNullOrWhiteSpace(studentID) || string.IsNullOrWhiteSpace(fullName) || string.IsNullOrWhiteSpace(scoreTextRaw))
                {
                    MessageBox.Show("Vui lòng nhập đầy đủ: Mã số, Tên sinh viên, Điểm.", "Thiếu dữ liệu", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                // Trùng MSSV so với grid ở form chính
                if (CheckStudentIdExists != null && CheckStudentIdExists(studentID))
                {
                    MessageBox.Show("Mã số sinh viên đã tồn tại trong danh sách.", "Trùng MSSV", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                // Điểm hợp lệ 0-10
                //scoreTextRaw.replace(',', '.')
                if (!double.TryParse(scoreTextRaw, NumberStyles.Any, CultureInfo.InvariantCulture, out double score))
                {
                    MessageBox.Show("Điểm không hợp lệ.", "Lỗi nhập liệu", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
                if (score < 0 || score > 10)
                {
                    MessageBox.Show("Điểm phải nằm trong khoảng 0 đến 10.", "Lỗi nhập liệu", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                if (string.IsNullOrWhiteSpace(faculty))
                {
                    MessageBox.Show("Vui lòng chọn Khoa.", "Thiếu dữ liệu", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    cmbFaculty.DroppedDown = true;
                    return;
                }

                // Gọi delegate với điểm đã chuẩn hóa
                //string normalizedScore = score.ToString("0.##");
                //string normalizedScore = score.ToString("0.##", CultureInfo.InvariantCulture);
                OnAddStudent?.Invoke(studentID, fullName, score, faculty);

                this.DialogResult = DialogResult.OK;
                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnCancer_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
