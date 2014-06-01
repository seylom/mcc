Partial Public Class umcDataContext
    Public Sub New()
        MyBase.New(Global.System.Configuration.ConfigurationManager.ConnectionStrings("ASPNETDBConnectionString").ConnectionString, mappingSource)
        OnCreated()
    End Sub
End Class

