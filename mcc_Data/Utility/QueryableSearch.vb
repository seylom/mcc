Imports System.Collections.Generic
Imports System.Linq
Imports System.Linq.Dynamic
Imports System.Reflection
Imports System.Collections

''' <summary>
''' This class will take any IQueryable and search through it recursively with minimal effort
''' This was designed to be used with Linq-to-SQL or Linq-to-Entities, but also works on DataTables and other 'multi-column' IQueryables
''' 
''' This can be as general or specific as  you want it to be. For example, if you simply want to search 5 text fields, you just pass in the 5 field names, and the keywords to seach for.
''' Or, if you want to be super specific, you can do that too:
''' Say you want to pass in an IQueryable with the following fields: int named 'ID', DateTime named 'birthday', text named 'name' and bool named 'male'
''' you would make following Dictionary[column_name, type]:
'''   ColumnNamesAndTypes.Add("ID", typeof(int));
'''   ColumnNamesAndTypes.Add("birthday", typeof(DateTime));
'''   ColumnNamesAndTypes.Add("name", typeof(string));
'''   ColumnNamesAndTypes.Add("male", typeof(bool));
'''   
''' then use a more specific overload:
''' yourIQeryable.Search(ColumnNamesAndTypes, objectArrayOfKeywords);
''' 
''' the object array of keywords will only be run against corresponding column types, so if you pass in a boolean value 'True', it will only be run against the "male" column and not the others
''' </summary>
Public Module QueryableSearch

   Public Enum StringSearchType
      Contains
      Equals
   End Enum


   ''' <summary>
   ''' Search an IQueryable's fields for the keywords (objects) - this does not iterate through levels of an object
   ''' </summary>
   ''' <param name="list_to_search">IQueryable to search</param>
   ''' <param name="keywords">objects to search for</param>
   ''' <returns>IQueryable of the inputed type filtered by the search specifications</returns>
   <System.Runtime.CompilerServices.Extension()> _
   Public Function Search(ByVal list_to_search As IQueryable, ByVal keywords As Object()) As IQueryable
      Dim dic As New Dictionary(Of String, Type)()
      For Each item In list_to_search.Take(1)
         For Each pi As PropertyInfo In item.[GetType]().GetProperties()
            dic.Add(pi.Name, pi.PropertyType)
         Next
      Next
      Return list_to_search.Search(dic, keywords)
   End Function

   ''' <summary>
   ''' Search an IQueryable's specified text fields if they contain the keywords; will not work for any fields other than string fields, if you ware using fields that are not strings, use on of the more specific overloads
   ''' </summary>
   ''' <param name="list_to_search">IQueryable to search</param>
   ''' <param name="columns_to_search">string names of the columns to be searched within the IQueryable, may be nest reltaions as well</param>
   ''' <param name="keywords">array for strings to search for</param>
   ''' <returns>IQueryable of the inputed type filtered by the search specifications</returns>
   <System.Runtime.CompilerServices.Extension()> _
   Public Function Search(ByVal list_to_search As IQueryable, ByVal columns_to_search As String(), ByVal keywords As String()) As IQueryable
      Return Search(list_to_search, columns_to_search, keywords, StringSearchType.Contains)
   End Function

   ''' <summary>
   ''' Search an IQueryable's specified text fields if they contain/equal the keywords;  will not work for any fields other than string fields, if you ware using fields that are not strings, use on of the more specific overloads
   ''' </summary>
   ''' <param name="list_to_search">IQueryable to search</param>
   ''' <param name="columns_to_search">string names of the columns to be searched within the IQueryable, may be nest relations as well</param>
   ''' <param name="keywords">array for strings to search for</param>
   ''' <param name="string_search_type">Whether or not the string operations use the strict 'Equals' or the broader 'Contains' method</param>
   ''' <returns>IQueryable of the inputed type filtered by the search specifications</returns>
   <System.Runtime.CompilerServices.Extension()> _
   Public Function Search(ByVal list_to_search As IQueryable, ByVal columns_to_search As String(), ByVal keywords As String(), ByVal string_search_type As StringSearchType) As IQueryable
      Return Search(list_to_search, MakeDictionary(columns_to_search), keywords, string_search_type)
   End Function

   ''' <summary>
   ''' Search an IQueryable's specified fields if they contain/equal the keywords; fields other than text will be forced to an '==' operator; strings will default to Contains with this overload
   ''' </summary>
   ''' <param name="list_to_search">IQueryable to search</param>
   ''' <param name="columns_to_search">Dictionary with KeyValuePairs of [column_name, type_of_column]; such as [record_id, typeof(int)] or [birthdate, typeof(DateTime)]</param>
   ''' <param name="keywords">array of objects of 'keywords' to search for, any type of objects</param>
   ''' <returns>IQueryable of the inputed type filtered by the search specifications</returns>
   <System.Runtime.CompilerServices.Extension()> _
   Public Function Search(ByVal list_to_search As IQueryable, ByVal columns_to_search As Dictionary(Of String, Type), ByVal keywords As Object()) As IQueryable
      Return Search(list_to_search, columns_to_search, keywords, StringSearchType.Contains)
   End Function

   ''' <summary>
   ''' Search an IQueryable's specified fields if they contain/equal the keywords; fields other than text will be forced to an '==' operator
   ''' </summary>
   ''' <param name="list_to_search">IQueryable to search</param>
   ''' <param name="columns_to_search">Dictionary with KeyValuePairs of [column_name, type_of_column]; such as [record_id, typeof(int)] or [birthdate, typeof(DateTime)]</param>
   ''' <param name="keywords">array of objects of 'keywords' to search for, any type of objects</param>
   ''' <param name="string_search_type">Whether or not the string operations use the strict 'Equals' or the broader 'Contains' method</param>
   ''' <returns>IQueryable of the inputed type filtered by the search specifications</returns>
   <System.Runtime.CompilerServices.Extension()> _
   Public Function Search(ByVal list_to_search As IQueryable, ByVal columns_to_search As Dictionary(Of String, Type), ByVal keywords As Object(), ByVal string_search_type As StringSearchType) As IQueryable
      Dim search_object_combos As New Dictionary(Of Object, String)()

      For Each o As Object In keywords
         Dim where_expression As String = String.Empty
         For Each column As KeyValuePair(Of String, Type) In columns_to_search
            If o.[GetType]() Is column.Value Then
               If column.Value Is GetType(String) Then
                  where_expression += column.Key & "." & (If(string_search_type = StringSearchType.Equals, "Equals", "Contains")) & "(@0) || "
               Else
                  ' any other data types will run against the keyword with a '=='
                  where_expression += column.Key & " == @0 || "
               End If
            End If
         Next
         search_object_combos.AddSearchObjectCombo(where_expression, o)
      Next

      Dim results As IQueryable
      If search_object_combos.Count() = 0 Then
         results = Nothing
      Else
         'nothing to search
         results = list_to_search.SearchInitial(search_object_combos.First().Value, search_object_combos.First().Key)
      End If

      If search_object_combos.Count() > 1 Then
         ' otherwise, keep use the resulting set and recursively filter it
         search_object_combos.Remove(search_object_combos.First().Key)
         For Each combo As KeyValuePair(Of Object, String) In search_object_combos
            results = Search(results, combo.Value, combo.Key)
         Next
      End If
      Return results
   End Function

   ''' <summary>
   ''' Adds a where_expression and object to a Dictionary used to hold the information for a search query
   ''' </summary>
   ''' <param name="dic">Dictionary for holding combinations of where_expressions and objects</param>
   ''' <param name="where_expression">LINQ where expression</param>
   ''' <param name="o">object corresponding to the where_expression</param>
   <System.Runtime.CompilerServices.Extension()> _
   Private Sub AddSearchObjectCombo(ByVal dic As Dictionary(Of Object, String), ByVal where_expression As String, ByVal o As Object)
      ' remove the last " || "
      where_expression = where_expression.Remove(where_expression.Length - 4, 4)
      dic.Add(o, where_expression)
   End Sub

   ''' <summary>
   ''' Called by the public Search function recursively after the initial search is made
   ''' </summary>
   ''' <param name="results">Results set from the previous search</param>
   ''' <param name="where_expression">LINQ where expression</param>
   ''' <param name="keyword">object corresponding to the where_expression</param>
   ''' <returns>IQueryable of the inputed type filtered by this search specification</returns>
   Private Function Search(ByVal results As IQueryable, ByVal where_expression As String, ByVal keyword As Object) As IQueryable
      Return results.Where(where_expression, keyword)
   End Function

   ''' <summary>
   ''' This will initially search the IQueryable, if you are using Linq-to-SQL or Linq-to-Entites, this will be your only DB call
   ''' </summary>
   ''' <param name="list_to_search">IQueryable to search</param>
   ''' <param name="where_expression">LINQ where expression</param>
   ''' <param name="keyword">object corresponding to the where_expression</param>
   ''' <returns>IQueryable of the inputed type filtered by this search specification</returns>
   <System.Runtime.CompilerServices.Extension()> _
   Private Function SearchInitial(ByVal list_to_search As IQueryable, ByVal where_expression As String, ByVal keyword As Object) As IQueryable
      Return list_to_search.Where(where_expression, keyword)
   End Function

   ''' <summary>
   ''' Takes in a string[] and outputs a corresponding Dictionary[string, typeof(string)] to feed through the search
   ''' </summary>
   ''' <param name="columns_to_search">array of column names</param>
   ''' <returns>Dictionary[string, typeof(string)]</returns>
   Private Function MakeDictionary(ByVal columns_to_search As String()) As Dictionary(Of String, Type)
      Dim columns As New Dictionary(Of String, Type)()
      For Each s As String In columns_to_search
         columns.Add(s, GetType(String))
      Next
      Return columns
   End Function
End Module