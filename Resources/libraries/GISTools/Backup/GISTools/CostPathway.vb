'Written by John Lindsay, July 28, 2009
'This algorithm can be used to perform a cost pathway analysis

Imports System
Imports System.Threading
Imports System.ComponentModel
Public Class CostPathway
    Implements Interfaces.IPlugin
    Private objHost As Interfaces.IHost
    Const NoData As Integer = -32768

    Public CancelBool As Boolean
    Public Property CancelOp() As Boolean Implements Interfaces.IPlugin.CancelOp
        Get
            Return CancelBool
        End Get
        Set(ByVal value As Boolean)
            CancelBool = value
        End Set
    End Property

    Public Sub Initialize(ByVal Host As Interfaces.IHost) Implements Interfaces.IPlugin.Initialize
        objHost = Host
    End Sub

    Public ReadOnly Property DescriptiveName() As String Implements Interfaces.IPlugin.DescriptiveName
        Get
            Return "Cost Pathway"
        End Get
    End Property

    Public ReadOnly Property Description() As String Implements Interfaces.IPlugin.Description
        Get
            Return "Performs cost-distance pathway analysis using a series of destination grid cells"
        End Get
    End Property

    Public ReadOnly Property ToolboxInfo() As String Implements Interfaces.IPlugin.ToolboxName
        Get
            Return "CostTools"
        End Get
    End Property

    Private Sub ShowFeedback(ByVal message As String, Optional ByVal caption As String = "GAT Message")
        objHost.ShowFeedback(message, caption)
    End Sub

    Private Sub UpdateProgressLabel(ByVal label As String)
        objHost.ProgressBarLabel(label)
    End Sub

    Public Function Execute(ByVal ParameterArray() As String, ByVal worker As BackgroundWorker) As Object Implements Interfaces.IPlugin.Execute
        Dim DestHeaderFile As String = Nothing
        Dim OutputHeaderFile As String = Nothing
        Dim BackLinkHeaderFile As String = Nothing
        Dim DestImage As New GeospatialFiles.GATGrid
        Dim Output As New GeospatialFiles.GATGrid
        Dim BackLink As New GeospatialFiles.GATGrid
        Dim NumCols, NumRows As Integer
        Dim Z, FlowDir As Single
        Dim Progress As Single = 0
        Dim DX() As Integer = {1, 1, 1, 0, -1, -1, -1, 0}
        Dim DY() As Integer = {-1, 0, 1, 1, 1, 0, -1, -1}
        Const LnOf2 As Double = 0.693147180559945
        Dim col, row, a, c As Integer
        Dim X, Y As Integer
        Dim flag As Boolean

        Try
            For a = 0 To UBound(ParameterArray)
                Select Case a
                    Case 0
                        DestHeaderFile = ParameterArray(a)
                    Case 1
                        BackLinkHeaderFile = ParameterArray(a)
                    Case 2
                        OutputHeaderFile = ParameterArray(a)
                End Select
            Next

            If DestHeaderFile = Nothing OrElse BackLinkHeaderFile = Nothing OrElse OutputHeaderFile = Nothing Then
                ShowFeedback("Parameters not set", "An Error Has Occurred")
                Return 2
            End If

            DestImage.HeaderFileName = DestHeaderFile
            NumCols = DestImage.NumberColumns
            NumRows = DestImage.NumberRows

            BackLink.HeaderFileName = BackLinkHeaderFile
            If BackLink.NumberColumns <> NumCols OrElse BackLink.NumberRows <> NumRows Then
                ShowFeedback("Input images must have the same dimensions")
                Return 2
            End If

            Output.HeaderFileName = OutputHeaderFile
            Output.WriteChangesToFile = True
            Output.SetPropertiesUsingAnotherFile(BackLink, "integer")
            Output.DataScale = "continuous"
            Output.InitializeGrid(NumCols, NumRows)

            For row = 0 To NumRows - 1
                For col = 0 To NumCols - 1
                    If DestImage(col, row) > 0 Then
                        X = col
                        Y = row
                        flag = True
                        Do
                            Z = Output(X, Y)
                            If Z = NoData Then
                                Output(X, Y) = 1
                            Else
                                Output(X, Y) = Z + 1
                            End If
                            FlowDir = BackLink(X, Y)
                            If FlowDir > 0 Then
                                'move x and y accordingly
                                c = Math.Log(FlowDir) / LnOf2
                                X += DX(c)
                                Y += DY(c)
                            Else
                                flag = False 'stop the flowpath traverse
                            End If
                        Loop While flag = True
                    End If
                Next
                If CancelBool Then Return 1
                Progress = row / (NumRows - 1) * 100
                worker.ReportProgress(Progress)
            Next

            DestImage.ReleaseMemoryResources()
            BackLink.ReleaseMemoryResources()
            Output.MetadataEntries(Output.NumMetadataEntries + 1) = "Created by the CostPathway tool"
            Dim dateTimeInfo As DateTime = DateTime.Now
            Output.MetadataEntries(Output.NumMetadataEntries + 1) = "Created on " & dateTimeInfo.ToString("F")
            Output.WriteHeaderFile()
            Output.ReleaseMemoryResources()

            worker.ReportProgress(0)

            Return OutputHeaderFile

        Catch e As Exception
            ShowFeedback(e.Message)
            Return 2
        End Try
    End Function

End Class
