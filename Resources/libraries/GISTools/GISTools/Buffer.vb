'#################################################################################################################################################################################################################################################
'# Copyright John Lindsay, 2009. Code written by John Lindsay, July 24, 2009.
'# This program is free software: you can redistribute it and/or modify it under the terms of the GNU General Public License as published by the Free Software Foundation, either version 3 of the License, or (at your option) any later version.
'# This program is distributed in the hope that it will be useful, but WITHOUT ANY WARRANTY; without even the implied warranty of MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the GNU General Public License for more details.
'# You should have received a copy of the GNU General Public License along with this program.  If not, see <http://www.gnu.org/licenses/>.
'#################################################################################################################################################################################################################################################

'Written by John Lindsay, June 24, 2009
'This algorithm can be used to create a buffer around non-background grid cells in an input image.
'Buffer distances are based on the efficient distance transform of Shih and Wu (2004), CV&IU.

Imports System
Imports System.Threading
Imports System.ComponentModel
Public Class Buffer
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
            Return "Buffer"
        End Get
    End Property

    Public ReadOnly Property Description() As String Implements Interfaces.IPlugin.Description
        Get
            Return "Maps a distance-based buffer around each non-background grid cell in an input image"
        End Get
    End Property

    Public ReadOnly Property ToolboxInfo() As String Implements Interfaces.IPlugin.ToolboxName
        Get
            Return "DistanceTools"
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
        Dim Allocation As New GeospatialFiles.GATGrid
        Dim Rx As New GeospatialFiles.GATGrid
        Dim Ry As New GeospatialFiles.GATGrid
        Dim NumCols, NumRows As Integer
        Dim Z, Z2, Zmin, AllocatedVal As Single
        Dim Progress As Single = 0
        Dim X, Y, a, b, i As Integer
        Dim h As Integer
        Dim WhichCell As Integer
        Dim InfVal As Single = Single.MaxValue - 10000
        Dim dX() As Integer = {-1, -1, 0, 1, 1, 1, 0, -1}
        Dim dY() As Integer = {0, -1, -1, -1, 0, 1, 1, 1}
        Dim Gx() As Integer = {1, 1, 0, 1, 1, 1, 0, 1}
        Dim Gy() As Integer = {0, 1, 1, 1, 0, 1, 1, 1}
        Dim GridRes As Double
        Dim BufferSize As Single = 0
        Dim BufferInGridCells As Boolean = False

        Try
            For a = 0 To UBound(ParameterArray)
                Select Case a
                    Case 0
                        HeaderFile = ParameterArray(a)
                    Case 1
                        OutputHeaderFile = ParameterArray(a)
                    Case 2
                        BufferSize = CSng(ParameterArray(a))
                    Case 3
                        If InStr(LCase(ParameterArray(a)), "cells") > 0 Then
                            BufferInGridCells = True
                        End If
                End Select
            Next

            If HeaderFile = Nothing OrElse OutputHeaderFile = Nothing Then
                ShowFeedback("Parameters not set", "An Error Has Occurred")
                Return 2
            End If

            Image.HeaderFileName = HeaderFile
            NumCols = Image.NumberColumns
            NumRows = Image.NumberRows
            GridRes = Image.GridResolution

            Output.HeaderFileName = OutputHeaderFile
            Output.WriteChangesToFile = True
            Output.SetPropertiesUsingAnotherFile(Image, "float")
            Output.DataScale = "categorical"
            Output.InitializeGrid(NumCols, NumRows, InfVal)

            Allocation.HeaderFileName = Replace(OutputHeaderFile, ".dep", "temp3.dep")
            Allocation.WriteChangesToFile = True
            Allocation.SetPropertiesUsingAnotherFile(Image, "float")
            Allocation.DataScale = "continuous"
            Allocation.InitializeGrid(NumCols, NumRows, NoData)
            Allocation.IsTemporaryFile = True

            Rx.HeaderFileName = Replace(OutputHeaderFile, ".dep", "temp1.dep")
            Rx.WriteChangesToFile = True
            Rx.SetPropertiesUsingAnotherFile(Image, "float")
            Rx.InitializeGrid(NumCols, NumRows, 0)
            Rx.IsTemporaryFile = True

            Ry.HeaderFileName = Replace(OutputHeaderFile, ".dep", "temp2.dep")
            Ry.WriteChangesToFile = True
            Ry.SetPropertiesUsingAnotherFile(Image, "float")
            Ry.InitializeGrid(NumCols, NumRows, 0)
            Ry.IsTemporaryFile = True

            UpdateProgressLabel("Calculating Distance Surface:")
            For b = 0 To NumRows - 1
                For a = 0 To NumCols - 1
                    If Image(a, b) <> 0 Then Output(a, b) = 0
                    Allocation(a, b) = Image(a, b)
                Next
                If CancelBool Then Return 1
                Progress = b / (NumRows - 1) * 100
                worker.ReportProgress(Progress)
            Next

            For b = 0 To NumRows - 1
                For a = 0 To NumCols - 1
                    Z = Output(a, b)
                    If Z <> 0 Then
                        Zmin = InfVal
                        WhichCell = -1
                        AllocatedVal = NoData
                        For i = 0 To 3
                            X = a + dX(i)
                            Y = b + dY(i)
                            Z2 = Output(X, Y)
                            If Z2 <> NoData Then
                                Select Case i
                                    Case 1, 3
                                        h = 2 * (Rx(X, Y) + Ry(X, Y) + 1)
                                    Case 0
                                        h = 2 * Rx(X, Y) + 1
                                    Case 2
                                        h = 2 * Ry(X, Y) + 1
                                End Select
                                Z2 += h
                                If Z2 < Zmin Then
                                    Zmin = Z2
                                    WhichCell = i
                                End If
                            End If
                        Next
                        If Zmin < Z Then
                            Output(a, b) = Zmin
                            X = a + dX(WhichCell)
                            Y = b + dY(WhichCell)
                            Rx(a, b) = Rx(X, Y) + Gx(WhichCell)
                            Ry(a, b) = Ry(X, Y) + Gy(WhichCell)
                            Allocation(a, b) = Allocation(X, Y)
                        End If
                    End If
                Next
                If CancelBool Then Return 1
                Progress = b / (NumRows - 1) * 100
                worker.ReportProgress(Progress)
            Next


            For b = NumRows - 1 To 0 Step -1
                For a = NumCols - 1 To 0 Step -1
                    Z = Output(a, b)
                    If Z <> 0 Then
                        Zmin = InfVal
                        WhichCell = -1
                        For i = 4 To 7
                            X = a + dX(i)
                            Y = b + dY(i)
                            Z2 = Output(X, Y)
                            If Z2 <> NoData Then
                                Select Case i
                                    Case 5, 7
                                        h = 2 * (Rx(X, Y) + Ry(X, Y) + 1)
                                    Case 4
                                        h = 2 * Rx(X, Y) + 1
                                    Case 6
                                        h = 2 * Ry(X, Y) + 1
                                End Select
                                Z2 += h
                                If Z2 < Zmin Then
                                    Zmin = Z2
                                    WhichCell = i
                                End If
                            End If
                        Next
                        If Zmin < Z Then
                            Output(a, b) = Zmin
                            X = a + dX(WhichCell)
                            Y = b + dY(WhichCell)
                            Rx(a, b) = Rx(X, Y) + Gx(WhichCell)
                            Ry(a, b) = Ry(X, Y) + Gy(WhichCell)
                            Allocation(a, b) = Allocation(X, Y)
                        End If
                    End If
                Next
                If CancelBool Then Return 1
                Progress = (NumRows - 1 - b) / (NumRows - 1) * 100
                worker.ReportProgress(Progress)
            Next

            UpdateProgressLabel("Performing Buffering:")
            'the buffer size should be expressed in squared grid cells (the units of the distance map)
            If BufferInGridCells = False Then
                BufferSize = (BufferSize / GridRes) ^ 2
            Else
                BufferSize = BufferSize ^ 2
            End If

            For b = 0 To NumRows - 1
                For a = 0 To NumCols
                    If Image(a, b) <> NoData Then
                        Z = Output(a, b)
                        If Z <= BufferSize Then
                            Output(a, b) = Allocation(a, b)
                        Else
                            Output(a, b) = NoData
                        End If
                    Else
                        Output(a, b) = NoData
                    End If
                Next
                If CancelBool Then Return 1
                Progress = b / (NumRows - 1) * 100
                worker.ReportProgress(Progress)
            Next

            Image.ReleaseMemoryResources()
            Rx.ReleaseMemoryResources()
            Ry.ReleaseMemoryResources()
            Allocation.ReleaseMemoryResources()
            Output.MetadataEntries(Output.NumMetadataEntries + 1) = "Created by the Buffer tool"
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
