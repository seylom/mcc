Public Module GlobalFilters


   <System.Runtime.CompilerServices.Extension()> _
   Function WithID(ByVal qry As IQueryable(Of MCC.Data.Ad), ByVal AdId As Integer) As IQueryable(Of MCC.Data.Ad)
      Return From o In qry Where (o.AdID = AdId) _
                   Select o
   End Function


End Module
