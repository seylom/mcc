Public Interface IValidationService
   Function IsValid() As Boolean
   Sub AddError(ByVal key As String, ByVal message As String)
End Interface
