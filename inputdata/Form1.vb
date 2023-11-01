Imports MySql.Data.MySqlClient
Imports System.IO
Public Class Form1
    Sub tampilan()
        Dim koneksi As New MySqlConnection("server=localhost;user id=root;database=prns101")
        koneksi.Open()
        Dim tampil As New MySqlDataAdapter("select * from mb", koneksi)
        Dim table As New DataTable
        tampil.Fill(table)
        DataGridView1.DataSource = table
        DataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill
        DataGridView1.Columns(0).HeaderText = "Kode"
        DataGridView1.Columns(1).HeaderText = "Nama"
        DataGridView1.Columns(2).HeaderText = "Tipe Barang"
        DataGridView1.Columns(3).HeaderText = "Tahun Pembelian"
        DataGridView1.Columns(4).HeaderText = "Tipe pembelian"
        DataGridView1.Columns(5).HeaderText = "gambar"
    End Sub
    Sub uploadgmbar()
        OpenFileDialog1.Filter = "Bitmaps (*.bmp)|*.bmp|GIFs (*.gif)|*.gif|JPEGs (*.jpg)|*.jpg|All Image File (*.jpg,*.bmp,*.gif)|*.jpg;*.bmp;*.gif"
        If OpenFileDialog1.ShowDialog() = Windows.Forms.DialogResult.OK Then
            PictureBox2.Image = Image.FromFile(Me.OpenFileDialog1.FileName)
            PictureBox2.SizeMode = PictureBoxSizeMode.StretchImage
        End If
    End Sub
    Private Sub Button1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button1.Click
        Try
            Dim con As New MySqlConnection("server=localhost;user id=root;database=prns101")
            con.Open()
            Dim input As New MySqlCommand("insert into mb(kode,nama,tipe,tahun,pembelian,gambar) values('" & TextBox1.Text & "','" & TextBox5.Text & "','" & TextBox4.Text & "','" & TextBox3.Text & "','" & TextBox2.Text & "',@gambar)", con)
            Dim MS As New MemoryStream
            PictureBox2.Image.Save(MS, PictureBox2.Image.RawFormat)
                input.Parameters.Add("@gambar", MySqlDbType.Blob).Value = MS.ToArray()
            Dim reader As MySqlDataReader = input.ExecuteReader
            MsgBox("DATA BERHASIL DISIMPAN", MsgBoxStyle.Information)
            con.Close()
            tampilan()

        Catch ex As Exception
            MsgBox("GAGAL DISIMPAN" & vbNewLine & vbNewLine & ex.Message, MsgBoxStyle.Critical)
        End Try
    End Sub
    Private Function getdatagambar(ByVal opic As PictureBox) As Byte()
        Dim ms As New MemoryStream()
        opic.Image.Save(ms, opic.Image.RawFormat)
        Dim datagbr As Byte() = ms.GetBuffer()
        Return datagbr
    End Function

    Private Sub Button2_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button2.Click
        Try
            Dim koneksi As New MySqlConnection("server=localhost;user id=root;database=prns101")
            koneksi.Open()
            Dim ganti As New MySqlCommand("update mb set kode ='" & TextBox1.Text & "',nama ='" & TextBox5.Text & "',tipe ='" & TextBox4.Text & "',tahun ='" & TextBox3.Text & "',pembelian ='" & TextBox2.Text & "', gambar= @gambar where kode = '" & TextBox6.Text & "'", koneksi)
            With ganti.Parameters.AddWithValue("@gambar", getdatagambar(PictureBox2))
            End With
            Dim reader As MySqlDataReader = ganti.ExecuteReader
            MsgBox("DATA DIUPDATE", MsgBoxStyle.Information)
            koneksi.Close()
            TextBox1.ReadOnly = False
            Button1.Enabled = True
            tampilan()
        Catch ex As Exception
            MsgBox("DATA GAGAL DIUPDATE" & vbNewLine & vbNewLine & ex.Message, MsgBoxStyle.Critical)
        End Try
    End Sub

    Private Sub Button5_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button5.Click
        Dim koneksi As New MySqlConnection("server=localhost;user id=root;database=prns101")
        koneksi.Open()
        Dim cari As New MySqlCommand("select * from mb where kode ='" & TextBox6.Text & "'", koneksi)
        Dim reader As MySqlDataReader = cari.ExecuteReader
        If reader.Read Then
            TextBox1.Text = reader("kode")
            TextBox1.ReadOnly = True
            Button1.Enabled = False
            TextBox5.Text = reader("nama")
            TextBox4.Text = reader("tipe")
            TextBox3.Text = reader("tahun")
            TextBox2.Text = reader("pembelian")
            Dim foto As Byte() = reader.Item("gambar")
            Dim ft As New MemoryStream(foto)
            PictureBox2.Image = Image.FromStream(ft)
        Else
            MsgBox("data tidak ditemukan", vbInformation, "informasi")
        End If
    End Sub

    Private Sub Button3_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button3.Click
        Try
            Dim koneksi As New MySqlConnection("server=localhost;user id=root;database=prns101")
            koneksi.Open()
            Dim hapus As New MySqlCommand("delete from mb where kode='" & TextBox6.Text & "'", koneksi)
            Dim reader As MySqlDataReader = hapus.ExecuteReader
            TextBox1.Text = ""
            TextBox5.Text = ""
            TextBox4.Text = ""
            TextBox3.Text = ""
            TextBox2.Text = ""
            TextBox1.ReadOnly = False
            Button1.Enabled = True
            tampilan()
        Catch ex As Exception
            MsgBox("DATA GAGAL DIHAPUS" & vbNewLine & vbNewLine & ex.Message, MsgBoxStyle.Critical)
        End Try


    End Sub

    Private Sub Button8_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button8.Click
        uploadgmbar()
    End Sub

    Private Sub Form1_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        tampilan()

    End Sub

    Private Sub Button4_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button4.Click
        TextBox1.Text = ""
        TextBox5.Text = ""
        TextBox4.Text = ""
        TextBox3.Text = ""
        TextBox2.Text = ""
        TextBox1.ReadOnly = False
        Button1.Enabled = True
    End Sub

    Private Sub Button6_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button6.Click
        Me.Close()

    End Sub
End Class
