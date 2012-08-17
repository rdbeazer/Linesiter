'#################################################################################################################################################################################################################################################
'# Copyright John Lindsay, 2009. Code written by John Lindsay, July 16, 2009.
'# This program is free software: you can redistribute it and/or modify it under the terms of the GNU General Public License as published by the Free Software Foundation, either version 3 of the License, or (at your option) any later version.
'# This program is distributed in the hope that it will be useful, but WITHOUT ANY WARRANTY; without even the implied warranty of MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the GNU General Public License for more details.
'# You should have received a copy of the GNU General Public License along with this program.  If not, see <http://www.gnu.org/licenses/>.
'#################################################################################################################################################################################################################################################

'This tool performs a weighted summation on multiple input images

Imports System
Imports System.IO
Imports System.Threading
Imports System.ComponentModel

Public Class WeightedOverlay
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
            Return "Weighted Overlay"
        End Get
    End Property

    Public ReadOnly Property Description() As String Implements Interfaces.IPlugin.Description
        Get
            Return "Performs a weighted sum on multiple input raster images after converting each image to a common scale"
        End Get
    End Property

    Public ReadOnly Property ToolboxInfo() As String Implements Interfaces.IPlugin.ToolboxName
        Get
            Return "OverlayTools"
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
        Dim InputFilesString As String = Nothing
        Dim ImageFiles() As String
        Dim Weights() As Single
        Dim OutputHeaderFile As String = Nothing
        Dim Images() As Object
        Dim Output As New GeospatialFiles.GATGrid
        Dim NumCols, NumRows, NumImages, NumWeights As Integer
        Dim Z As Single
        Dim Progress As Single = 0
        Dim col, row As Integer
        Dim a As Integer
        Dim j As Integer = 0
        Dim InputString() As String = Nothing
        Dim InputConstraintString() As String = Nothing
        Dim WeightedSum As Single
        Dim CommonScale As String
        Dim ScaleMin As Single = 0
        Dim ScaleMax As Single = 1
        Dim ImageMin As Single = 0
        Dim ImageMax As Single = 1
        Dim CostFactor() As Boolean

        ReDim InputString(0)
        ReDim Weights(0)
        ReDim ImageFiles(0)
        ReDim CostFactor(0)
        Try
            For a = 0 To UBound(ParameterArray)
                Select Case a
                    Case 0
                        InputString = Split(Trim(ParameterArray(a)), vbTab)
                    Case 1
                        InputConstraintString = Split(Trim(ParameterArray(a)), "; ")
                    Case 2
                        OutputHeaderFile = ParameterArray(a)
                    Case 3
                        CommonScale = ParameterArray(a)
                        Select Case CommonScale
                            Case "0-1"
                                ScaleMin = 0
                                ScaleMax = 1
                            Case "0-100"
                                ScaleMin = 0
                                ScaleMax = 100
                            Case "0-255"
                                ScaleMin = 0
                                ScaleMax = 255
                        End Select
                End Select
            Next

            'read the input data
            NumImages = 0
            NumWeights = 0
            Dim k As Byte = 0
            For a = 0 To UBound(InputString)
                Select Case k
                    Case 0 'it should be a boolean indicating whether it is a cost factor
                        If Trim(InputString(a)) <> Nothing AndAlso InputString(a) <> vbTab Then
                            ReDim Preserve CostFactor(NumImages)
                            CostFactor(NumImages) = CBool(InputString(a))
                        End If
                        k = 1
                    Case 1 'it should be a file
                        If Trim(InputString(a)) <> Nothing AndAlso InputString(a) <> vbTab Then
                            HeaderFile = Trim(InputString(a))
                            If File.Exists(HeaderFile) Then
                                ReDim Preserve ImageFiles(NumImages)
                                ImageFiles(NumImages) = HeaderFile
                                NumImages += 1
                            Else
                                ShowFeedback("Input image not found.", "An Error Has Occurred")
                                Return 2
                            End If
                        End If
                        k = 2
                    Case 2 'it should be a weight
                        If InputString(a) <> Nothing AndAlso InputString(a) <> vbTab Then
                            ReDim Preserve Weights(NumWeights)
                            Weights(NumWeights) = CSng(InputString(a))
                            NumWeights += 1
                        End If
                        k = 0
                End Select
            Next

            If NumImages <> NumWeights Then
                ShowFeedback("The number of input images is not equal to the number of weights.", "An Error Has Occurred")
                Return 2
            End If

            If NumImages < 2 Then
                ShowFeedback("At least two input images must be specified", "An Error Has Occurred")
                Return 2
            End If

            'convert the weights to the range 0 to 1 and make sure that they sum to 1
            Z = 0
            For a = 0 To NumWeights - 1
                Z += Weights(a)
            Next
            If Z <= 0 Then
                ShowFeedback("Weights have not been correctly specified", "An Error Has Occurred")
                Return 2
            End If
            For a = 0 To NumWeights - 1
                Weights(a) = Weights(a) / Z
            Next

            'initialize each of the input images and make sure that they have the same number of rows and columns
            ReDim Images(NumImages - 1)
            Dim NewImage As New GeospatialFiles.GATGrid
            For j = 0 To NumImages - 1
                Dim NewImage2 As New GeospatialFiles.GATGrid
                NewImage2.HeaderFileName = ImageFiles(j)
                Images(j) = NewImage2

                If j = 0 Then
                    NumCols = NewImage2.NumberColumns
                    NumRows = NewImage2.NumberRows
                Else
                    If NewImage2.NumberColumns <> NumCols OrElse _
                    NewImage2.NumberRows <> NumRows Then
                        ShowFeedback("Input images must have the same dimensions.", "An Error Has Occurred")
                        Return 2
                    End If
                End If
            Next

            If OutputHeaderFile = Nothing Then
                ShowFeedback("Parameters not set", "An Error Has Occurred")
                Return 2
            End If

            NewImage = DirectCast(Images(0), GeospatialFiles.GATGrid)
            Output.HeaderFileName = OutputHeaderFile
            Output.DeleteCurrentFiles()
            Output.WriteChangesToFile = True
            Output.SetPropertiesUsingAnotherFile(NewImage, "float")
            Output.InitializeGrid(NumCols, NumRows, 0)
            Output.DataScale = "continuous"

            For j = 0 To NumImages - 1
                NewImage = DirectCast(Images(j), GeospatialFiles.GATGrid)
                ImageMin = NewImage.Minimum
                ImageMax = NewImage.Maximum
                Dim BlnCost As Boolean = CostFactor(j)
                Dim ImageRange As Single = ImageMax - ImageMin
                For row = 0 To NumRows - 1
                    For col = 0 To NumCols - 1
                        Z = NewImage(col, row)
                        If Z <> NoData Then
                            WeightedSum = Output(col, row)
                            If WeightedSum <> NoData Then
                                If BlnCost = False Then
                                    Z = (Z - ImageMin) / ImageRange * ScaleMax
                                    Output(col, row) = WeightedSum + Z * Weights(j)
                                Else
                                    Z = (1 - (Z - ImageMin) / ImageRange) * ScaleMax
                                    Output(col, row) = WeightedSum + Z * Weights(j)
                                End If
                            End If
                        Else
                            Output(col, row) = NoData
                        End If
                    Next
                    If CancelBool Then Return 1
                    Progress = row / (NumRows - 1) * 100
                    worker.ReportProgress(Progress)
                Next
            Next

            'now deal with the contraints
            NumImages = InputConstraintString.GetUpperBound(0) + 1
            If NumImages > 0 Then
                For j = 0 To NumImages - 1
                    HeaderFile = InputConstraintString(j)
                    If File.Exists(HeaderFile) Then
                        NewImage.ReleaseMemoryResources()
                        NewImage.HeaderFileName = HeaderFile

                        If NewImage.NumberColumns <> NumCols OrElse NewImage.NumberRows <> NumRows Then
                            ShowFeedback("Each of the input images must have the same dimensions.")
                            Return 2
                        End If
                        For row = 0 To NumRows - 1
                            For col = 0 To NumCols - 1
                                Z = NewImage(col, row)
                                If Z <> NoData Then
                                    If Z <= 0 Then
                                        Output(col, row) = 0
                                    End If
                                Else
                                    Output(col, row) = NoData
                                End If
                            Next
                            If CancelBool Then Return 1
                            Progress = row / (NumRows - 1) * 100
                            worker.ReportProgress(Progress)
                        Next
                    End If
                Next
            End If

            NewImage.ReleaseMemoryResources()
            Output.MetadataEntries(Output.NumMetadataEntries + 1) = "Created by the WeightedOverlay tool"
            Dim dateTimeInfo As DateTime = DateTime.Now
            Output.MetadataEntries(Output.NumMetadataEntries + 1) = "Created on " & dateTimeInfo.ToString("F")
            Output.WriteHeaderFile()
            Output.ReleaseMemoryResources()

            worker.ReportProgress(0)

            Return OutputHeaderFile
        Catch ex As Exception
            ShowFeedback(ex.Message)
            Return 2
        End Try
    End Function
End Class
