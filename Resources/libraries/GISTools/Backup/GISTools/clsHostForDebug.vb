Public Class clsHostForDebug
    Implements Interfaces.IHost
    Public Sub RunPlugin(ByVal PluginClassName As String) Implements Interfaces.IHost.RunPlugin
        'This sub is just for testing the new tool. it does nothing.
    End Sub

    Public Sub SetParameters(ByVal ParameterArray() As String) Implements Interfaces.IHost.SetParameters
        'This sub is just for testing the new tool. it does nothing.
    End Sub

    Public Sub ShowFeedback(ByVal strFeedback As String, Optional ByVal Caption As String = "GAT Message") Implements Interfaces.IHost.ShowFeedback
        'This sub is just for testing the new tool. it does nothing.
    End Sub

    Public Sub ProgressBarLabel1(ByVal label As String) Implements Interfaces.IHost.ProgressBarLabel
        'This sub is just for testing the new tool. it does nothing.
    End Sub

    Public Property RunInSynchronousMode() As Boolean Implements Interfaces.IHost.RunInSynchronousMode
        Get

        End Get
        Set(ByVal value As Boolean)

        End Set
    End Property

    Public Property RecentDir() As String Implements Interfaces.IHost.RecentDirectory
        Get
            Return Nothing
        End Get
        Set(ByVal value As String)

        End Set
    End Property

    Private AppDir As String
    Public ReadOnly Property ApplicationDirectory() As String Implements Interfaces.IHost.ApplicationDirectory
        Get
            Return Nothing
        End Get
    End Property
End Class