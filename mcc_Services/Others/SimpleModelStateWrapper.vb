Imports System.Web.mvc
Public Class SimpleModelStateWrapper
   Implements IValidationService

   Private _modelState As ModelStateDictionary

   Public Sub New(ByVal modelState As ModelStateDictionary)
      _modelState = modelState
   End Sub

   Public Sub AddError(ByVal key As String, ByVal message As String) Implements IValidationService.AddError
      _modelState.AddModelError(key, message)
   End Sub

   Public Function IsValid() As Boolean Implements IValidationService.IsValid
      Return _modelState.IsValid
   End Function
End Class
