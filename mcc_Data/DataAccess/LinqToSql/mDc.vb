Partial Public Class MCCDataContext
    Public Sub New()
        MyBase.New(Global.System.Configuration.ConfigurationManager.ConnectionStrings("ASPNETDBConnectionString").ConnectionString, mappingSource)
        OnCreated()
    End Sub
End Class

