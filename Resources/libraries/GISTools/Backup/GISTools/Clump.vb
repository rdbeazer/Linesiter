'#################################################################################################################################################################################################################################################
'# Copyright John Lindsay, 2009.
'# This program is free software: you can redistribute it and/or modify it under the terms of the GNU General Public License as published by the Free Software Foundation, either version 3 of the License, or (at your option) any later version.
'# This program is distributed in the hope that it will be useful, but WITHOUT ANY WARRANTY; without even the implied warranty of MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the GNU General Public License for more details.
'# You should have received a copy of the GNU General Public License along with this program.  If not, see <http://www.gnu.org/licenses/>.
'#################################################################################################################################################################################################################################################

Imports System
Imports System.Threading
Imports System.ComponentModel
Public Class Clump
    Implements Interfaces.IPlugin

    Private objHost As Interfaces.IHost
    Dim CurrentPatchNumber As Single = 0
    Dim CurrentImageValue As Single = 0
    Dim Image As New GeospatialFiles.GATGrid
    Dim Output As New GeospatialFiles.GATGrid
    Dim MaxDepth As Integer = 1000
    Dim Depth As Integer = 0
    Dim DX() As Integer = {1, 1, 1, 0, -1, -1, -1, 0}
    Dim DY() As Integer = {-1, 0, 1, 1, 1, 0, -1, -1}
    Dim NumScanCells As Integer = 7
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
            Return "Clump"
        End Get
    End Property

    Public ReadOnly Property Description() As String Implements Interfaces.IPlugin.Description
        Get
            Return "Recategorizes data in a raster map by grouping cells that form physically discrete areas into unique categories"
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
        'Written by John Lindsay, June 4, 2009
        Dim HeaderFile As String = Nothing
        Dim OutputHeaderFile As String = Nothing
        Dim NumCols, NumRows As Integer
        Dim MaxPatchValue As Single = -1
        Dim Z, Z1, Z2 As Single
        Dim Progress As Single = 0
        Dim X, Y As Integer
        Dim BlnFoundNeighbour As Boolean
        Dim BlnIncludeDiagNeighbour As Boolean = False
        Dim BlnTreatZerosAsBackground As Boolean = False

        Try
            For a As Integer = 0 To UBound(ParameterArray)
                Select Case a
                    Case 0
                        HeaderFile = ParameterArray(a)
                    Case 1
                        OutputHeaderFile = ParameterArray(a)
                    Case 2
                        BlnIncludeDiagNeighbour = CBool(ParameterArray(a))
                        If BlnIncludeDiagNeighbour = False Then
                            ReDim DX(3)
                            DX(0) = 0
                            DX(1) = 1
                            DX(2) = 0
                            DX(3) = -1
                            ReDim DY(3)
                            DY(0) = -1
                            DY(1) = 0
                            DY(2) = 1
                            DY(3) = 0
                            NumScanCells = 3
                        End If
                    Case 3
                        BlnTreatZerosAsBackground = CBool(ParameterArray(a))
                End Select
            Next

            If HeaderFile = Nothing OrElse OutputHeaderFile = Nothing Then
                ShowFeedback("Parameters not set", "An Error Has Occurred")
                Return 2
            End If

            Image.HeaderFileName = HeaderFile
            NumCols = Image.NumberColumns
            NumRows = Image.NumberRows

            Output.HeaderFileName = OutputHeaderFile
            Output.WriteChangesToFile = True
            Output.SetPropertiesUsingAnotherFile(Image, "integer")
            Output.DataScale = "categorical"
            Output.InitializeGrid(NumCols, NumRows, -1)

            If BlnTreatZerosAsBackground Then
                For b As Integer = 0 To NumRows - 1
                    For a = 0 To NumCols - 1
                        If Image(a, b) = 0 Then Output(a, b) = 0
                    Next
                Next
                If Output(0, 0) = -1 Then Output(0, 0) = 1
            Else
                Output(0, 0) = 0
            End If

            UpdateProgressLabel("Loop 1 of 4:")
            For b As Integer = 0 To NumRows - 1
                For a = 0 To NumCols - 1
                    Z = Image(a, b)
                    If Z <> NoData Then
                        BlnFoundNeighbour = False
                        If Output(a, b) < 0 Then
                            'see if any neighbour has the same value in the input image
                            BlnFoundNeighbour = False
                            For c = 0 To NumScanCells
                                X = a + DX(c)
                                Y = b + DY(c)
                                CurrentPatchNumber = Output(X, Y)
                                If CurrentPatchNumber > 0 AndAlso Image(X, Y) = Z Then
                                    'cell is neighbouring a cell with the same value in image that
                                    'has already been assigned a patch value
                                    Output(a, b) = CurrentPatchNumber
                                    BlnFoundNeighbour = True
                                    Exit For
                                End If
                            Next
                            If BlnFoundNeighbour = False Then
                                'no neighbouring cell has the same value in Image and has 
                                'already been assigned a value
                                MaxPatchValue += 1
                                CurrentPatchNumber = MaxPatchValue
                                Output(a, b) = CurrentPatchNumber
                            End If
                            'recursively scan all connected cells of equal value in Image
                            CurrentImageValue = Z
                            Depth = 0
                            ScanConnectedCells(a, b)
                        End If
                    Else
                        Output(a, b) = NoData
                    End If
                Next
                If CancelBool Then Return 1
                Progress = b / (NumRows - 1) * 100
                worker.ReportProgress(Progress)
            Next


            UpdateProgressLabel("Loop 2 of 4:")
            Dim PatchArray(MaxPatchValue, 2) As Single

            For a = 0 To MaxPatchValue
                PatchArray(a, 2) = 99999999999
            Next

            Dim CellScanNumber As Long = 0

            For b As Integer = 0 To NumRows - 1
                For a = 0 To NumCols - 1
                    Z = Output(a, b)
                    If Z <> NoData Then
                        PatchArray(Z, 0) += 1
                        PatchArray(Z, 1) = Z
                        CellScanNumber = b * NumCols + NumCols
                        If PatchArray(Z, 2) > CellScanNumber Then
                            PatchArray(Z, 2) = CellScanNumber
                        End If
                    End If
                Next
                If CancelBool Then Return 1
                Progress = b / (NumRows - 1) * 100
                worker.ReportProgress(Progress)
            Next

            UpdateProgressLabel("Loop 3 of 4:")
            For b As Integer = 0 To NumRows - 1
                For a = 0 To NumCols - 1
                    Z = Image(a, b)
                    If Z <> NoData Then
                        CurrentPatchNumber = Output(a, b)
                        'see if there is a neighbouring cell with the same value in Image but a 
                        'different patch value
                        For c = 0 To NumScanCells
                            X = a + DX(c)
                            Y = b + DY(c)
                            Z1 = Output(X, Y)
                            If Image(X, Y) = Z AndAlso CurrentPatchNumber <> Z1 AndAlso _
                             PatchArray(CurrentPatchNumber, 1) <> PatchArray(Z1, 1) Then
                                'cell is neighbouring a cell with the same value in Image but
                                'a different value in Output

                                'which one has the cell closest to the top lefthand corner?
                                If PatchArray(CurrentPatchNumber, 2) < PatchArray(Z1, 2) Then
                                    Z2 = PatchArray(Z1, 1)
                                    PatchArray(Z1, 1) = PatchArray(CurrentPatchNumber, 1)
                                    For d As Integer = 0 To MaxPatchValue
                                        If PatchArray(d, 1) = Z2 Then
                                            PatchArray(d, 1) = PatchArray(CurrentPatchNumber, 1)
                                        End If
                                    Next
                                Else
                                    Z2 = PatchArray(CurrentPatchNumber, 1)
                                    PatchArray(CurrentPatchNumber, 1) = PatchArray(Z1, 1)
                                    For d As Integer = 0 To MaxPatchValue
                                        If PatchArray(d, 1) = Z2 Then
                                            PatchArray(d, 1) = PatchArray(Z1, 1)
                                        End If
                                    Next
                                End If
                                Output(a, b) = CurrentPatchNumber
                                BlnFoundNeighbour = True
                                Exit For
                            End If
                        Next
                    End If
                Next
                If CancelBool Then Return 1
                Progress = b / (NumRows - 1) * 100
                worker.ReportProgress(Progress)
            Next

            CurrentPatchNumber = 0
            For a = 0 To MaxPatchValue
                Z = PatchArray(a, 1)
                If Z > CurrentPatchNumber Then
                    PatchArray(a, 1) = CurrentPatchNumber
                    For b As Integer = 0 To MaxPatchValue
                        If PatchArray(b, 1) = Z Then
                            PatchArray(b, 1) = CurrentPatchNumber
                        End If
                    Next
                    CurrentPatchNumber += 1
                End If
            Next

            UpdateProgressLabel("Loop 4 of 4:")
            For b As Integer = 0 To NumRows - 1
                For a = 0 To NumCols - 1
                    Z = Output(a, b)
                    If Z <> NoData Then
                        Output(a, b) = PatchArray(Z, 1)
                    End If
                Next
                If CancelBool Then Return 1
                Progress = b / (NumRows - 1) * 100
                worker.ReportProgress(Progress)
            Next


            Image.ReleaseMemoryResources()
            Output.MetadataEntries(Output.NumMetadataEntries + 1) = "Created by the Clump tool"
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

    Private Sub ScanConnectedCells(ByRef col As Integer, ByRef row As Integer)
        Depth += 1
        Dim X, Y As Integer
        If Depth < MaxDepth Then
            For c As Integer = 0 To NumScanCells
                X = col + DX(c)
                Y = row + DY(c)
                If Output(X, Y) < 0 AndAlso Image(X, Y) = CurrentImageValue Then
                    'cell has not been assigned a value and has the same value in Image
                    Output(X, Y) = CurrentPatchNumber
                    ScanConnectedCells(X, Y)
                End If
            Next
        End If

        Depth -= 1

    End Sub
End Class
