'#################################################################################################################################################################################################################################################
'# Copyright John Lindsay, 2009.
'# This program is free software: you can redistribute it and/or modify it under the terms of the GNU General Public License as published by the Free Software Foundation, either version 3 of the License, or (at your option) any later version.
'# This program is distributed in the hope that it will be useful, but WITHOUT ANY WARRANTY; without even the implied warranty of MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the GNU General Public License for more details.
'# You should have received a copy of the GNU General Public License along with this program.  If not, see <http://www.gnu.org/licenses/>.
'#################################################################################################################################################################################################################################################

Imports System
Imports System.Threading
Imports System.ComponentModel
Public Class Reclass
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
            Return "Reclass (user-defined classes)"
        End Get
    End Property

    Public ReadOnly Property Description() As String Implements Interfaces.IPlugin.Description
        Get
            Return "Assigns grid cells in a raster image new values based on user-defined ranges"
        End Get
    End Property

    Public ReadOnly Property ToolboxInfo() As String Implements Interfaces.IPlugin.ToolboxName
        Get
            Return "ReclassTools"
        End Get
    End Property

    Private Sub ShowFeedback(ByVal message As String, Optional ByVal caption As String = "GAT Message")
        objHost.ShowFeedback(message, caption)
    End Sub

    Private Sub UpdateProgressLabel(ByVal label As String)
        objHost.ProgressBarLabel(label)
    End Sub

    Public Function Execute(ByVal ParameterArray() As String, ByVal worker As BackgroundWorker) As Object Implements Interfaces.IPlugin.Execute
        'Written by John Lindsay, June 24, 2009

        Dim HeaderFile As String = Nothing
        Dim OutputHeaderFile As String = Nothing
        Dim Image As New GeospatialFiles.GATGrid
        Dim Output As New GeospatialFiles.GATGrid
        Dim NumCols, NumRows As Integer
        Dim Z As Single
        Dim Progress As Single = 0
        Dim a, b, i As Integer
        Dim NumReclassRanges As Integer
        Dim NumReclassRangesMinusOne As Integer
        Dim ReclassRangeStr() As String
        Dim ReclassRange(,) As Single
        Dim BlnAssignMode As Boolean = False
        ReDim ReclassRangeStr(-1)

        Try
            For a = 0 To UBound(ParameterArray)
                Select Case a
                    Case 0
                        HeaderFile = ParameterArray(a)
                    Case 1
                        OutputHeaderFile = ParameterArray(a)
                    Case 2
                        ReclassRangeStr = Split(Trim(ParameterArray(a)), vbTab)
                        If ReclassRangeStr(2) = Nothing Then BlnAssignMode = True
                End Select
            Next

            If HeaderFile = Nothing OrElse OutputHeaderFile = Nothing Then
                ShowFeedback("Parameters not set", "An Error Has Occurred")
                Return 2
            End If

            'How many rows should there be in the ReclassRange array?
            'There are three numbers in each range: New Value, From Value, To Just Less Than Value
            NumReclassRanges = UBound(ReclassRangeStr) \ 3 + 1
            NumReclassRangesMinusOne = NumReclassRanges - 1
            ReDim ReclassRange(2, NumReclassRanges - 1)
            i = 0
            For b = 0 To UBound(ReclassRangeStr)
                If ReclassRangeStr(b) <> Nothing Then
                    If LCase(ReclassRangeStr(b)) <> "nodata" Then
                        ReclassRange(i, b \ 3) = CSng(ReclassRangeStr(b))
                    Else
                        ReclassRange(i, b \ 3) = NoData
                    End If
                Else
                    ReclassRange(i, b \ 3) = 0
                End If
                i += 1
                If i = 3 Then i = 0
            Next

            If NumReclassRanges = 0 Then
                ShowFeedback("There is an error with the reclass ranges.", "An Error Has Occurred")
                Return 2
            End If

            Image.HeaderFileName = HeaderFile
            NumCols = Image.NumberColumns
            NumRows = Image.NumberRows

            Output.HeaderFileName = OutputHeaderFile
            Output.WriteChangesToFile = True
            Output.SetPropertiesUsingAnotherFile(Image, "float")
            Output.InitializeGrid(NumCols, NumRows)

            Select Case BlnAssignMode
                Case True
                    For b = 0 To NumRows - 1
                        For a = 0 To NumCols - 1
                            Z = Image(a, b)
                            For i = 0 To NumReclassRangesMinusOne
                                If Z = ReclassRange(1, i) Then
                                    Output(a, b) = ReclassRange(0, i)
                                    Exit For
                                End If
                                If i = NumReclassRangesMinusOne Then 'z was not in the reclass ranges; output value equals input value
                                    Output(a, b) = Z
                                End If
                            Next
                        Next
                        If CancelBool Then Return 1
                        Progress = b / (NumRows - 1) * 100
                        worker.ReportProgress(Progress)
                    Next

                Case False
                    For b = 0 To NumRows - 1
                        For a = 0 To NumCols - 1
                            Z = Image(a, b)
                            For i = 0 To NumReclassRangesMinusOne
                                If Z >= ReclassRange(1, i) AndAlso Z < ReclassRange(2, i) Then
                                    Output(a, b) = ReclassRange(0, i)
                                    Exit For
                                End If
                                If i = NumReclassRangesMinusOne Then 'z was not in the reclass ranges; output value equals input value
                                    Output(a, b) = Z
                                End If
                            Next

                        Next
                        If CancelBool Then Return 1
                        Progress = b / (NumRows - 1) * 100
                        worker.ReportProgress(Progress)
                    Next

            End Select


            Image.ReleaseMemoryResources()
            Output.MetadataEntries(Output.NumMetadataEntries + 1) = "Created by the Reclass tool"
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
