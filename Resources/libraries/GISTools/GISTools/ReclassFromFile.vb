'#################################################################################################################################################################################################################################################
'# Copyright John Lindsay, 2009.
'# This program is free software: you can redistribute it and/or modify it under the terms of the GNU General Public License as published by the Free Software Foundation, either version 3 of the License, or (at your option) any later version.
'# This program is distributed in the hope that it will be useful, but WITHOUT ANY WARRANTY; without even the implied warranty of MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the GNU General Public License for more details.
'# You should have received a copy of the GNU General Public License along with this program.  If not, see <http://www.gnu.org/licenses/>.
'#################################################################################################################################################################################################################################################

Imports System
Imports System.IO
Imports System.Threading
Imports System.ComponentModel
Imports Microsoft.VisualBasic.FileIO

Public Class ReclassFromFile
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
            Return "Reclass From ASCII Text File"
        End Get
    End Property

    Public ReadOnly Property Description() As String Implements Interfaces.IPlugin.Description
        Get
            Return "This tool assigns grid cells in a raster image new values based on ranges defined in an ASCII text file"
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
        Dim ReclassFile As String = Nothing
        Dim Image As New GeospatialFiles.GATGrid
        Dim Output As New GeospatialFiles.GATGrid
        Dim NumCols, NumRows As Integer
        Dim Z As Single
        Dim Progress As Single = 0
        Dim a, b, i As Integer
        Dim NumReclassRanges As Integer
        Dim NumReclassRangesMinusOne As Integer
        Dim ReclassRangeStr() As String
        Dim ReclassRange(0, 0) As Single
        Dim BlnAssignMode As Boolean = False

        Try
            For a = 0 To UBound(ParameterArray)
                Select Case a
                    Case 0
                        HeaderFile = ParameterArray(a)
                    Case 1
                        OutputHeaderFile = ParameterArray(a)
                    Case 2
                        ReclassFile = ParameterArray(a)
                End Select
            Next

            If HeaderFile Is Nothing OrElse OutputHeaderFile Is Nothing OrElse ReclassFile Is Nothing Then
                ShowFeedback("Parameters not set", "An Error Has Occurred")
                Return 2
            End If

            'see if the reclass file exists
            If File.Exists(ReclassFile) = False Then
                ShowFeedback("Reclass file does not exist", "An Error Has Occurred")
                Return 2
            End If
            'determine the delimiter, find out how many lines there are, and whether it is in 
            'assignmode, and read the data into the reclassrange array
            Dim delimiter As String = Nothing
            Dim TextLine As String
            NumReclassRanges = 0
            Using sr As StreamReader = New StreamReader(ReclassFile)
                Do
                    TextLine = sr.ReadLine()
                    If TextLine IsNot Nothing Then
                        NumReclassRanges += 1
                        If NumReclassRanges = 1 Then 'just evaluate the delimiter of the first line
                            'delimiter?
                            If InStr(TextLine, ",") > 0 Then
                                delimiter = ","
                            ElseIf InStr(TextLine, " ") > 0 Then
                                delimiter = " "
                            ElseIf InStr(TextLine, vbTab) > 0 Then
                                delimiter = vbTab
                            Else
                                ShowFeedback("File delimiter could not be identified", "An Error Has Occurred")
                                Return 2
                            End If

                            'assign mode?
                            ReclassRangeStr = Split(TextLine, delimiter)
                            If UBound(ReclassRangeStr) = 1 Then
                                BlnAssignMode = True
                            Else
                                BlnAssignMode = False
                            End If
                        End If
                        ReDim Preserve ReclassRange(2, NumReclassRanges - 1)
                        ReclassRangeStr = Split(TextLine, delimiter)
                        ReclassRange(0, NumReclassRanges - 1) = CSng(ReclassRangeStr(0))
                        ReclassRange(1, NumReclassRanges - 1) = CSng(ReclassRangeStr(1))
                        If BlnAssignMode = False Then
                            ReclassRange(2, NumReclassRanges - 1) = CSng(ReclassRangeStr(2))
                        End If
                    End If
                Loop Until TextLine Is Nothing
            End Using
            NumReclassRangesMinusOne = NumReclassRanges - 1

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
                            If Z <> NoData Then
                                For i = 0 To NumReclassRangesMinusOne
                                    If Z = ReclassRange(1, i) Then
                                        Output(a, b) = ReclassRange(0, i)
                                        Exit For
                                    End If
                                    If i = NumReclassRangesMinusOne Then 'z was not in the reclass ranges; output value equals input value
                                        Output(a, b) = Z
                                    End If
                                Next

                            Else
                                Output(a, b) = NoData
                            End If
                        Next
                        If CancelBool Then Return 1
                        Progress = b / (NumRows - 1) * 100
                        worker.ReportProgress(Progress)
                    Next

                Case False
                    For b = 0 To NumRows - 1
                        For a = 0 To NumCols - 1
                            Z = Image(a, b)
                            If Z <> NoData Then
                                For i = 0 To NumReclassRangesMinusOne
                                    If Z >= ReclassRange(1, i) AndAlso Z < ReclassRange(2, i) Then
                                        Output(a, b) = ReclassRange(0, i)
                                        Exit For
                                    End If
                                    If i = NumReclassRangesMinusOne Then 'z was not in the reclass ranges; output value equals input value
                                        Output(a, b) = Z
                                    End If
                                Next

                            Else
                                Output(a, b) = NoData
                            End If
                        Next
                        If CancelBool Then Return 1
                        Progress = b / (NumRows - 1) * 100
                        worker.ReportProgress(Progress)
                    Next

            End Select


            Image.ReleaseMemoryResources()
            Output.MetadataEntries(Output.NumMetadataEntries + 1) = "Created by the ReclassFromFile tool"
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
