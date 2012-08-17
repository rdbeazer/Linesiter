Imports System
Imports System.Threading
Imports System.ComponentModel

Public Interface IPlugin

    Sub Initialize(ByVal Host As IHost)

    ReadOnly Property DescriptiveName() As String

    ReadOnly Property Description() As String

    ReadOnly Property ToolboxName() As String

    Property CancelOp() As Boolean

    Function Execute(ByVal ParameterArray() As String, ByVal worker As BackgroundWorker) As Object

End Interface
