'Imports Microsoft.VisualBasic
'Imports System.Xml


'Public Class XmlJSON
'   Public Shared Function XmlToJSON(ByVal xmlDoc As XmlDocument) As String
'      Dim sbJSON As New StringBuilder()
'      sbJSON.Append("{ ")
'      XmlToJSONnode(sbJSON, xmlDoc.DocumentElement, True)
'      sbJSON.Append("}")
'      Return sbJSON.ToString()
'   End Function

'   ' XmlToJSONnode: Output an XmlElement, possibly as part of a higher array 
'   Public Shared Sub XmlToJSONnode(ByVal sbJSON As StringBuilder, ByVal node As XmlElement, ByVal showNodeName As Boolean)
'      Dim childAdded As Boolean = False
'      If showNodeName Then
'         sbJSON.Append("""" + SafeJSON(node.Name) + """: ")
'      End If
'      sbJSON.Append("{")
'      ' Build a sorted list of key-value pairs 
'      ' where key is case-sensitive nodeName 
'      ' value is an ArrayList of string or XmlElement 
'      ' so that we know whether the nodeName is an array or not. 
'      Dim childNodeNames As New SortedList()

'      ' Add in all node attributes 
'      If node.Attributes IsNot Nothing Then
'         For Each attr As XmlAttribute In node.Attributes
'            StoreChildNode(childNodeNames, attr.Name, attr.InnerText)
'         Next
'      End If

'      ' Add in all nodes 
'      For Each cnode As XmlNode In node.ChildNodes
'         childAdded = True
'         If TypeOf cnode Is XmlText Then
'            StoreChildNode(childNodeNames, "value", cnode.InnerText)
'         ElseIf TypeOf cnode Is XmlElement Then
'            StoreChildNode(childNodeNames, cnode.Name, cnode)
'         End If
'      Next

'      ' Now output all stored info 
'      For Each childname As String In childNodeNames.Keys
'         childAdded = True
'         Dim alChild As ArrayList = DirectCast(childNodeNames(childname), ArrayList)
'         If alChild.Count = 1 AndAlso alChild(0).GetType Is GetType(String) Then
'            OutputNode(childname, alChild(0), sbJSON, True)
'         Else
'            sbJSON.Append(" """ + SafeJSON(childname) + """: [ ")
'            For Each Child As Object In alChild
'               OutputNode(childname, Child, sbJSON, False)
'            Next
'            sbJSON.Remove(sbJSON.Length - 2, 2)
'            sbJSON.Append(" ], ")
'         End If
'      Next
'      sbJSON.Remove(sbJSON.Length - 2, 2)
'      If childAdded Then
'         sbJSON.Append(" }")
'      Else
'         sbJSON.Append(" null")
'      End If
'   End Sub

'   ' StoreChildNode: Store data associated with each nodeName 
'   ' so that we know whether the nodeName is an array or not. 
'   Private Shared Sub StoreChildNode(ByVal childNodeNames As SortedList, ByVal nodeName As String, ByVal nodeValue As Object)
'      ' Pre-process contraction of XmlElement-s 
'      If TypeOf nodeValue Is XmlElement Then
'         ' Convert <aa></aa> into "aa":null 
'         ' <aa>xx</aa> into "aa":"xx" 
'         Dim cnode As XmlNode = DirectCast(nodeValue, XmlNode)
'         If cnode.Attributes.Count = 0 Then
'            Dim children As XmlNodeList = cnode.ChildNodes
'            If children.Count = 0 Then
'               nodeValue = Nothing
'            ElseIf children.Count = 1 AndAlso (TypeOf children(0) Is XmlText) Then
'               nodeValue = DirectCast((children(0)), XmlText).InnerText
'            End If
'         End If
'      End If
'      ' Add nodeValue to ArrayList associated with each nodeName 
'      ' If nodeName doesn't exist then add it 
'      Dim oValuesAL As Object = childNodeNames(nodeName)
'      Dim ValuesAL As ArrayList
'      If oValuesAL Is Nothing Then
'         ValuesAL = New ArrayList()
'         childNodeNames(nodeName) = ValuesAL
'      Else
'         ValuesAL = DirectCast(oValuesAL, ArrayList)
'      End If
'      ValuesAL.Add(nodeValue)
'   End Sub

'   Private Shared Sub OutputNode(ByVal childname As String, ByVal alChild As Object, ByVal sbJSON As StringBuilder, ByVal showNodeName As Boolean)
'      If alChild Is Nothing Then
'         If showNodeName Then
'            sbJSON.Append("""" + SafeJSON(childname) + """: ")
'         End If
'         sbJSON.Append("null")
'      ElseIf TypeOf alChild Is String Then
'         If showNodeName Then
'            sbJSON.Append("""" + SafeJSON(childname) + """: ")
'         End If
'         Dim sChild As String = DirectCast(alChild, String)
'         sChild = sChild.Trim()
'         sbJSON.Append("""" + SafeJSON(sChild) + """")
'      Else
'         XmlToJSONnode(sbJSON, DirectCast(alChild, XmlElement), showNodeName)
'      End If
'      sbJSON.Append(", ")
'   End Sub

'   ' Make a string safe for JSON 
'   Private Shared Function SafeJSON(ByVal sIn As String) As String
'      Dim sbOut As New StringBuilder(sIn.Length)
'      For Each ch As Char In sIn
'         If [Char].IsControl(ch) OrElse ch = "'"c Then
'            Dim ich As Integer = AscW(ch)
'            sbOut.Append("\u" + ich.ToString("x4"))
'            Continue For
'         ElseIf ch = """"c OrElse ch = "\"c OrElse ch = "/"c Then
'            sbOut.Append("\"c)
'         End If
'         sbOut.Append(ch)
'      Next
'      Return sbOut.ToString()
'   End Function
'End Class
