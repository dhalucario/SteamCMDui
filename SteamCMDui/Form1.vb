Imports Newtonsoft.Json

Public Class Form1

    Public Function SteamWebApiAppList(Type As String)

        Dim RW As Integer = 1
        Dim SteamGameInfoObject As SteamAPIJSONResult = JsonConvert.DeserializeObject(Of SteamAPIJSONResult)(jsonSource)

        If Type = "appid" Then

            Dim GameInformation(450000) As Integer

            For Each Item In SteamGameInfoObject.applist.apps

                GameInformation(RW) = Item.appid
                RW = RW + 1

            Next

            Return GameInformation

        ElseIf Type = "name" Then

            Dim GameInformation(450000) As String

            For Each Item In SteamGameInfoObject.applist.apps

                GameInformation(RW) = Item.name
                RW = RW + 1

            Next

            Return GameInformation

        Else

            Return 0

        End If
    End Function

    Public Class App

        Public Property appid As Integer
        Public Property name As String

    End Class

    Public Class Applist

        Public Property apps As App()

    End Class

    Public Class SteamAPIJSONResult

        Public Property applist As Applist

    End Class

    Dim jsonSource = New System.Net.WebClient().DownloadString("http://api.steampowered.com/ISteamApps/GetAppList/v2?format=json")
    Dim SteamGameAppIDs(450000) As Integer
    Dim SteamGameNames(450000) As String
    Dim RW As Integer = 0
    Dim TempString As String
    Dim TempStrArr() As String

    Private Sub Form1_Load(sender As Object, e As EventArgs) Handles MyBase.Load

        SteamGameAppIDs = SteamWebApiAppList("appid")
        SteamGameNames = SteamWebApiAppList("name")

        RW = 0

        While RW < SteamGameAppIDs.Length

            If Not SteamGameAppIDs(RW) = Nothing Then

                CheckedListBox1.Items.Add(SteamGameAppIDs(RW).ToString + " - " + SteamGameNames(RW))

            End If

            RW = RW + 1

        End While
    End Sub

    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click

        For Each Item In CheckedListBox1.CheckedItems

            TempStrArr = Item.ToString.Split("-")

            TempString = "+app_update " + TempStrArr(0) + "validate "

            If CheckBox1.Checked Then

                TempString = "+force_install_dir " + TextBox1.Text + " " + TempString

            End If

            If CheckBox2.Checked Then

                TempString = "+login anonymous " + TempString

            Else

                TempString = "+login " + TextBox2.Text + " " + TextBox3.Text + " " + TempString

            End If

            TempString = TempString + "+quit"

            Try

                Process.Start(New ProcessStartInfo(My.Application.Info.DirectoryPath + "\SteamCMD.exe", TempString))

            Catch ex As Exception

                MsgBox(ex.Message)

            End Try
        Next
    End Sub

    Private Sub Button2_Click(sender As Object, e As EventArgs) Handles Button2.Click

        TempString = ""

        CheckedListBox1.Items.Clear()
        RW = 0

        While RW < SteamGameAppIDs.Length

            If Not SteamGameAppIDs(RW) = Nothing Then

                TempString = SteamGameAppIDs(RW).ToString + " - " + SteamGameNames(RW)

                If TempString.Contains(TextBox4.Text) Then

                    CheckedListBox1.Items.Add(TempString)

                End If
            End If

                RW = RW + 1

        End While
    End Sub

    Private Sub CheckBox2_CheckedChanged(sender As Object, e As EventArgs) Handles CheckBox2.CheckedChanged

        If CheckBox2.Checked Then

            TextBox2.Enabled = False
            TextBox3.Enabled = False

        Else

            TextBox2.Enabled = True
            TextBox3.Enabled = True

        End If
    End Sub

    Private Sub CheckBox1_CheckedChanged(sender As Object, e As EventArgs) Handles CheckBox1.CheckedChanged

        If CheckBox1.Checked Then

            TextBox1.Enabled = True

        Else

            TextBox1.Enabled = False

        End If
    End Sub

    Private Sub Button3_Click(sender As Object, e As EventArgs) Handles Button3.Click

        TempString = ""

        If Not ComboBox1.Text = Nothing Then

            TempString = "+app_set_config 90 mod " + ComboBox1.Text + " +app_update 90 "

            If CheckBox1.Checked Then

                TempString = "+force_install_dir " + TextBox1.Text + " " + TempString

            End If

            If CheckBox2.Checked Then

                TempString = "+login anonymous " + TempString

            Else

                TempString = "+login " + TextBox2.Text + " " + TextBox3.Text + " " + TempString

            End If
        End If

        TempString = TempString + "+quit"

        Try

            Process.Start(New ProcessStartInfo(My.Application.Info.DirectoryPath + "\SteamCMD.exe", TempString))

        Catch ex As Exception

            MsgBox(ex.Message)

        End Try
    End Sub
End Class