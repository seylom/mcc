Imports Microsoft.VisualBasic
Imports MCC
Imports System.Linq
Imports MCC.Data

Public Class RegistrySettings
   Inherits mccObject
   Private mRegHash As RegistryHash


   Public Sub New()
      mRegHash = New RegistryHash
      LoadRegistrySettings()
   End Sub

   Public Sub LoadRegistrySettings()
      Dim ps As List(Of mcc_Registry)
      Dim key As String = "Registry_Registry_"

      If Cache(key) IsNot Nothing Then
         ps = DirectCast(mccObject.Cache(key), List(Of mcc_Registry))
      Else
         Dim mdc As New MCCDataContext()
         ps = (From e As mcc_Registry In mdc.mcc_Registries).ToList
         CacheData(key, ps)
         mdc.Dispose()
      End If

      For Each it As mcc_Registry In ps
         If Not mRegHash.Contains(it.RegistryName) Then
            mRegHash.Add(it.RegistryName.ToLower(), it.RegistryValue.ToLower())
         End If
      Next
   End Sub

   Public Sub SaveRegistrySettings()
      Dim mdc As New MCCDataContext()
      For Each obj As DictionaryEntry In mRegHash
         Dim str As String = obj.Key.ToString
         Dim rs As mcc_Registry = (From it As mcc_Registry In mdc.mcc_Registries Where it.RegistryName.ToLower = str).FirstOrDefault
         If rs IsNot Nothing Then
            rs.RegistryValue = obj.Value.ToString
            Dim cs As System.Data.Linq.ChangeSet = mdc.GetChangeSet()
            mdc.SubmitChanges()
         End If
      Next
      mccObject.PurgeCacheItems("Registry_Registry_")
   End Sub


   'Private _quotes As Boolean = False
   Public Property EnableQuotes() As Boolean
      Get
         Return mRegHash.GetValueBool("EnableQuotes", False)
      End Get
      Set(ByVal value As Boolean)
         mRegHash.SetValueBool("EnableQuotes", value)
      End Set
   End Property

   'Private _maxArticlesPerPage As Integer = 12
   Public Property MaxArticlesPerPage() As Integer
      Get
         Return mRegHash.GetValueInt("MaxArticlesPerPage", 12)
      End Get
      Set(ByVal value As Integer)
         mRegHash.SetValueString("MaxArticlesPerPage", value)
      End Set
   End Property

   'Private _maxAdvicesPerPage As Integer = 12
   Public Property MaxAdvicesPerPage() As Integer
      Get
         Return mRegHash.GetValueInt("MaxAdvicesPerPage", 12)
      End Get
      Set(ByVal value As Integer)
         mRegHash.SetValueString("MaxAdvicesPerPage", value)
      End Set
   End Property

   'Private _maxCommentsPerPage As Integer = 25
   Public Property MaxCommentsPerPage() As Integer
      Get
         Return mRegHash.GetValueInt("MaxCommentsPerPage", 12)
      End Get
      Set(ByVal value As Integer)
         mRegHash.SetValueString("MaxCommentsPerPage", value)
      End Set
   End Property


   'Private _theme As String  = dark
   Public Property Theme() As String
      Get
         Return mRegHash.GetValueString("Theme", "dark")
      End Get
      Set(ByVal value As String)
         mRegHash.SetValueString("Theme", value)
      End Set
   End Property

   Public Property Version() As String
      Get
         Return mRegHash.GetValueString("Version", "1.4")
      End Get
      Set(ByVal value As String)
         mRegHash.SetValueString("Version", value)
      End Set
   End Property


   'Private _homePageID As Integer = 0
   Public Property HomePageID() As Integer
      Get
         Return mRegHash.GetValueInt("HomePageID", 0)
      End Get
      Set(ByVal value As Integer)
         mRegHash.SetValueString("HomePageID", value)
      End Set
   End Property


   'Private _verifyEmail As Boolean = False
   Public Property VerifyEmail() As Boolean
      Get
         Return mRegHash.GetValueBool("VerifyEmail", False)
      End Get
      Set(ByVal value As Boolean)
         mRegHash.SetValueBool("VerifyEmail", value)
      End Set
   End Property


   'Private _NotifyFlaggedComments As Boolean
   Public Property NotifyFlaggedComments() As Boolean
      Get
         Return mRegHash.GetValueBool("NotifyFlaggedComments", False)
      End Get
      Set(ByVal value As Boolean)
         mRegHash.SetValueBool("NotifyFlaggedComments", value)
      End Set
   End Property

   'Private _filterWords As Boolean = False
   Public Property FilterWords() As Boolean
      Get
         Return mRegHash.GetValueBool("FilterWords", False)
      End Get
      Set(ByVal value As Boolean)
         mRegHash.SetValueBool("FilterWords", value)
      End Set
   End Property

   'Private _enablePolls As Boolean = False
   Public Property EnablePolls() As Boolean
      Get
         Return mRegHash.GetValueBool("EnablePolls", False)
      End Get
      Set(ByVal value As Boolean)
         mRegHash.SetValueBool("EnablePolls", value)
      End Set
   End Property

   Public Property EnableComments() As Boolean
      Get
         Return mRegHash.GetValueBool("EnableComments", False)
      End Get
      Set(ByVal value As Boolean)
         mRegHash.SetValueBool("EnableComments", value)
      End Set
   End Property

   Public Property EnableRegistrations() As Boolean
      Get
         Return mRegHash.GetValueBool("EnableRegistrations", False)
      End Get
      Set(ByVal value As Boolean)
         mRegHash.SetValueBool("EnableRegistrations", value)
      End Set
   End Property


   Public Property FullTextSearch() As Boolean
      Get
         Return mRegHash.GetValueBool("FullTextSearch", False)
      End Get
      Set(ByVal value As Boolean)
         mRegHash.SetValueBool("FullTextSearch", value)
      End Set
   End Property

   ''' <summary>
   ''' Cache the input data, if caching is enabled
   ''' </summary>
   Protected Shared Sub CacheData(ByVal key As String, ByVal data As Object)
      mccObject.Cache.Insert(key, data, Nothing, DateTime.Now.AddSeconds(600), TimeSpan.Zero)
   End Sub
End Class

Public Class RegistryHash
   Inherits Hashtable

   ' helper class functions
   Public Function GetValueInt(ByVal Name As String, ByVal [default] As Integer) As Integer
      If Me(Name.ToLower()) Is Nothing Then
         Return [default]
      End If
      Return Convert.ToInt32(Me(Name.ToLower()))
   End Function

   Public Sub SetValueInt(ByVal Name As String, ByVal Value As Integer)
      Me(Name.ToLower()) = Convert.ToString(Value)
   End Sub
   Public Function GetValueBool(ByVal Name As String, ByVal [Default] As Boolean) As Boolean
      If Me(Name.ToLower()) Is Nothing Then
         Return [Default]
      End If
      Return Convert.ToBoolean(Convert.ToInt32(Me(Name.ToLower())))
   End Function
   Public Sub SetValueBool(ByVal Name As String, ByVal Value As Boolean)
      Me(Name.ToLower()) = Convert.ToString(Convert.ToInt32(Value))
   End Sub
   Public Function GetValueString(ByVal Name As String, ByVal [Default] As String) As String
      If Me(Name.ToLower()) Is Nothing Then
         Return [Default]
      End If
      Return Convert.ToString(Me(Name.ToLower()))
   End Function
   Public Sub SetValueString(ByVal Name As String, ByVal Value As String)
      Me(Name.ToLower()) = Value
   End Sub
End Class
