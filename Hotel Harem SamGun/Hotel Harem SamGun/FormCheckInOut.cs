﻿using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Hotel_Harem_SamGun
{
    public partial class FormCheckInOut : Form
    {
        private DataTable dtList;
        private List<string> columnName;
        public string fontName = "Gill Sans MT";

        public FormCheckInOut()
        {
            InitializeComponent();

            columnName = new List<string>();

            foreach (Control control in this.Controls)
            {
                // Console.WriteLine(control.GetType());

                control.Font = new Font(fontName, control.Font.Size + 4, control.Font.Style, GraphicsUnit.Pixel, ((byte)(0)));
            }
        }

        private void FormCheckInOut_Load(object sender, EventArgs e)
        {
            radioButton1.Checked = true;
            radioButton2.Checked = false;

            checkBox1.Checked = true;
            checkBox1.Enabled = false;
            checkBox1.Visible = false;

            columnName.Add("Kode Reservasi");
            columnName.Add("Nama Tamu");
            columnName.Add("Nomor Kamar");
            columnName.Add("Jenis Kamar");
            columnName.Add("Jumlah Penghuni Kamar");
            columnName.Add("Tanggal Check In");
            columnName.Add("Tanggal Check Out");
            columnName.Add("Jadwal Check In");
            columnName.Add("Jadwal Check Out");
            columnName.Add("Lama Menginap");
            columnName.Add("Subtotal Reservasi");
            columnName.Add("Status Reservasi");
            columnName.Add("ID");
            columnName.Add("Kode Kamar");

            dataGridViewSetup();
        }

        private DataGridView UpdateDataGridViewFont(DataGridView dataGridView, float fontSize)
        {
            dataGridView.ColumnHeadersDefaultCellStyle.Font = new Font(fontName, fontSize, FontStyle.Regular, GraphicsUnit.Pixel);
            dataGridView.ColumnHeadersDefaultCellStyle.WrapMode = DataGridViewTriState.False;

            foreach (DataGridViewRow r in dataGridView.Rows)
            {
                r.DefaultCellStyle.Font = new Font(fontName, fontSize, FontStyle.Regular, GraphicsUnit.Pixel);
            }

            return dataGridView;
        }

        private void dataGridViewSetup()
        {
            dataGridView1.ColumnHeadersDefaultCellStyle.BackColor = Color.White;
            dataGridView1.ColumnHeadersDefaultCellStyle.SelectionBackColor = Color.White;
            dataGridView1.EnableHeadersVisualStyles = false;

            loadDatabase();
            refreshDataGridView();

            dataGridView1 = UpdateDataGridViewFont(dataGridView1, 14F);
        }

        public void loadDatabase()
        {
            try
            {
                MySqlDataAdapter da = new MySqlDataAdapter("SELECT hr.kode_reservasi, t.nama_tamu, k.nomor_kamar, jk.nama_jenis_kamar, CONCAT(dr.jumlah_penghuni_kamar, ' orang'), DATE_FORMAT(dr.tanggal_check_in, '%W, %d %M %Y'), DATE_FORMAT(dr.tanggal_check_out, '%W, %d %M %Y'), DATE_FORMAT(dr.jadwal_check_in, '%W, %d %M %Y'), DATE_FORMAT(dr.jadwal_check_out, '%W, %d %M %Y'), CONCAT(DATEDIFF(dr.jadwal_check_out, dr.jadwal_check_in), ' hari'), CONCAT('Rp. ', FORMAT(dr.subtotal_biaya_reservasi, '###,###,###')), dr.status_detail_reservasi, dr.id_detail_reservasi, k.kode_kamar FROM header_reservasi hr RIGHT OUTER JOIN detail_reservasi dr ON hr.kode_reservasi = dr.kode_reservasi LEFT OUTER JOIN kamar k ON dr.kode_kamar = k.kode_kamar LEFT OUTER JOIN tamu t ON hr.kode_tamu = t.kode_tamu LEFT OUTER JOIN jenis_kamar jk ON k.id_jenis_kamar = jk.id_jenis_kamar WHERE dr.status_detail_reservasi = @status_detail_reservasi;", Koneksi.getConn());
                da.SelectCommand.Parameters.AddWithValue("@status_detail_reservasi", (radioButton2.Checked) ? 2 : 1);

                dtList = new DataTable();
                da.Fill(dtList);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void fitDataGridViewColumn(int columnIndex, bool setWidthGrow = false, int minimumWidth = 0)
        {
            if (setWidthGrow)
            {
                dataGridView1.Columns[columnIndex].MinimumWidth = (minimumWidth != 0) ? minimumWidth : dataGridView1.Columns[columnIndex].Width;
            }

            dataGridView1.Columns[columnIndex].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            // Console.WriteLine("Column [Index, Width]: " + columnIndex + ", " + dataGridView1.Columns[columnIndex].Width);
        }

        private void fitDataGridViewColumn(string columnName, bool setWidthGrow = false, int minimumWidth = 0)
        {
            if (setWidthGrow)
            {
                dataGridView1.Columns[columnName].MinimumWidth = (minimumWidth != 0) ? minimumWidth : dataGridView1.Columns[columnName].Width;
            }

            dataGridView1.Columns[columnName].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            // Console.WriteLine("Column [Index, Width]: " + columnName + ", " + dataGridView1.Columns[columnName].Width);
        }

        public void refreshDataGridView()
        {
            dataGridView1.DataSource = null;
            dataGridView1.DataSource = dtList;

            for (int i = 0; i < dataGridView1.ColumnCount; i++)
            {
                dtList.Columns[i].ColumnName = columnName[i];
            }

            fitDataGridViewColumn("Kode Reservasi", true, minimumWidth: 132);
            fitDataGridViewColumn("Nomor Kamar", false);
            fitDataGridViewColumn("Jenis Kamar", true, minimumWidth: 116);
            fitDataGridViewColumn("Jumlah Penghuni Kamar", false);

            if (radioButton1.Checked)
            {
                dataGridView1.Columns[5].Visible = false;
                dataGridView1.Columns[6].Visible = false;
                dataGridView1.Columns[10].Visible = false;
                dataGridView1.Columns[11].Visible = false;
                dataGridView1.Columns[12].Visible = false;
                dataGridView1.Columns[13].Visible = false;
            }
            else
            {
                dataGridView1.Columns[6].Visible = false;
                dataGridView1.Columns[7].Visible = false;
                dataGridView1.Columns[9].Visible = false;
                dataGridView1.Columns[11].Visible = false;
                dataGridView1.Columns[12].Visible = false;
                dataGridView1.Columns[13].Visible = false;
            }

            // dataGridView1.ColumnHeadersDefaultCellStyle.Font = new Font("Microsoft Sans Serif", 9F, FontStyle.Regular);
            // dataGridView1.ColumnHeadersDefaultCellStyle.WrapMode = DataGridViewTriState.False;
            // dataGridView1.AutoSize = false;

            if (radioButton2.Checked)
            {
                foreach (DataGridViewRow r in dataGridView1.Rows)
                {
                    r.Cells[10].Style.Alignment = DataGridViewContentAlignment.MiddleRight;
                }
            }

            dataGridView1.ClearSelection();
        }

        private void radioButton1_CheckedChanged(object sender, EventArgs e)
        {
            if (radioButton1.Checked)
            {
                checkBox1.Visible = false;
                label1.Text = "KONFIRMASI CHECK IN";
                button1.Text = "CHECK IN";
                dataGridViewSetup();
            }
        }

        private void radioButton2_CheckedChanged(object sender, EventArgs e)
        {
            if (radioButton2.Checked)
            {
                label1.Text = "KONFIRMASI CHECK OUT";
                button1.Text = "CHECK OUT";
                dataGridViewSetup();
                checkBox1.Checked = true;
                checkBox1.Visible = true;
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (radioButton1.Checked)
            {
                using (MySqlTransaction obTrans = Koneksi.getConn().BeginTransaction())
                {
                    try
                    {
                        string kode_reservasi = dataGridView1.SelectedRows[0].Cells[0].Value.ToString();
                        int id_detail_reservasi = Convert.ToInt32(dataGridView1.SelectedRows[0].Cells[12].Value);

                        MySqlCommand cmd = new MySqlCommand("SELECT dr.jadwal_check_in FROM detail_reservasi dr WHERE dr.id_detail_reservasi = @id_detail_reservasi;", Koneksi.getConn());
                        cmd.Parameters.AddWithValue("@id_detail_reservasi", id_detail_reservasi);
                        DateTime jadwal_check_in = Convert.ToDateTime(cmd.ExecuteScalar().ToString());

                        if (DateTime.Now.Date < jadwal_check_in.Date)
                        {
                            obTrans.Rollback();
                            MessageBox.Show("Check-in tidak boleh di luar jadwal yang sudah direservasi!", "Gagal");
                        }
                        else
                        {
                            cmd = new MySqlCommand("SELECT COUNT(dr.kode_kamar) FROM detail_reservasi dr WHERE dr.kode_kamar = @kode_kamar AND dr.status_detail_reservasi = 2;", Koneksi.getConn());
                            cmd.Parameters.AddWithValue("@kode_kamar", dataGridView1.SelectedRows[0].Cells[13].Value.ToString());
                            int activeReservation = Convert.ToInt32(cmd.ExecuteScalar());

                            if (activeReservation != 0)
                            {
                                obTrans.Rollback();
                                MessageBox.Show("Kamar masih ditempati/belum kosong!", "Gagal");
                            }
                            else
                            {
                                cmd = new MySqlCommand("SELECT jk.harga_jenis_kamar / 2 FROM kamar k LEFT OUTER JOIN jenis_kamar jk ON k.id_jenis_kamar = jk.id_jenis_kamar WHERE k.nomor_kamar = @nomor_kamar;", Koneksi.getConn());
                                cmd.Parameters.AddWithValue("@nomor_kamar", dataGridView1.SelectedRows[0].Cells[2].Value);
                                int harga_jenis_kamar = Convert.ToInt32(cmd.ExecuteScalar());

                                cmd = new MySqlCommand("UPDATE detail_reservasi SET tanggal_check_in = @tanggal_check_in, subtotal_biaya_reservasi = @subtotal_biaya_reservasi, status_detail_reservasi = @status_detail_reservasi WHERE id_detail_reservasi = @id_detail_reservasi;", Koneksi.getConn());
                                cmd.Parameters.AddWithValue("@tanggal_check_in", DateTime.Now.ToString("yyyy-MM-dd"));
                                cmd.Parameters.AddWithValue("@subtotal_biaya_reservasi", harga_jenis_kamar);
                                cmd.Parameters.AddWithValue("@status_detail_reservasi", 2);
                                cmd.Parameters.AddWithValue("@id_detail_reservasi", id_detail_reservasi);
                                cmd.ExecuteNonQuery();

                                cmd = new MySqlCommand("SELECT hr.total_biaya_reservasi FROM header_reservasi hr WHERE hr.kode_reservasi = @kode_reservasi;", Koneksi.getConn());
                                cmd.Parameters.AddWithValue("@kode_reservasi", kode_reservasi);
                                int total_biaya_reservasi = Convert.ToInt32(cmd.ExecuteScalar());

                                cmd = new MySqlCommand("UPDATE header_reservasi SET total_biaya_reservasi = @total_biaya_reservasi WHERE kode_reservasi = @kode_reservasi;", Koneksi.getConn());
                                cmd.Parameters.AddWithValue("@total_biaya_reservasi", total_biaya_reservasi + harga_jenis_kamar);
                                cmd.Parameters.AddWithValue("@kode_reservasi", kode_reservasi);
                                cmd.ExecuteNonQuery();

                                obTrans.Commit();
                                MessageBox.Show("Berhasil check-in!", "Berhasil");

                                dataGridViewSetup();
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        obTrans.Rollback();
                        Console.WriteLine(ex.Message);
                        MessageBox.Show("Tidak ada data yang sedang dipilih!", "Gagal");
                    }
                }
            }
            else
            {
                using (MySqlTransaction obTrans = Koneksi.getConn().BeginTransaction())
                {
                    try
                    {
                        // status detail extra fasilitas
                        string kode_reservasi = dataGridView1.SelectedRows[0].Cells[0].Value.ToString();
                        int id_detail_reservasi = Convert.ToInt32(dataGridView1.SelectedRows[0].Cells[12].Value);

                        MySqlCommand cmd = new MySqlCommand("SELECT dr.subtotal_biaya_reservasi FROM detail_reservasi dr WHERE dr.id_detail_reservasi = @id_detail_reservasi;", Koneksi.getConn());
                        cmd.Parameters.AddWithValue("@id_detail_reservasi", id_detail_reservasi);
                        int subtotal_biaya_reservasi = Convert.ToInt32(cmd.ExecuteScalar());

                        cmd = new MySqlCommand("SELECT IFNULL(SUM(def.subtotal_extra_fasilitas), 0) FROM detail_extra_fasilitas def LEFT OUTER JOIN header_extra_fasilitas hef ON def.id_header_extra_fasilitas = hef.id_header_extra_fasilitas WHERE hef.kode_reservasi = @kode_reservasi AND def.kode_kamar = @kode_kamar AND def.status_detail = @status_detail;", Koneksi.getConn());
                        cmd.Parameters.AddWithValue("@kode_reservasi", kode_reservasi);
                        cmd.Parameters.AddWithValue("@kode_kamar", dataGridView1.SelectedRows[0].Cells[13].Value.ToString());
                        cmd.Parameters.AddWithValue("@status_detail", 1);
                        int subtotal_extra_fasilitas = Convert.ToInt32(cmd.ExecuteScalar());

                        cmd = new MySqlCommand("UPDATE detail_reservasi SET tanggal_check_out = @tanggal_check_out, subtotal_biaya_reservasi = @subtotal_biaya_reservasi, status_detail_reservasi = @status_detail_reservasi WHERE id_detail_reservasi = @id_detail_reservasi;", Koneksi.getConn());
                        cmd.Parameters.AddWithValue("@tanggal_check_out", DateTime.Now.ToString("yyyy-MM-dd"));
                        cmd.Parameters.AddWithValue("@subtotal_biaya_reservasi", subtotal_biaya_reservasi + subtotal_extra_fasilitas);
                        cmd.Parameters.AddWithValue("@status_detail_reservasi", 0);
                        cmd.Parameters.AddWithValue("@id_detail_reservasi", id_detail_reservasi);
                        cmd.ExecuteNonQuery();

                        cmd = new MySqlCommand("SELECT hr.total_biaya_reservasi FROM header_reservasi hr WHERE hr.kode_reservasi = @kode_reservasi;", Koneksi.getConn());
                        cmd.Parameters.AddWithValue("@kode_reservasi", kode_reservasi);
                        int total_biaya_reservasi = Convert.ToInt32(cmd.ExecuteScalar());

                        cmd = new MySqlCommand("UPDATE header_reservasi SET total_biaya_reservasi = @total_biaya_reservasi WHERE kode_reservasi = @kode_reservasi;", Koneksi.getConn());
                        cmd.Parameters.AddWithValue("@total_biaya_reservasi", total_biaya_reservasi + subtotal_extra_fasilitas);
                        cmd.Parameters.AddWithValue("@kode_reservasi", kode_reservasi);
                        cmd.ExecuteNonQuery();

                        cmd = new MySqlCommand("SELECT COUNT(*) FROM detail_reservasi dr WHERE dr.kode_reservasi = @kode_reservasi AND (dr.status_detail_reservasi = 1 OR dr.status_detail_reservasi = 2);", Koneksi.getConn());
                        cmd.Parameters.AddWithValue("@kode_reservasi", kode_reservasi);
                        int activeReservation = Convert.ToInt32(cmd.ExecuteScalar());

                        if (activeReservation == 0)
                        {
                            cmd = new MySqlCommand("UPDATE header_reservasi SET status_header_reservasi = 0 WHERE kode_reservasi = @kode_reservasi;", Koneksi.getConn());
                            cmd.Parameters.AddWithValue("@kode_reservasi", kode_reservasi);
                            cmd.ExecuteNonQuery();
                        }

                        if (checkBox1.Checked)
                        {
                            cmd = new MySqlCommand("SELECT dr.deposito FROM detail_reservasi dr WHERE dr.id_detail_reservasi = @id_detail_reservasi;", Koneksi.getConn());
                            cmd.Parameters.AddWithValue("@id_detail_reservasi", id_detail_reservasi);
                            int deposito = Convert.ToInt32(cmd.ExecuteScalar());

                            cmd = new MySqlCommand("SELECT hr.total_biaya_reservasi FROM header_reservasi hr WHERE hr.kode_reservasi = @kode_reservasi;", Koneksi.getConn());
                            cmd.Parameters.AddWithValue("@kode_reservasi", kode_reservasi);
                            total_biaya_reservasi = Convert.ToInt32(cmd.ExecuteScalar());

                            cmd = new MySqlCommand("UPDATE header_reservasi SET total_biaya_reservasi = @total_biaya_reservasi WHERE kode_reservasi = @kode_reservasi;", Koneksi.getConn());
                            cmd.Parameters.AddWithValue("@total_biaya_reservasi", total_biaya_reservasi - deposito);
                            cmd.Parameters.AddWithValue("@kode_reservasi", kode_reservasi);
                            cmd.ExecuteNonQuery();

                            cmd = new MySqlCommand("UPDATE detail_reservasi SET deposito = 0 WHERE id_detail_reservasi = @id_detail_reservasi;", Koneksi.getConn());
                            cmd.Parameters.AddWithValue("@id_detail_reservasi", id_detail_reservasi);
                            cmd.ExecuteNonQuery();
                        }

                        obTrans.Commit();
                        MessageBox.Show("Berhasil check-out!", "Berhasil");

                        dataGridViewSetup();
                    }
                    catch (Exception ex)
                    {
                        obTrans.Rollback();
                        Console.WriteLine(ex.Message);
                        MessageBox.Show("Tidak ada data yang sedang dipilih!", "Gagal");
                    }
                }
            }
        }

        private void btnBack_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void dataGridView1_ColumnAdded(object sender, DataGridViewColumnEventArgs e)
        {
            dataGridView1.Columns[e.Column.Index].SortMode = DataGridViewColumnSortMode.NotSortable;
        }

        private void dataGridView1_SelectionChanged(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedRows.Count > 0)
            {
                checkBox1.Enabled = true;
            }
            else
            {
                checkBox1.Enabled = false;
            }
        }
    }
}
