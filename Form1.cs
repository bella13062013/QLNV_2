using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using System.Configuration;

namespace QLNV1_2lop
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

       
        #region Phương thức
        public void loadPB()
        {
            string sql = "select * from PhongBan";

            cboPhongBan.DataSource = KetNoi.getData(sql);
            cboPhongBan.DisplayMember = "TenPB";
            cboPhongBan.ValueMember = "MaPB";

        }
        public void loadData()
        {
            string sql = "select * from NhanVien";
            data.DataSource=KetNoi.getData(sql);
        }
        #endregion
        private void Form1_Load(object sender, EventArgs e)
        {
            KetNoi.moKetNoi();
            loadPB();
            loadData();
            KetNoi.dongKetNoi();
        }

        private void btnChonAnh_Click(object sender, EventArgs e)
        {
            OpenFileDialog oFile = new OpenFileDialog();
            if (oFile.ShowDialog()==DialogResult.OK)
            {
                pictureBox1.Image = Image.FromFile(oFile.FileName);
                lblanh.Text = Path.GetFileName(oFile.FileName);
            }    
        }

        private void btnThem_Click(object sender, EventArgs e)
        {
            string sql = "Insert into NhanVien values(@MaNV,@TenNV ,@ngaysinh,@gioiTinh,@sodt,@maPB,@picture)";
            string[] name = {"@MaNV", "@TenNV", "@ngaysinh", "@gioiTinh", "@sodt", "@maPB", "@picture" };

            bool gt = rdNam.Checked == true ? true : false;

            object[] value = { txtMaNV.Text, txtTenNV.Text, dtpNgaySinh.Value, gt, txtSDT.Text, cboPhongBan.SelectedValue, lblanh.Text };

            KetNoi.moKetNoi();
            KetNoi.updateData(sql, value, name, 7);
            loadData();
            KetNoi.dongKetNoi();

        }

        private void btnXoa_Click(object sender, EventArgs e)
        {
            int i = data.CurrentCell.RowIndex;
            if (i>=0)
            {
                DialogResult dr = MessageBox.Show("Bạn có muốn xóa không?", "Thông báo", MessageBoxButtons.OKCancel);
                if (dr==DialogResult.OK)
                {
                    string ma = data.Rows[i].Cells[0].Value.ToString();
                    string sql =string.Format("delete from NhanVien where maNV ='{0}'",ma);
                    object[] value = { };
                    string[] name = { };

                    KetNoi.moKetNoi();
                    KetNoi.updateData(sql, value, name, 0);
                    loadData();
                    KetNoi.dongKetNoi();
                }    
            }    
        }

        private void data_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            int i = data.CurrentCell.RowIndex;
            txtMaNV.Text = data.Rows[i].Cells[0].Value.ToString();
            txtTenNV.Text = data.Rows[i].Cells[1].Value.ToString();
            dtpNgaySinh.Text = data.Rows[i].Cells[2].Value.ToString();

            string gt = data.Rows[i].Cells[3].Value.ToString();
            if (gt == "True")
            {
                rdNam.Checked = true;
            }
            else
                rdNu.Checked = true;
            txtSDT.Text = data.Rows[i].Cells[4].Value.ToString();
            cboPhongBan.SelectedValue = data.Rows[i].Cells[5].Value.ToString();

            lblanh.Text = data.Rows[i].Cells[6].Value.ToString();

            string pathAnh = ConfigurationManager.AppSettings.Get("duongdananh") + "\\" + lblanh.Text;
            if (File.Exists(pathAnh))
            {
                pictureBox1.Image = Image.FromFile(pathAnh);
            }
            else
                pictureBox1.Image = null;
        }

        private void btnSua_Click(object sender, EventArgs e)
        {
            string sql = string.Format("Update NhanVien set MaNV = @MaNV,tenNV =@TenNV ,ngaysinh=@ngaysinh,gioitinh = @gioiTinh,soDT =@sodt,maPB =@maPB,picture = @picture where MaNV ='{0}'",txtMaNV.Text);
            
            string[] name = { "@MaNV", "@TenNV", "@ngaysinh", "@gioiTinh", "@sodt", "@maPB", "@picture" };

            bool gt = rdNam.Checked == true ? true : false;

            object[] value = { txtMaNV.Text, txtTenNV.Text, dtpNgaySinh.Value, gt, txtSDT.Text, cboPhongBan.SelectedValue, lblanh.Text };

            KetNoi.moKetNoi();
            KetNoi.updateData(sql, value, name, 7);
            loadData();
            KetNoi.dongKetNoi();
        }
    }
}
