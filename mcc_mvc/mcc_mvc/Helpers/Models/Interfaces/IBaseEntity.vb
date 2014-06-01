Imports Microsoft.VisualBasic

Public Interface IBaseEntity

   ReadOnly Property IsValid() As Boolean
   Property IsDirty() As Boolean

   ReadOnly Property CanEdit() As Boolean
   ReadOnly Property CanRead() As Boolean
   ReadOnly Property CanDelete() As Boolean
   ReadOnly Property CanAdd() As Boolean

End Interface
