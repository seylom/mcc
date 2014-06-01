Imports Microsoft.VisualBasic

Public Class MasterBase
   Inherits MasterPage


   Protected ReadOnly Property BaseURL() As String
      Get
         Try
            Return String.Format("http://{0}{1}", _
                                 HttpContext.Current.Request.ServerVariables("HTTP_HOST"), _
                                 IIf((VirtualFolder.Equals("/")), String.Empty, VirtualFolder))
         Catch
            ' This is for design time
            Return Nothing
         End Try

      End Get
   End Property

   ''' <summary>
   ''' Returns the name of the virtual folder where our project lives
   ''' </summary>
   ''' <value></value>
   ''' <returns></returns>
   ''' <remarks></remarks>
   Private Shared ReadOnly Property VirtualFolder() As String
      Get
         Return HttpContext.Current.Request.ApplicationPath
      End Get
   End Property


End Class
