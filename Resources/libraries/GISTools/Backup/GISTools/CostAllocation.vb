'#################################################################################################################################################################################################################################################
'# Copyright John Lindsay, 2009. Code written by John Lindsay, July 28, 2009.
'# This program is free software: you can redistribute it and/or modify it under the terms of the GNU General Public License as published by the Free Software Foundation, either version 3 of the License, or (at your option) any later version.
'# This program is distributed in the hope that it will be useful, but WITHOUT ANY WARRANTY; without even the implied warranty of MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the GNU General Public License for more details.
'# You should have received a copy of the GNU General Public License along with this program.  If not, see <http://www.gnu.org/licenses/>.
'#################################################################################################################################################################################################################################################

'This algorithm can be used perform a source-allocation on a least cost-distance operation

Imports System
Imports System.Threading
Imports System.ComponentModel
Public Class CostAllocation
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
            Return "Cost Allocation"
        End Get
    End Property

    Public ReadOnly Property Description() As String Implements Interfaces.IPlugin.Description
        Get
            Return "Performs cost-distance source allocation"
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
        Dim SourceHeaderFile As String = Nothing
        Dim OutputHeaderFile As String = Nothing
        Dim BackLinkHeaderFile As String = Nothing
        Dim SourceImage As New GeospatialFiles.GATGrid
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
                        SourceHeaderFile = ParameterArray(a)
                    Case 1
                        BackLinkHeaderFile = ParameterArray(a)
                    Case 2
                        OutputHeaderFile = ParameterArray(a)
                End Select
            Next

            If SourceHeaderFile = Nothing OrElse BackLinkHeaderFile = Nothing OrElse OutputHeaderFile = Nothing Then
                ShowFeedback("Parameters not set", "An Error Has Occurred")
                Return 2
            End If

            SourceImage.HeaderFileName = SourceHeaderFile
            NumCols = SourceImage.NumberColumns
            NumRows = SourceImage.NumberRows

            BackLink.HeaderFileName = BackLinkHeaderFile
            If BackLink.NumberColumns <> NumCols OrElse BackLink.NumberRows <> NumRows Then
                ShowFeedback("Input images must have the same dimensions")
                Return 2
            End If

            Output.HeaderFileName = OutputHeaderFile
            Output.WriteChangesToFile = True
            Output.SetPropertiesUsingAnotherFile(BackLink, "float")
            Output.DataScale = "categorical"
            Output.InitializeGrid(NumCols, NumRows)

            UpdateProgressLabel("Loop 1 of 2:")
            For row = 0 To NumRows - 1
                For col = 0 To NumCols - 1
                    If SourceImage(col, row) > 0 Then
                        Output(col, row) = SourceImage(col, row)
                    End If
                Next
                If CancelBool Then Return 1
                Progress = row / (NumRows - 1) * 100
                worker.ReportProgress(Progress)
            Next

            UpdateProgressLabel("Loop 2 of 2:")
            For row = 0 To NumRows - 1
                For col = 0 To NumCols - 1
                    If BackLink(col, row) <> NoData AndAlso Output(col, row) = NoData Then
                        X = col
                        Y = row
                        flag = True
                        Do
                            FlowDir = BackLink(X, Y)
                            If FlowDir > 0 Then
                                'move x and y accordingly
                                c = Math.Log(FlowDir) / LnOf2
                                X += DX(c)
                                Y += DY(c)
                                If Output(X, Y) > 0 Then
                                    Z = Output(X, Y)
                                    flag = False 'stop the flowpath traverse
                                End If
                            Else
                                Z = Output(X, Y)
                                flag = False 'stop the flowpath traverse
                            End If
                        Loop While flag = True

                        X = col
                        Y = row
                        flag = True
                        Do
                            Output(X, Y) = Z
                            FlowDir = BackLink(X, Y)
                            If FlowDir > 0 Then
                                'move x and y accordingly
                                c = Math.Log(FlowDir) / LnOf2
                                X += DX(c)
                                Y += DY(c)
                                If Output(X, Y) > 0 Then
                                    flag = False 'stop the flowpath traverse
                                End If
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

            SourceImage.ReleaseMemoryResources()
            BackLink.ReleaseMemoryResources()
            Output.MetadataEntries(Output.NumMetadataEntries + 1) = "Created by the CostAllocation tool"
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
