'#################################################################################################################################################################################################################################################
'# Copyright John Lindsay, 2009. Code written by John Lindsay, July 27, 2009.
'# This program is free software: you can redistribute it and/or modify it under the terms of the GNU General Public License as published by the Free Software Foundation, either version 3 of the License, or (at your option) any later version.
'# This program is distributed in the hope that it will be useful, but WITHOUT ANY WARRANTY; without even the implied warranty of MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the GNU General Public License for more details.
'# You should have received a copy of the GNU General Public License along with this program.  If not, see <http://www.gnu.org/licenses/>.
'#################################################################################################################################################################################################################################################
'
'This algorithm can be used to create a cost accumulation  from a series of sources and
'using a cost surface

Imports System
Imports System.Threading
Imports System.ComponentModel
Public Class CostAccumulation
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
            Return "Cost Accumulation"
        End Get
    End Property

    Public ReadOnly Property Description() As String Implements Interfaces.IPlugin.Description
        Get
            Return "Performs cost-distance accumulation on a cost surface and a group of source cells"
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
        Dim CostHeaderFile As String = Nothing
        Dim OutputHeaderFile As String = Nothing
        Dim BackLinkHeaderFile As String = Nothing
        Dim CostSurface As New GeospatialFiles.GATGrid
        Dim SourceImage As New GeospatialFiles.GATGrid
        Dim Output As New GeospatialFiles.GATGrid
        Dim BackLink As New GeospatialFiles.GATGrid
        Dim NumCols, NumRows As Integer
        Dim Z, CostVal, SrcVal As Single
        Dim Progress As Single = 0
        Dim LargeVal As Single = Single.MaxValue - 10000000
        Dim dX() As Integer = {1, 1, 0, -1, -1, -1, 0, 1}
        Dim dY() As Integer = {0, 1, 1, 1, 0, -1, -1, -1}
        Dim DiagDist As Single = Math.Sqrt(2)
        Dim Dist() As Single = {1, DiagDist, 1, DiagDist, 1, DiagDist, 1, DiagDist}
        Dim GridRes As Double
        Dim col, row, a As Integer
        Dim X, Y As Integer
        Dim BackLinkDir() = {32, 64, 128, 1, 2, 4, 8, 16}
        Dim CostAccumVal, Cost1, Cost2, NewCostVal As Single
        Dim DidSomething As Boolean
        Dim LoopNum As Integer = 0
        Dim BlnAnisotropicForce As Boolean
        Dim AnisotropicForceDirection As Single = NoData
        Dim AnisotropicForceStrength As Single = NoData
        Dim AzDir() = {90, 135, 180, 225, 270, 315, 0, 45}

        Try
            For a = 0 To UBound(ParameterArray)
                Select Case a
                    Case 0
                        SourceHeaderFile = ParameterArray(a)
                    Case 1
                        CostHeaderFile = ParameterArray(a)
                    Case 2
                        OutputHeaderFile = ParameterArray(a)
                    Case 3
                        BackLinkHeaderFile = ParameterArray(a)
                    Case 4
                        BlnAnisotropicForce = False
                        If LCase(ParameterArray(a)) <> "not specified" Then
                            BlnAnisotropicForce = True
                            AnisotropicForceDirection = CSng(ParameterArray(a))
                            If AnisotropicForceDirection >= 360 Then AnisotropicForceDirection = 0
                            If AnisotropicForceDirection < 0 Then AnisotropicForceDirection = 0
                        End If
                    Case 5
                        BlnAnisotropicForce = False
                        If LCase(ParameterArray(a)) <> "not specified" Then
                            AnisotropicForceStrength = CSng(ParameterArray(a))
                            If AnisotropicForceStrength = 1 OrElse AnisotropicForceStrength = 0 Then
                                BlnAnisotropicForce = False
                            Else
                                BlnAnisotropicForce = True
                                If AnisotropicForceStrength > 100 Then AnisotropicForceStrength = 100
                                If AnisotropicForceStrength < -100 Then AnisotropicForceStrength = -100
                            End If
                        End If
                End Select
            Next

            If SourceHeaderFile = Nothing OrElse CostHeaderFile = Nothing OrElse OutputHeaderFile = Nothing Then
                ShowFeedback("Parameters not set", "An Error Has Occurred")
                Return 2
            End If

            If AnisotropicForceDirection = NoData OrElse AnisotropicForceStrength = NoData Then
                If BlnAnisotropicForce Then
                    ShowFeedback("Both the Anisotropic Force Direction and Anisotropic Force Strength must be set to valid values to carry out this operation.", "An Error Has Occurred")
                    Return 2
                End If
            End If

            SourceImage.HeaderFileName = SourceHeaderFile
            NumCols = SourceImage.NumberColumns
            NumRows = SourceImage.NumberRows
            GridRes = SourceImage.GridResolution

            CostSurface.HeaderFileName = CostHeaderFile
            If CostSurface.NumberColumns <> NumCols OrElse CostSurface.NumberRows <> NumRows Then
                ShowFeedback("Input images must have the same dimensions")
                Return 2
            End If

            Output.HeaderFileName = OutputHeaderFile
            Output.WriteChangesToFile = True
            Output.SetPropertiesUsingAnotherFile(SourceImage, "float")
            Output.DataScale = "continuous"
            Output.InitializeGrid(NumCols, NumRows, LargeVal)

            BackLink.HeaderFileName = BackLinkHeaderFile
            BackLink.WriteChangesToFile = True
            BackLink.SetPropertiesUsingAnotherFile(SourceImage, "integer")
            BackLink.DataScale = "continuous"
            BackLink.InitializeGrid(NumCols, NumRows)

            UpdateProgressLabel("Calculating Cost Accumulation Surface:")
            For row = 0 To NumRows - 1
                For col = 0 To NumCols - 1
                    CostVal = CostSurface(col, row)
                    If CostVal <> NoData Then
                        SrcVal = SourceImage(col, row)
                        If SrcVal > 0 Then
                            Output(col, row) = 0
                            BackLink(col, row) = 0
                        End If
                    Else
                        Output(col, row) = NoData
                    End If
                Next
                If CancelBool Then Return 1
                Progress = row / (NumRows - 1) * 100
                worker.ReportProgress(Progress)
            Next

            If BlnAnisotropicForce = False Then
                Do
                    DidSomething = False

                    LoopNum += 1
                    UpdateProgressLabel("Loop Number " & CStr(LoopNum) & ":")
                    For row = 0 To NumRows - 1
                        For col = 0 To NumCols - 1
                            CostAccumVal = Output(col, row)
                            If CostAccumVal < LargeVal AndAlso CostAccumVal <> NoData Then
                                Cost1 = CostSurface(col, row)
                                For c = 0 To 3
                                    X = col + dX(c)
                                    Y = row + dY(c)
                                    Cost2 = CostSurface(X, Y)
                                    NewCostVal = CostAccumVal + (Cost1 + Cost2) / 2 * Dist(c)
                                    If NewCostVal < Output(X, Y) Then
                                        Output(X, Y) = NewCostVal
                                        BackLink(X, Y) = BackLinkDir(c)
                                        DidSomething = True
                                    End If
                                Next
                            End If
                        Next
                        If CancelBool Then Return 1
                        Progress = row / (NumRows - 1) * 100
                        If CInt(Progress / 5) = Progress / 5 Then
                            worker.ReportProgress(Progress)
                        End If
                    Next
                    If DidSomething = False Then Exit Do

                    LoopNum += 1
                    UpdateProgressLabel("Loop Number " & CStr(LoopNum) & ":")
                    For row = NumRows - 1 To 0 Step -1
                        For col = NumCols - 1 To 0 Step -1
                            CostAccumVal = Output(col, row)
                            If CostAccumVal < LargeVal AndAlso CostAccumVal <> NoData Then
                                Cost1 = CostSurface(col, row)
                                For c = 4 To 7
                                    X = col + dX(c)
                                    Y = row + dY(c)
                                    Cost2 = CostSurface(X, Y)
                                    NewCostVal = CostAccumVal + (Cost1 + Cost2) / 2 * Dist(c)
                                    If NewCostVal < Output(X, Y) Then
                                        Output(X, Y) = NewCostVal
                                        BackLink(X, Y) = BackLinkDir(c)
                                        DidSomething = True
                                    End If
                                Next
                            End If
                        Next
                        If CancelBool Then Return 1
                        Progress = (NumRows - 1 - row) / (NumRows - 1) * 100
                        If CInt(Progress / 5) = Progress / 5 Then
                            worker.ReportProgress(Progress)
                        End If
                    Next
                    If DidSomething = False Then Exit Do

                    LoopNum += 1
                    UpdateProgressLabel("Loop Number " & CStr(LoopNum) & ":")
                    For col = NumCols - 1 To 0 Step -1
                        For row = NumRows - 1 To 0 Step -1
                            CostAccumVal = Output(col, row)
                            If CostAccumVal < LargeVal AndAlso CostAccumVal <> NoData Then
                                Cost1 = CostSurface(col, row)
                                For c = 3 To 6
                                    X = col + dX(c)
                                    Y = row + dY(c)
                                    Cost2 = CostSurface(X, Y)
                                    NewCostVal = CostAccumVal + (Cost1 + Cost2) / 2 * Dist(c)
                                    If NewCostVal < Output(X, Y) Then
                                        Output(X, Y) = NewCostVal
                                        BackLink(X, Y) = BackLinkDir(c)
                                        DidSomething = True
                                    End If
                                Next
                            End If
                        Next
                        If CancelBool Then Return 1
                        Progress = (NumCols - 1 - col) / (NumCols - 1) * 100
                        If CInt(Progress / 5) = Progress / 5 Then
                            worker.ReportProgress(Progress)
                        End If
                    Next
                    If DidSomething = False Then Exit Do

                    LoopNum += 1
                    UpdateProgressLabel("Loop Number " & CStr(LoopNum) & ":")
                    For row = 0 To NumRows - 1
                        For col = NumCols - 1 To 0 Step -1
                            CostAccumVal = Output(col, row)
                            If CostAccumVal < LargeVal AndAlso CostAccumVal <> NoData Then
                                Cost1 = CostSurface(col, row)
                                For c = 1 To 4
                                    X = col + dX(c)
                                    Y = row + dY(c)
                                    Cost2 = CostSurface(X, Y)
                                    NewCostVal = CostAccumVal + (Cost1 + Cost2) / 2 * Dist(c)
                                    If NewCostVal < Output(X, Y) Then
                                        Output(X, Y) = NewCostVal
                                        BackLink(X, Y) = BackLinkDir(c)
                                        DidSomething = True
                                    End If
                                Next
                            End If
                        Next
                        If CancelBool Then Return 1
                        Progress = row / (NumRows - 1) * 100
                        If CInt(Progress / 5) = Progress / 5 Then
                            worker.ReportProgress(Progress)
                        End If
                    Next
                    If DidSomething = False Then Exit Do

                    LoopNum += 1
                    UpdateProgressLabel("Loop Number " & CStr(LoopNum) & ":")
                    For col = NumCols - 1 To 0 Step -1
                        For row = 0 To NumRows - 1
                            CostAccumVal = Output(col, row)
                            If CostAccumVal < LargeVal AndAlso CostAccumVal <> NoData Then
                                Cost1 = CostSurface(col, row)
                                For c = 2 To 5
                                    X = col + dX(c)
                                    Y = row + dY(c)
                                    Cost2 = CostSurface(X, Y)
                                    NewCostVal = CostAccumVal + (Cost1 + Cost2) / 2 * Dist(c)
                                    If NewCostVal < Output(X, Y) Then
                                        Output(X, Y) = NewCostVal
                                        BackLink(X, Y) = BackLinkDir(c)
                                        DidSomething = True
                                    End If
                                Next
                            End If
                        Next
                        If CancelBool Then Return 1
                        Progress = (NumCols - 1 - col) / (NumCols - 1) * 100
                        If CInt(Progress / 5) = Progress / 5 Then
                            worker.ReportProgress(Progress)
                        End If
                    Next

                Loop While DidSomething

            Else
                Dim Dir As Single
                'convert the azdir to force multipliers
                For c = 0 To 7
                    Dir = Math.Abs(AzDir(c) - AnisotropicForceDirection)
                    If Dir > 180 Then Dir = 360 - Dir
                    AzDir(c) = 1 + (180 - Dir) / 180 * (AnisotropicForceStrength - 1)
                Next
                Do
                    DidSomething = False

                    LoopNum += 1
                    UpdateProgressLabel("Loop Number " & CStr(LoopNum) & ":")
                    For row = 0 To NumRows - 1
                        For col = 0 To NumCols - 1
                            CostAccumVal = Output(col, row)
                            If CostAccumVal < LargeVal AndAlso CostAccumVal <> NoData Then
                                Cost1 = CostSurface(col, row)
                                For c = 0 To 3
                                    X = col + dX(c)
                                    Y = row + dY(c)
                                    Cost2 = CostSurface(X, Y)
                                    NewCostVal = CostAccumVal + ((Cost1 + Cost2) / 2 * Dist(c)) / AzDir(c)
                                    If NewCostVal < Output(X, Y) Then
                                        Output(X, Y) = NewCostVal
                                        BackLink(X, Y) = BackLinkDir(c)
                                        DidSomething = True
                                    End If
                                Next
                            End If
                        Next
                        If CancelBool Then Return 1
                        Progress = row / (NumRows - 1) * 100
                        If CInt(Progress / 5) = Progress / 5 Then
                            worker.ReportProgress(Progress)
                        End If
                    Next
                    If DidSomething = False Then Exit Do

                    LoopNum += 1
                    UpdateProgressLabel("Loop Number " & CStr(LoopNum) & ":")
                    For row = NumRows - 1 To 0 Step -1
                        For col = NumCols - 1 To 0 Step -1
                            CostAccumVal = Output(col, row)
                            If CostAccumVal < LargeVal AndAlso CostAccumVal <> NoData Then
                                Cost1 = CostSurface(col, row)
                                For c = 4 To 7
                                    X = col + dX(c)
                                    Y = row + dY(c)
                                    Cost2 = CostSurface(X, Y)
                                    NewCostVal = CostAccumVal + ((Cost1 + Cost2) / 2 * Dist(c)) / AzDir(c)
                                    If NewCostVal < Output(X, Y) Then
                                        Output(X, Y) = NewCostVal
                                        BackLink(X, Y) = BackLinkDir(c)
                                        DidSomething = True
                                    End If
                                Next
                            End If
                        Next
                        If CancelBool Then Return 1
                        Progress = (NumRows - 1 - row) / (NumRows - 1) * 100
                        If CInt(Progress / 5) = Progress / 5 Then
                            worker.ReportProgress(Progress)
                        End If
                    Next
                    If DidSomething = False Then Exit Do

                    LoopNum += 1
                    UpdateProgressLabel("Loop Number " & CStr(LoopNum) & ":")
                    For col = NumCols - 1 To 0 Step -1
                        For row = NumRows - 1 To 0 Step -1
                            CostAccumVal = Output(col, row)
                            If CostAccumVal < LargeVal AndAlso CostAccumVal <> NoData Then
                                Cost1 = CostSurface(col, row)
                                For c = 3 To 6
                                    X = col + dX(c)
                                    Y = row + dY(c)
                                    Cost2 = CostSurface(X, Y)
                                    NewCostVal = CostAccumVal + ((Cost1 + Cost2) / 2 * Dist(c)) / AzDir(c)
                                    If NewCostVal < Output(X, Y) Then
                                        Output(X, Y) = NewCostVal
                                        BackLink(X, Y) = BackLinkDir(c)
                                        DidSomething = True
                                    End If
                                Next
                            End If
                        Next
                        If CancelBool Then Return 1
                        Progress = (NumCols - 1 - col) / (NumCols - 1) * 100
                        If CInt(Progress / 5) = Progress / 5 Then
                            worker.ReportProgress(Progress)
                        End If
                    Next
                    If DidSomething = False Then Exit Do

                    LoopNum += 1
                    UpdateProgressLabel("Loop Number " & CStr(LoopNum) & ":")
                    For row = 0 To NumRows - 1
                        For col = NumCols - 1 To 0 Step -1
                            CostAccumVal = Output(col, row)
                            If CostAccumVal < LargeVal AndAlso CostAccumVal <> NoData Then
                                Cost1 = CostSurface(col, row)
                                For c = 1 To 4
                                    X = col + dX(c)
                                    Y = row + dY(c)
                                    Cost2 = CostSurface(X, Y)
                                    NewCostVal = CostAccumVal + ((Cost1 + Cost2) / 2 * Dist(c)) / AzDir(c)
                                    If NewCostVal < Output(X, Y) Then
                                        Output(X, Y) = NewCostVal
                                        BackLink(X, Y) = BackLinkDir(c)
                                        DidSomething = True
                                    End If
                                Next
                            End If
                        Next
                        If CancelBool Then Return 1
                        Progress = row / (NumRows - 1) * 100
                        If CInt(Progress / 5) = Progress / 5 Then
                            worker.ReportProgress(Progress)
                        End If
                    Next
                    If DidSomething = False Then Exit Do

                    LoopNum += 1
                    UpdateProgressLabel("Loop Number " & CStr(LoopNum) & ":")
                    For col = NumCols - 1 To 0 Step -1
                        For row = 0 To NumRows - 1
                            CostAccumVal = Output(col, row)
                            If CostAccumVal < LargeVal AndAlso CostAccumVal <> NoData Then
                                Cost1 = CostSurface(col, row)
                                For c = 2 To 5
                                    X = col + dX(c)
                                    Y = row + dY(c)
                                    Cost2 = CostSurface(X, Y)
                                    NewCostVal = CostAccumVal + ((Cost1 + Cost2) / 2 * Dist(c)) / AzDir(c)
                                    If NewCostVal < Output(X, Y) Then
                                        Output(X, Y) = NewCostVal
                                        BackLink(X, Y) = BackLinkDir(c)
                                        DidSomething = True
                                    End If
                                Next
                            End If
                        Next
                        If CancelBool Then Return 1
                        Progress = (NumCols - 1 - col) / (NumCols - 1) * 100
                        If CInt(Progress / 5) = Progress / 5 Then
                            worker.ReportProgress(Progress)
                        End If
                    Next

                Loop While DidSomething

            End If

            LoopNum += 1
            UpdateProgressLabel("Loop Number " & CStr(LoopNum) & ":")
            For row = 0 To NumRows - 1
                For col = 0 To NumCols - 1
                    Z = Output(col, row)
                    If Z <> NoData Then
                        Output(col, row) = Z * GridRes
                    End If
                Next
                If CancelBool Then Return 1
                Progress = row / (NumRows - 1) * 100
                If CInt(Progress / 5) = Progress / 5 Then
                    worker.ReportProgress(Progress)
                End If
            Next

            SourceImage.ReleaseMemoryResources()
            CostSurface.ReleaseMemoryResources()
            Output.MetadataEntries(Output.NumMetadataEntries + 1) = "Created by the CostAccumulation tool"
            Dim dateTimeInfo As DateTime = DateTime.Now
            Output.MetadataEntries(Output.NumMetadataEntries + 1) = "Created on " & dateTimeInfo.ToString("F")
            Output.WriteHeaderFile()
            Output.ReleaseMemoryResources()
            BackLink.MetadataEntries(BackLink.NumMetadataEntries + 1) = "Created by the CostAccumulation tool"
            BackLink.MetadataEntries(BackLink.NumMetadataEntries + 1) = "Created on " & dateTimeInfo.ToString("F")
            BackLink.WriteHeaderFile()
            BackLink.ReleaseMemoryResources()

            worker.ReportProgress(0)

            Return OutputHeaderFile

        Catch e As Exception
            ShowFeedback(e.Message)
            Return 2
        End Try
    End Function

   End Class
