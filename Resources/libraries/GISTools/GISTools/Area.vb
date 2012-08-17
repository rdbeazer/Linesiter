'#################################################################################################################################################################################################################################################
'# Copyright John Lindsay, 2009. Code written by John Lindsay, July 24, 2009.
'# This program is free software: you can redistribute it and/or modify it under the terms of the GNU General Public License as published by the Free Software Foundation, either version 3 of the License, or (at your option) any later version.
'# This program is distributed in the hope that it will be useful, but WITHOUT ANY WARRANTY; without even the implied warranty of MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the GNU General Public License for more details.
'# You should have received a copy of the GNU General Public License along with this program.  If not, see <http://www.gnu.org/licenses/>.
'#################################################################################################################################################################################################################################################

'This tool calculates the area of each category or polygon in an input image

Imports System
Imports System.IO
Imports System.Threading
Imports System.ComponentModel

Public Class Area
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
            Return "Area"
        End Get
    End Property

    Public ReadOnly Property Description() As String Implements Interfaces.IPlugin.Description
        Get
            Return "Calculates the area of each category or polygon in an input image"
        End Get
    End Property

    Public ReadOnly Property ToolboxInfo() As String Implements Interfaces.IPlugin.ToolboxName
        Get
            Return "GISTools"
        End Get
    End Property

    Private Sub ShowFeedback(ByVal message As String, Optional ByVal caption As String = "GAT Message")
        objHost.ShowFeedback(message, caption)
    End Sub

    Private Sub UpdateProgressLabel(ByVal label As String)
        objHost.ProgressBarLabel(label)
    End Sub

    Public Function Execute(ByVal ParameterArray() As String, ByVal worker As BackgroundWorker) As Object Implements Interfaces.IPlugin.Execute
        Dim HeaderFile As String = Nothing
        Dim OutputHeaderFile As String = Nothing
        Dim Image As New GeospatialFiles.GATGrid
        Dim Output As New GeospatialFiles.GATGrid
        Dim NumCols, NumRows As Integer
        Dim Z As Single
        Dim Progress As Single = 0
        Dim a, b, i As Integer
        Dim NumClasses As Integer
        Dim MinClass, MaxClass As Integer
        Dim ClassArea() As Double
        Dim BlnImageOutput As Boolean = False
        Dim BlnTextOutput As Boolean = False
        Dim BlnOutputUnitsGridCells As Boolean = False
        Dim GridRes As Double
        Dim GridArea As Double

        Try
            For a = 0 To UBound(ParameterArray)
                Select Case a
                    Case 0
                        HeaderFile = ParameterArray(a)
                    Case 1
                        OutputHeaderFile = ParameterArray(a)
                        If LCase(OutputHeaderFile) <> "not specified" Then
                            BlnImageOutput = True
                        End If
                    Case 2
                        BlnTextOutput = CBool(ParameterArray(a))
                    Case 3
                        If LCase(ParameterArray(a)) = "grid cells" Then
                            BlnOutputUnitsGridCells = True
                        Else
                            BlnOutputUnitsGridCells = False
                        End If
                End Select
            Next

            If BlnImageOutput = False AndAlso BlnTextOutput = False Then
                ShowFeedback("You must select either an image or text output or both.", "An Error Has Occurred")
                Return 2
            End If

            If HeaderFile = Nothing OrElse OutputHeaderFile = Nothing Then
                ShowFeedback("Parameters not set", "An Error Has Occurred")
                Return 2
            End If

            Image.HeaderFileName = HeaderFile
            NumCols = Image.NumberColumns
            NumRows = Image.NumberRows
            GridRes = Image.GridResolution
            GridArea = GridRes * GridRes

            If LCase(Image.DataScale) <> "categorical" Then
                ShowFeedback("The input image must be categorical", "An Error Has Occurred")
                Return 2
            End If
            MinClass = Image.Minimum
            MaxClass = Image.Maximum
            NumClasses = MaxClass - MinClass + 1

            ReDim ClassArea(NumClasses - 1)


            Output.HeaderFileName = OutputHeaderFile
            Output.WriteChangesToFile = True
            Output.SetPropertiesUsingAnotherFile(Image, "float")
            Output.DataScale = "continuous"
            Output.InitializeGrid(NumCols, NumRows)

            For b = 0 To NumRows - 1
                For a = 0 To NumCols - 1
                    Z = Image(a, b)
                    If Z <> NoData Then
                        i = Z - MinClass
                        ClassArea(i) += 1
                    End If
                Next
                If CancelBool Then Return 1
                Progress = b / (NumRows - 1) * 100
                worker.ReportProgress(Progress)
            Next

            If BlnOutputUnitsGridCells = False Then 'convert the areas to map units
                For i = 0 To NumClasses - 1
                    ClassArea(i) = ClassArea(i) * GridArea
                Next
            End If

            If BlnImageOutput Then
                For b = 0 To NumRows - 1
                    For a = 0 To NumCols - 1
                        Z = Image(a, b)
                        If Z <> NoData Then
                            i = Z - MinClass
                            Output(a, b) = ClassArea(i)
                        Else
                            Output(a, b) = NoData
                        End If
                    Next
                    If CancelBool Then Return 1
                    Progress = b / (NumRows - 1) * 100
                    worker.ReportProgress(Progress)
                Next

                Output.MetadataEntries(Output.NumMetadataEntries + 1) = "Created by the Area tool"
                Dim dateTimeInfo As DateTime = DateTime.Now
                Output.MetadataEntries(Output.NumMetadataEntries + 1) = "Created on " & dateTimeInfo.ToString("F")
                Output.WriteHeaderFile()
                Output.ReleaseMemoryResources()

            End If

            Image.ReleaseMemoryResources()

            worker.ReportProgress(0)

            If BlnTextOutput Then
                Dim retstr As String = "Area Analysis" & vbNewLine

                For a = 0 To NumClasses - 1
                    If ClassArea(a) > 0 Then
                        retstr = retstr & vbNewLine & CStr(MinClass + a) & vbTab & Format(ClassArea(a), "############0.00")
                    End If
                Next

                Return retstr
            End If

            Return OutputHeaderFile

        Catch e As Exception
            ShowFeedback(e.Message)
            Return 2
        End Try
    End Function
End Class
