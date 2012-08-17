Public Interface IPlugin

    Sub Initialize(ByVal Host As IHost)

    ReadOnly Property Name() As String

    Function Calculate(ByVal int1 As Integer, ByVal int2 As Integer) As Double

End Interface
