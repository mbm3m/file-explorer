Imports System.IO
Imports System.Diagnostics
Imports System.Windows.Forms.DataVisualization.Charting
Imports System.Windows.Forms.DataVisualization.Charting.Series


Public Class Form1


    'button Clilk for start the program 
    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
   TreeView4.Visible = False


        TreeView1.Nodes.Clear()
        TreeView2.Nodes.Clear()
        TreeView3.Nodes.Clear()
        ' Loop to Get Drives

        Dim drives As String() = Environment.GetLogicalDrives
        For Each drive As String In drives

            '
            Dim dnode1 As New TreeNode()
            dnode1.Tag = drive
            dnode1.Text = drive
            dnode1.Text = drive
            TreeView1.Nodes.Add(dnode1)
            Dim dnode3 As New TreeNode()
            dnode3.Tag = drive
            dnode3.Text = drive
            TreeView3.Nodes.Add(dnode3)
            Dim dnode2 As New TreeNode()
            dnode2.Tag = drive
            dnode2.Text = drive
            TreeView2.Nodes.Add(dnode2)
        Next
        TreeView1.Nodes.Clear()

        ' Loop to get drives
        For Each drive As DriveInfo In DriveInfo.GetDrives()
            Dim driveNode As TreeNode = TreeView1.Nodes.Add(drive.Name)
            If drive.Name = "D:\" Then
                Continue For
            End If
            '
            ' Display drive details
            Dim totalSizeInGB As Double = drive.TotalSize / (1024 ^ 3) ' Convert total size to GB
            Dim freeSpaceInGB As Double = drive.TotalFreeSpace / (1024 ^ 3) ' Convert free space to GB
            Dim driveDetails As String = String.Format("Type: {0}, Total Size: {1} GB, Free Space: {2} GB", drive.DriveType, totalSizeInGB.ToString("0.00"), freeSpaceInGB.ToString("0.00"))
            Dim driveDetailsNode As TreeNode = driveNode.Nodes.Add(driveDetails)
            ' Get root folders

            Try
                For Each rootFolder As String In Directory.GetDirectories(drive.Name)
                    Dim rootFolderNode As TreeNode = driveNode.Nodes.Add(rootFolder)
                    ' Add expand option
                Next
                ' Get root files
                For Each rootFile As String In Directory.GetFiles(drive.Name)
                    driveNode.Nodes.Add(rootFile)

                Next

            Catch ex As Exception
                MessageBox.Show("An error occurred while accessing drive information: " & ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
            End Try
        Next




    End Sub
    'this class  use for  check the type of  the  drive
    Public Sub akk()
        Try
            TreeView4.Nodes.Clear()
            Dim drives() As DriveInfo = DriveInfo.GetDrives()

            ' Iterate through each drive
            For Each drive As DriveInfo In drives
                ' Check if the drive is ready
                If drive.IsReady Then
                    ' Check if the drive is not D:\
                    If drive.Name <> "D:\" Then
                        ' Create a TreeNode for the drive
                        Dim driveNode As New TreeNode(drive.Name)
                        driveNode.Tag = drive.RootDirectory.FullName ' Store the full path of the drive in the Tag property

                        ' Add the drive node to the TreeView
                        TreeView4.Nodes.Add(driveNode)

                    End If
                End If
            Next

            ' Display message prompting the user to select the browse button
          
        Catch ex As Exception
            ' Handle any exceptions
            MessageBox.Show("An error occurred: " & ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub





    ' this treeview use for the  bar chart
    Private Sub TreeView3_AfterSelect(sender As Object, e As TreeViewEventArgs) Handles TreeView3.AfterSelect
        Try

            Chart1.Series.Clear()
            Chart1.Titles.Clear()
            ' Get the selected node
            Dim selectedNode As TreeNode = e.Node
            ' Check if the selected node is a folder
            If selectedNode IsNot Nothing AndAlso Directory.Exists(selectedNode.Tag.ToString()) Then
                Dim selectedFolder As String = selectedNode.Tag.ToString()

                ' Create a new series for the bar chart
                Dim series As New Series("File and Folder Sizes")
                series.ChartType = SeriesChartType.Bar ' Change to Bar chart type

                ' Get all files in the folder and calculate their sizes
                Dim files As String() = Directory.GetFiles(selectedFolder)
                Dim fileSizes As New List(Of Tuple(Of String, Double))() ' List to store file name and size tuples

                For Each file As String In files
                    Dim fileInfo As New FileInfo(file)
                    Dim fileSizeInMB As Double = fileInfo.Length / (1024 ^ 2) ' Convert file size to MB
                    fileSizes.Add(New Tuple(Of String, Double)(Path.GetFileName(file), fileSizeInMB))

                    Dim label As String = Path.GetFileName(file) & " (" & fileSizeInMB.ToString("0.00") & " MB)" ' Format label as "FileName (Size MB)"
                    Dim dataPoint As New DataPoint()
                    dataPoint.SetValueXY(Path.GetFileName(file), fileSizeInMB)
                    dataPoint.Label = label
                    series.Points.Add(dataPoint)
                    dataPoint.Color = Color.Red
                Next

                ' Get all directories in the folder and calculate their sizes
                Dim directories As String() = Directory.GetDirectories(selectedFolder)

                Dim folderSizes As New List(Of Tuple(Of String, Double))() ' List to store folder name and size tuples

                For Each directory As String In directories
                    Dim directoryInfo As New DirectoryInfo(directory)
                    Dim folderSizeInMB As Double = GetDirectorySize(directory) / (1024 ^ 2) ' Convert folder size to MB
                    folderSizes.Add(New Tuple(Of String, Double)(directoryInfo.Name, folderSizeInMB))
                Next

                ' Sort files and folders by size
                fileSizes = fileSizes.OrderBy(Function(x) x.Item2).ToList()
                folderSizes = folderSizes.OrderBy(Function(x) x.Item2).ToList()

                ' Add data points for folders to the series
                For Each folderSize In folderSizes
                    Dim sizeInMB As Double = folderSize.Item2
                    Dim label As String = folderSize.Item1 & " (" & sizeInMB.ToString("0.00") & " MB)" ' Format label as "FolderName (Size MB)"
                    Dim dataPoint As New DataPoint()
                    dataPoint.SetValueXY(folderSize.Item1, sizeInMB)
                    dataPoint.Label = label
                    series.Points.Add(dataPoint)
                    dataPoint.Color = Color.Blue
                Next

                ' Add the series to the chart
                Chart1.Series.Add(series)
                '  series.Name = "Series1" ' Set the name of the series

                ' Set chart title
                ''     Chart1.Titles.Add("Files and Folders Sizes in Folder: " & selectedFolder)
                ''   Dim title As Title = Chart1.Titles.FirstOrDefault()
                '' If title IsNot Nothing Then
                ''Title.Alignment = ContentAlignment.MiddleCenter ' Set title alignment
                '' End If
                ' Customize appearance
                ''  Chart1.BackColor = Color.LightGray
                '    Chart1.ChartAreas(0).AxisY.MajorGrid.LineColor = Color.LightGray
                Chart1.ChartAreas(0).AxisY.Interval = 1
                Chart1.ChartAreas(0).AxisY.IntervalOffset = 0.5
                Chart1.ChartAreas(0).AxisX.Interval = 1
                Chart1.ChartAreas(0).AxisX.IntervalOffset = 0.5
                Chart1.ChartAreas(0).AxisX.IsLabelAutoFit = True
                Chart1.ChartAreas(0).AxisY.IsLabelAutoFit = True
                Chart1.ChartAreas(0).AxisX.LineDashStyle = ChartDashStyle.NotSet
                Chart1.ChartAreas(0).AxisY.LineDashStyle = ChartDashStyle.NotSet
                Chart1.ChartAreas(0).AxisX.MajorGrid.Enabled = False
                Chart1.ChartAreas(0).AxisY.MajorGrid.Enabled = False
                series.SmartLabelStyle.Enabled = True
                'Chart1.BorderlineColor = Color.Gray
                Chart1.BorderlineDashStyle = ChartDashStyle.Solid
                Chart1.BorderlineWidth = 1
                Chart1.Width = Me.Width
                Chart1.Size = New Size(500, 400)
                ''   Chart1.Location = New Point(20, 20)
                Chart1.BorderlineColor = Color.Black ' Set the border color
                Chart1.BorderlineDashStyle = ChartDashStyle.Solid ' Set the border style
                Chart1.BorderlineWidth = 2 ' Set the border width


                ' Add the selected folder as the root node
                Dim folderPath As String = e.Node.Tag.ToString()
                Dim rootNode As New TreeNode(folderPath)
                rootNode.Tag = folderPath ' Store the folder path in the Tag property
                TreeView3.Nodes.Add(rootNode)

                ' Example condition for C: drive
                Dim cdrive As Boolean = True
                ' Example condition for D: drive
                Dim ddrive As Boolean = False

                If cdrive OrElse ddrive Then
                    ' Do something for C: or D: drive
                    LoadFolderAndSubfoldersData(folderPath, rootNode)
                End If
            Else
                MessageBox.Show("Please select a folder.", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information)
            End If

            Try
                ' Your existing code here...

            Catch ex As UnauthorizedAccessException
                ' Handle access denied errors
                MessageBox.Show("Access to the selected folder is unauthorized. Please ensure you have the necessary permissions.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Catch ex As System.AccessViolationException
                ' Handle memory access violation errors
                MessageBox.Show("An error occurred: Memory access violation (0xC0000005). Please contact support for assistance.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Catch ex As Exception
                ' Handle other types of exceptions

                MessageBox.Show("An unexpected error occurred: " & ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
            End Try
        Catch ex As Exception
            ' Handle other types of exceptions
            MessageBox.Show("An error occurred: " & ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try

    End Sub
    ' this  class use for the folder And subfolder of  the drive 
    Private Sub LoadFolderAndSubfoldersData(ByVal folderPath As String, ByVal rootNode As TreeNode)
        ' Clear existing nodes
        rootNode.Nodes.Clear()

        ' Check if the folder exists
        If Directory.Exists(folderPath) Then
            ' Create a new series for the bar chart
            Dim series As New Series("File and Folder Sizes")
            ''     series.ChartType = SeriesChartType.Bar ' Change to Bar chart type

            ' Recursively load folders and subfolders
            LoadFolderAndSubfolders(folderPath, series, rootNode)

            ' Add the series to the chart
            ''Chart1.Series.Add(series)
            ''   series.Name = "Series1" ' Set the name of the series

            ' Set chart title
            ''  Chart1.Titles.Add("Files and Folders Sizes in C: Drive")
            ''Dim title As Title = Chart1.Titles.FirstOrDefault()
            '' If title IsNot Nothing Then
            ''Title.Alignment = ContentAlignment.MiddleCenter ' Set title alignment
            '' End If

            ' Customize appearance

        End If
    End Sub

    Private Sub LoadFolderAndSubfolders(ByVal folderPath As String, ByVal series As Series, ByVal parentNode As TreeNode)
        Try
            ' Get the drive letter of the folder
            Dim drive As Char = Path.GetPathRoot(folderPath)(0)

            ' Check if the drive letter is C or D
            If drive = "C"c OrElse drive = "D"c Then
                ' Get all directories in the folder
                Dim directories As String() = Directory.GetDirectories(folderPath)

                ' Iterate through each directory
                For Each directory As String In directories
                    Dim directoryInfo As New DirectoryInfo(directory)
                    '     Dim folderSizeInMB As Double = GetDirectorySize(directory) / (1024 ^ 2) ' Convert folder size to MB
                    '    Dim label As String = directoryInfo.Name & " (" & folderSizeInMB.ToString("0.00") & " MB)" ' Format label as "FolderName (Size MB)"
                    '  Dim dataPoint As New DataPoint()
                    '   dataPoint.SetValueXY(directoryInfo.Name, folderSizeInMB)
                    ' dataPoint.Label = label
                    ' series.Points.Add(dataPoint)
                    'dataPoint.Color = Color.Blue

                    ' Add the folder to the tree view
                    Dim folderNode As New TreeNode(directoryInfo.Name)
                    folderNode.Tag = directory ' Store the folder path in the Tag property
                    parentNode.Nodes.Add(folderNode)
                    ' Do not load data for subfolders here
                Next



            End If
        Catch ex As UnauthorizedAccessException
            ' Handle "Access Denied" error for the C: drive
            MessageBox.Show("Access to the folder is denied. Please ensure you have the necessary permissions.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)



        End Try
    End Sub






    Private Sub AddFolderInfoToPDF(folderName As String, sizeInMB As Double)
        ' Code to add folder information to the PDF document
    End Sub
    Private Sub AddFilesInfoToPDF(folderPath As String)
        Dim filesInfo As List(Of String) = GetWordAndPDFFiles(folderPath)
        For Each fileInfo As String In filesInfo
            ' Code to add file information to the PDF document
        Next
    End Sub
    ' this treeview use for the pie chart of  the drive
    Private Sub TreeView2_AfterSelect(sender As Object, e As TreeViewEventArgs) Handles TreeView2.AfterSelect

        Try
            Chart2.Series.Clear()
            '  Chart2.Titles.Clear()
            ' Get the selected node
            Dim selectedNode As TreeNode = e.Node
            ' Check if the selected node is a folder
            If selectedNode IsNot Nothing AndAlso Directory.Exists(selectedNode.Tag.ToString()) Then
                Dim selectedFolder As String = selectedNode.Tag.ToString()

                ' Create a new series for the bar chart
                Dim series As New Series("File and Folder Sizes")
                series.ChartType = SeriesChartType.Pie ' Change to Bar chart type

                ' Get all files in the folder and calculate their sizes
                Dim files As String() = Directory.GetFiles(selectedFolder)
                Dim fileSizes As New List(Of Tuple(Of String, Double))() ' List to store file name and size tuples

                For Each file As String In files
                    Dim fileInfo As New FileInfo(file)
                    Dim fileSizeInMB As Double = fileInfo.Length / (1024 ^ 2) ' Convert file size to MB
                    fileSizes.Add(New Tuple(Of String, Double)(Path.GetFileName(file), fileSizeInMB))

                    Dim label As String = Path.GetFileName(file) & " (" & fileSizeInMB.ToString("0.00") & " MB)" ' Format label as "FileName (Size MB)"
                    Dim dataPoint As New DataPoint()
                    dataPoint.SetValueXY(Path.GetFileName(file), fileSizeInMB)
                    dataPoint.Label = label
                    series.Points.Add(dataPoint)
                    dataPoint.Color = Color.Red
                Next

                ' Get all directories in the folder and calculate their sizes
                Dim directories As String() = Directory.GetDirectories(selectedFolder)
                Dim folderSizes As New List(Of Tuple(Of String, Double))() ' List to store folder name and size tuples

                For Each directory As String In directories
                    Dim directoryInfo As New DirectoryInfo(directory)
                    Dim folderSizeInMB As Double = GetDirectorySize(directory) / (1024 ^ 2) ' Convert folder size to MB
                    folderSizes.Add(New Tuple(Of String, Double)(directoryInfo.Name, folderSizeInMB))
                Next

                ' Sort files and folders by size
                fileSizes = fileSizes.OrderBy(Function(x) x.Item2).ToList()
                folderSizes = folderSizes.OrderBy(Function(x) x.Item2).ToList()

                ' Add data points for folders to the series
                For Each folderSize In folderSizes
                    Dim sizeInMB As Double = folderSize.Item2
                    Dim label As String = folderSize.Item1 & " (" & sizeInMB.ToString("0.00") & " MB)" ' Format label as "FolderName (Size MB)"
                    Dim dataPoint As New DataPoint()
                    dataPoint.SetValueXY(folderSize.Item1, sizeInMB)
                    dataPoint.Label = label
                    series.Points.Add(dataPoint)
                    dataPoint.Color = Color.Blue
                Next

                ' Add the series to the chart
                Chart2.Series.Add(series)
                '    series.Name = "Series1" ' Set the name of the series

                ' Set chart title
                ''    Chart1.Titles.Add("Files and Folders Sizes in Folder: " & selectedFolder)
                ''  Dim title As Title = Chart2.Titles.FirstOrDefault()
                ''If title IsNot Nothing Then
                ''Title.Alignment = ContentAlignment.MiddleCenter ' Set title alignment
                ''End If
                ' Customize appearance
                Chart2.Size = New Size(500, 400)
                Chart1.Anchor = AnchorStyles.Top Or AnchorStyles.Left  ' Anchor the chart to the top-right corner of the form
                Me.StartPosition = FormStartPosition.CenterScreen

                '' Chart2.Location = New Point(20, 20)
                Chart2.BorderlineColor = Color.Black ' Set the border color
                Chart2.BorderlineDashStyle = ChartDashStyle.Solid ' Set the border style
                Chart2.BorderlineWidth = 2
                ' Add the selected folder as the root node
                Dim folderPath As String = e.Node.Tag.ToString()
                Dim rootNode As New TreeNode(folderPath)
                rootNode.Tag = folderPath ' Store the folder path in the Tag property
                TreeView2.Nodes.Add(rootNode)

                ' Example condition for C: drive
                Dim cdrive As Boolean = True
                ' Example condition for D: drive
                Dim ddrive As Boolean = False


                If cdrive OrElse ddrive Then
                    ' Do something for C: or D: drive
                    LoadFolderAndSubfoldersData(folderPath, rootNode)
                End If
            Else

                MessageBox.Show("Please select a folder.", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information)
            End If
            Try
                ' Your existing code here...

            Catch ex As UnauthorizedAccessException
                ' Handle access denied errors
                MessageBox.Show("Access to the selected folder is unauthorized. Please ensure you have the necessary permissions.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Catch ex As System.AccessViolationException
                ' Handle memory access violation errors
                MessageBox.Show("An error occurred: Memory access violation (0xC0000005). Please contact support for assistance.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Catch ex As Exception
                ' Handle other types of exceptions
                MessageBox.Show("An unexpected error occurred: " & ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
            End Try
        Catch ex As Exception
            ' Handle other types of exceptions
            MessageBox.Show("An error occurred: " & ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub
    Private Function GetWordAndPDFFiles(ByVal directoryPath As String) As List(Of String)
        Dim wordAndPDFFiles As New List(Of String)()
        Try
            ' Get all Word files (docx) in the directory and its subdirectories
            Dim wordFiles As String() = Directory.GetFiles(directoryPath, "*.docx", SearchOption.AllDirectories)
            wordAndPDFFiles.AddRange(wordFiles)

            ' Get all PDF files in the directory and its subdirectories
            Dim pdfFiles As String() = Directory.GetFiles(directoryPath, "*.pdf", SearchOption.AllDirectories)
            wordAndPDFFiles.AddRange(pdfFiles)

            ' Create a list to store folder names and sizes
            Dim fileInfoList As New List(Of String)()

            ' Add file information to the list
            For Each filePath As String In wordAndPDFFiles
                Dim fileInfo As New FileInfo(filePath)
                Dim fileSizeInMB As Double = fileInfo.Length / (1024 ^ 2) ' Convert file size to MB
                Dim folderInfo As String = fileInfo.Name & ": " & fileSizeInMB.ToString("0.00") & " MB"
                fileInfoList.Add(folderInfo)
            Next

            ' Return the list containing folder names and sizes
            Return fileInfoList

        Catch ex As Exception
            ' Handle exceptions
            MessageBox.Show("Error: " & ex.Message)
            Return New List(Of String)() ' Return an empty list in case of error
        End Try
    End Function

    Private Function GetDirectorySize(ByVal directoryPath As String) As Long
        Dim size As Long = 0

        Try
            ' Get the size of all files in the directory and its subdirectories
            Dim files As String() = Directory.GetFiles(directoryPath, "*.*", SearchOption.AllDirectories)
            For Each file As String In files
                Dim fileInfo As New FileInfo(file)
                size += fileInfo.Length
            Next
        Catch ex As Exception
            ' Handle exceptions such as unauthorized access
            MessageBox.Show("Error: " & ex.Message)
        End Try

        Return size
    End Function

    Private Function FormatFileSize(ByVal size As Long) As String
        If size >= 1024 ^ 3 Then
            Return Math.Round(size / 1024 ^ 3, 2) & " GB"
        ElseIf size >= 1024 ^ 2 Then
            Return Math.Round(size / 1024 ^ 2, 2) & " MB"
        ElseIf size >= 1024 Then
            Return Math.Round(size / 1024, 2) & " KB"
        Else
            Return size & " bytes"
        End If
    End Function

    Private Sub Form2_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        akk()

    End Sub



















    Private Sub LoadFolderData(folderPath As String, parentNode As TreeNode)
        Try
            ' Get all files in the folder
            Dim files As String() = Directory.GetFiles(folderPath)
            For Each file As String In files
                Dim fileName As String = Path.GetFileName(file)
                Dim fileNode As New TreeNode(fileName)
                fileNode.Tag = file ' Store the file path in the Tag property
                parentNode.Nodes.Add(fileNode)
            Next

            ' Get all subfolders in the folder
            Dim subfolders As String() = Directory.GetDirectories(folderPath)
            For Each subfolder As String In subfolders
                Dim folderName As String = Path.GetFileName(subfolder)
                Dim folderNode As New TreeNode(folderName)
                folderNode.Tag = subfolder ' Store the folder path in the Tag property
                parentNode.Nodes.Add(folderNode)
                ' Recursively load data for subfolders
                LoadFolderData(subfolder, folderNode)

            Next
        Catch ex As Exception
            ' Handle exceptions
            '   MessageBox.Show($"An error occurred while loading folder data: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub




    Public Sub dds()
        ' Clear TreeView
        TreeView1.Nodes.Clear()
        ' Loop to Get Drives
        For Each myDrive As System.IO.DriveInfo In System.IO.DriveInfo.GetDrives()
            Dim myDriveNode As TreeNode = TreeView1.Nodes.Add(myDrive.Name)
            ' Adding "Expand" string is used to Add the Expand "[+]" option on Drives
            myDriveNode.Nodes.Add("Expand")
        Next
    End Sub

    Private defaultDir As String = "C:\MyDirectory\"
    Private logwriter As System.IO.StreamWriter ' Declare logwriter at the class level

    Private Sub But(sender As Object, e As EventArgs)
        Dim folder As String = Now.ToString("MM_dd_yyyy_hh_mm_ss")

        ' Create the folder if it doesn't exist
        If (Not System.IO.Directory.Exists(folder)) Then
            System.IO.Directory.CreateDirectory(folder)
        End If

        Try
            ' Set the current directory (ensure defaultDir is defined)
            Directory.SetCurrentDirectory(Path.Combine(defaultDir, folder))
        Catch ex As DirectoryNotFoundException
            Console.WriteLine("The specified directory does not exist. {0}", e)
        End Try

        ' Create a log file
        Dim LogBook = folder & "log.txt"
        logwriter = New System.IO.StreamWriter(LogBook)

        ' Clear TreeView nodes
        TreeView1.Nodes.Clear()
        TreeView3.Nodes.Clear()

        ' Populate TreeViews with logical drives
        Dim drives As String() = Environment.GetLogicalDrives
        For Each drive As String In drives
            Dim dnode1 As New TreeNode()
            dnode1.Tag = drive
            dnode1.Text = drive
            TreeView1.Nodes.Add(dnode1)

            Dim dnode3 As New TreeNode()
            dnode3.Tag = drive
            dnode3.Text = drive
            TreeView3.Nodes.Add(dnode3)


        Next

        ' Call the dds method
        dds()

        Try
            ' Handle UnauthorizedAccessException
        Catch ex As UnauthorizedAccessException
            If (ex.Message.Contains("file")) Then
                MessageBox.Show("Access to the file is denied. Please ensure you have the necessary permissions.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
            ElseIf (ex.Message.Contains("directory") Or ex.Message.Contains("folder")) Then
                MessageBox.Show("Access to the folder is denied. Please ensure you have the necessary permissions.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
            ElseIf (ex.Message.Contains("drive")) Then
                MessageBox.Show("Access to the drive is denied. Please ensure you have the necessary permissions.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Else
                MessageBox.Show("Data access is denied. Please ensure you have the necessary permissions.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
            End If
        Catch ex As Exception
            ' Handle other types of exceptions
            MessageBox.Show("An error occurred: " & ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub


    Private Sub TreeView1_AfterSelect(sender As Object, e As TreeViewEventArgs) Handles TreeView1.AfterSelect

    End Sub



    Private Sub Chart1_Click(sender As Object, e As EventArgs) Handles Chart1.Click

    End Sub

    Private Sub Button2_Click(sender As Object, e As EventArgs)


    End Sub

    Private Sub TreeView4_AfterSelect(sender As Object, e As TreeViewEventArgs) Handles TreeView4.AfterSelect
        Try
            ' Get the selected node
            Dim selectedNode As TreeNode = e.Node

            ' Check if the selected node represents a drive
            If IsDriveNode(selectedNode) Then
                ' Display message prompting the user to select the browse button
                MessageBox.Show("Please select the browse button to proceed.", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information)
            End If
        Catch ex As Exception
            ' Handle any exceptions
            MessageBox.Show("An error occurred: " & ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub

    Private Function IsDriveNode(node As TreeNode) As Boolean
        ' Check if the node's tag represents a drive
        If node IsNot Nothing AndAlso Directory.Exists(node.Tag.ToString()) Then
            Dim driveInfo As DriveInfo = New DriveInfo(node.Tag.ToString())
            Return True
        End If
        Return False
    End Function

    Private Sub TreeView5_AfterSelect(sender As Object, e As TreeViewEventArgs)
        Try
            ' Get the selected node
            Dim selectedNode As TreeNode = e.Node

            ' Check if the selected node represents a drive
            If IsDriveNode(selectedNode) Then
                ' Display message prompting the user to select the browse button
                MessageBox.Show("Please select the browse button to proceed.", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information)
            End If
        Catch ex As Exception
            ' Handle any exceptions
            MessageBox.Show("An error occurred: " & ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub
End Class