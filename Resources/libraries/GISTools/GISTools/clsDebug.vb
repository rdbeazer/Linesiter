Imports System.Windows.Forms
'Imports System.IO
Imports System.Threading
Imports System.ComponentModel
'Imports System.Drawing
Imports System.Collections.ObjectModel
'Imports Microsoft.VisualBasic.FileIO
Public Class clsDebug
    Private WithEvents BackgroundWorker1 As New System.ComponentModel.BackgroundWorker
    Dim objHost As Interfaces.IHost
    Dim objPlugin As Interfaces.IPlugin

    Public Sub CallTheTool()
        Dim ParameterArray() As String
        ReDim ParameterArray(5)
        Dim SourceFileName As String = "C:\Users\John Lindsay\Documents\Research\Active Papers\Bmax\data\Vermont\source.dep"
        Dim CostFileName As String = "C:\Users\John Lindsay\Documents\Research\Active Papers\Bmax\data\Vermont\slope.dep"
        Dim BackLinkFileName As String = "C:\Users\John Lindsay\Documents\Research\Active Papers\Bmax\data\Vermont\tmp8.dep"
        Dim Output As String = "C:\Users\John Lindsay\Documents\Research\Active Papers\Bmax\data\Vermont\tmp1.dep"

        'Dim DestFileName As String = "C:\Users\John Lindsay\Documents\Research\Active Papers\Bmax\data\Vermont\destination.dep"
        'Dim BackLinkFileName As String = "C:\Users\John Lindsay\Documents\Research\Active Papers\Bmax\data\Vermont\tmp2.dep"
        'Dim Output As String = "C:\Users\John Lindsay\Documents\Research\Active Papers\Bmax\data\Vermont\tmp8.dep"

        ParameterArray(0) = SourceFileName
        ParameterArray(1) = CostFileName
        ParameterArray(2) = Output
        ParameterArray(3) = BackLinkFileName
        ParameterArray(4) = "45"
        ParameterArray(5) = "2"

        Dim TestObj As New CostAccumulation
        Dim TestHost As New clsHostForDebug
        objHost = DirectCast(TestHost, Interfaces.IHost)

        objPlugin = DirectCast(TestObj, Interfaces.IPlugin)
        objPlugin.Initialize(objHost)

        BackgroundWorker1.WorkerReportsProgress = True
        BackgroundWorker1.WorkerSupportsCancellation = True

        BackgroundWorker1.RunWorkerAsync(ParameterArray)
        Do

        Loop While BackgroundWorker1.IsBusy
    End Sub


    Private Sub BackgroundWorker1_DoWork(ByVal sender As Object, ByVal e As System.ComponentModel.DoWorkEventArgs) Handles BackgroundWorker1.DoWork
        ' Get the BackgroundWorker object that raised this event.
        Dim worker As BackgroundWorker = CType(sender, BackgroundWorker)

        ' Assign the result of the computation
        ' to the Result property of the DoWorkEventArgs
        ' object. This is will be available to the 
        ' RunWorkerCompleted eventhandler.
        e.Result = objPlugin.Execute(e.Argument, worker)
    End Sub

    Private Sub BackgroundWorker1_ProgressChanged(ByVal sender As Object, ByVal e As System.ComponentModel.ProgressChangedEventArgs) Handles BackgroundWorker1.ProgressChanged
        'This sub is just for testing the new tool. it does nothing.
    End Sub

    Private Sub BackgroundWorker1_RunWorkerCompleted(ByVal sender As Object, ByVal e As System.ComponentModel.RunWorkerCompletedEventArgs) Handles BackgroundWorker1.RunWorkerCompleted
        'This sub is just for testing the new tool. it does nothing.
    End Sub


End Class
