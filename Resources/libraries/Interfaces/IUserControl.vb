Imports System
Imports System.Threading
Imports System.ComponentModel

Public Interface IUserControl
    Sub Initialize(ByVal Host As IHost, ByVal ParameterArray() As String)

    Property CancelOp() As Boolean

    Property RecentDirectory() As String

    'Function Execute(ByVal ParameterArray() As String, ByVal worker As BackgroundWorker) As Object

End Interface
