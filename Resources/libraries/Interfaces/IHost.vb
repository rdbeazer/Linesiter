Public Interface IHost

    Sub ShowFeedback(ByVal strFeedback As String, Optional ByVal Caption As String = "GAT Message")

    Sub SetParameters(ByVal ParameterArray() As String)

    Sub ProgressBarLabel(ByVal label As String)

    Sub RunPlugin(ByVal PluginClassName As String)

    Property RunInSynchronousMode() As Boolean

    ReadOnly Property ApplicationDirectory() As String

    Property RecentDirectory() As String

End Interface
