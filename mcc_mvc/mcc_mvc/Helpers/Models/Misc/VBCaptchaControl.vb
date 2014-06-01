Imports System.IO
Imports System.Math
Imports System.Drawing
Imports System.Drawing.Drawing2D
Imports System.Drawing.Imaging

' About this Captcha Control:
' - Displays a mix of letter and numbers over a light grey and 
' color-dotted background
' - Random security codes contain between 4 and 6 characters
' - No 'O' nor 'Q' letters, nor 'ZERO' figure to avoid confusion
' - Custom size (Width and Height), text size is proportionate

Public Class VBCaptchaControl

   Private m_Random As Random
   Private m_PaletteBack As List(Of Color)
   Private m_PaletteText As List(Of Brush)
   Private m_PaletteDots As List(Of Brush)
   Private m_FontSizes As List(Of Integer)
   Private m_Fonts As List(Of String)




   Private _captchaId As Guid
   Public Property CaptchaId() As Guid
      Get
         Return _captchaId
      End Get
      Set(ByVal value As Guid)
         _captchaId = value
      End Set
   End Property


   Private m_RandomCode As String
   Public Property RandomCode() As String
      Get
         Return m_RandomCode
      End Get
      Set(ByVal value As String)
         m_RandomCode = value
      End Set
   End Property



   Private m_Img As Bitmap
   Public Property CaptchaImage() As Bitmap
      Get
         Return m_Img
      End Get
      Set(ByVal value As Bitmap)
         m_Img = value
      End Set
   End Property



   Private Const CHAR_IMG_MIN_DIM_RATIO As Double = 0.5 ' Percentage of the image's minimum dimension covered by a character
   Private Const CHARACTERS As String = "123456789WERTYUIPASDFGHJKLZXCVBNM"
   Private Const NB_DOT As Integer = 100
   Private Const MAX_DEVIATION_ANGLE As Integer = 4

   Public Sub New(ByVal WidthInPixel As Integer, ByVal HeightInPixel As Integer)
      Try
         m_RandomCode = ""

         m_Img = New Bitmap(WidthInPixel, HeightInPixel)

         StartRandomGenerator()

         CreateDrawingTools(WidthInPixel, HeightInPixel)
      Catch ex As Exception
         MsgBox("ERROR: Can't create CAPTCHA:" + vbCrLf + ex.ToString)
      End Try
   End Sub

   Public Function ValidateUserInput(ByVal Input As String) As Boolean
      Return (m_RandomCode = Input)
   End Function

   Public Function GetCaptchaAs(ByVal Format As ImageFormat, ByRef StreamImg As Stream) As Boolean
      If m_Img Is Nothing Or _
         StreamImg Is Nothing Then
         Return (False)
      End If

      Dim res As Boolean = True

      Try
         m_Img.Save(StreamImg, Format)
         m_Img.Dispose()
      Catch ex As Exception
         res = False
      End Try

      Return (res)
   End Function

   'Public Function GetCaptchaImageAs(ByVal Format As ImageFormat) As Stream
   '   Dim StreamImg As New StreamWriter(
   '   If m_Img Is Nothing  Then
   '      Return Nothing
   '   End If

   '   Dim res As Boolean = True

   '   Try
   '      m_Img.Save(StreamImg, Format)
   '      m_Img.Dispose()
   '   Catch ex As Exception
   '      res = False
   '   End Try

   'End Function

   Public Function BuildCaptcha(ByVal bAddDots As Boolean) As Boolean
      If m_Img Is Nothing Then
         Return (False)
      End If

      Dim res As Boolean = True

      Try
         PaintSolidRectangle()

         Dim g As Graphics = Graphics.FromImage(m_Img)
         g.SmoothingMode = Drawing2D.SmoothingMode.HighQuality

         If bAddDots Then
            AddDots(g)
         End If

         GenerateRandomCode()

         PaintRandomCode(g)

         'Dim pts() As PointF = {New Point(0, 0), _
         '                       New Point(10, 40), _
         '                       New Point(20, 60), _
         '                       New Point(30, 30), _
         '                       New Point(40, 80), _
         '                       New Point(50, 20), _
         '                       New Point(60, 90), _
         '                       New Point(70, 180), _
         '                       New Point(80, 60)}
         '
         'g.DrawCurve(Pens.DarkViolet, pts, 1.5)
      Catch ex As Exception
         res = False
      End Try

      Return (res)
   End Function

   Private Sub GenerateRandomCode()
      Try
         Dim L As Integer = m_Random.Next(4, 7)

         For i As Integer = 0 To L - 1 Step 1
            Dim CharacterIndex As Integer = m_Random.Next(0, CHARACTERS.Length)

            Dim RandCharacter As Char = CHARACTERS.Chars(CharacterIndex)

            m_RandomCode += RandCharacter
         Next i
      Catch ex As Exception
         Throw (ex)
      End Try
   End Sub

   Private Sub PaintRandomCode(ByRef g As Graphics)
      Try
         Dim H, W As Integer

         GetImageDimension(H, W)

         Dim RefX As Integer = 5
         Dim RefY As Integer = H * (1 - CHAR_IMG_MIN_DIM_RATIO)
         Dim X As Integer = RefX
         Dim Y As Integer = RefY
         Dim L As Integer = m_RandomCode.Length

         'Dim RandAngle As Integer = m_Random.Next(MAX_DEVIATION_ANGLE)
         'Dim AngleSign As Boolean = CBool(m_Random.Next(0, 2))

         'If Not AngleSign Then
         '   RandAngle = -RandAngle
         'End If

         'g.RotateTransform(RandAngle)

         For i As Integer = 0 To L Step 1
            Dim RandCharacter As Char = m_RandomCode.Chars(i)

            Dim FontIndex As Integer = m_Random.Next(0, m_Fonts.Count)
            Dim RandFont As String = m_Fonts(FontIndex)

            Dim SizeIndex As Integer = m_Random.Next(0, m_FontSizes.Count)
            Dim RandSize As Integer = m_FontSizes(SizeIndex)

            Dim MyFont As New Font(RandFont, RandSize, FontStyle.Bold)
            Dim Delta = MyFont.GetHeight() / 2

            Dim BrushIndex As Integer = m_Random.Next(0, m_PaletteText.Count)
            Dim RandBrush As Brush = m_PaletteText(BrushIndex)

            Y -= RandSize

            g.DrawString(RandCharacter, MyFont, RandBrush, X, Y)

            X += (Delta * 1.35)
            Y = RefY
         Next i
      Catch ex As Exception
         Throw (ex)
      End Try
   End Sub

   Private Sub AddDots(ByRef g As Graphics)
      Try
         Dim H As Integer
         Dim W As Integer

         GetImageDimension(H, W)

         Dim MinDotDiameter As Integer = 2
         Dim MaxDotDiameter As Integer = Min(H / 10, W / 10) + 3

         For i As Integer = 0 To NB_DOT Step 1
            Dim BrushIndex As Integer = m_Random.Next(0, m_PaletteDots.Count)
            Dim MyBrush As Brush = m_PaletteDots(BrushIndex)

            Dim Diameter As Integer = m_Random.Next(MinDotDiameter, MaxDotDiameter)
            Dim X As Integer = m_Random.Next(W - Diameter)
            Dim Y As Integer = m_Random.Next(H - Diameter)

            g.FillEllipse(MyBrush, X, Y, Diameter, Diameter)
         Next i
      Catch ex As Exception
         Throw (ex)
      End Try
   End Sub

   Private Sub CreateDrawingTools(ByVal ImgWidthInPixel As Integer, ByVal ImgHeightInPixel As Integer)
      Try
         m_PaletteText = New List(Of Brush)
         m_PaletteText.Add(Brushes.Red)
         m_PaletteText.Add(Brushes.Green)
         m_PaletteText.Add(Brushes.DarkBlue)
         m_PaletteText.Add(Brushes.DarkCyan)
         m_PaletteText.Add(Brushes.Black)
         m_PaletteText.Add(Brushes.Brown)

         m_PaletteBack = New List(Of Color)
         m_PaletteBack.Add(Color.LightGray)
         m_PaletteBack.Add(Color.LightSkyBlue)
         m_PaletteBack.Add(Color.LightYellow)
         m_PaletteBack.Add(Color.WhiteSmoke)

         m_PaletteDots = New List(Of Brush)
         m_PaletteDots.Add(Brushes.Gray)
         m_PaletteDots.Add(Brushes.LightGreen)
         m_PaletteDots.Add(Brushes.Magenta)
         m_PaletteDots.Add(Brushes.Pink)
         m_PaletteDots.Add(Brushes.Yellow)
         m_PaletteDots.Add(Brushes.Cyan)

         Dim FontSizeBase As Integer = Min(ImgHeightInPixel * CHAR_IMG_MIN_DIM_RATIO, _
                                           ImgWidthInPixel * CHAR_IMG_MIN_DIM_RATIO)

         m_FontSizes = New List(Of Integer)
         m_FontSizes.Add(FontSizeBase - 3)
         m_FontSizes.Add(FontSizeBase)
         m_FontSizes.Add(FontSizeBase + 3)

         m_Fonts = New List(Of String)
         m_Fonts.Add("Arial")
         m_Fonts.Add("Times New Roman")
         m_Fonts.Add("Comic Sans MS")
      Catch ex As Exception
         Throw (ex)
      End Try
   End Sub

   Private Sub StartRandomGenerator()
      Try
         Dim Seed As Integer = New Random(TimeOfDay.Hour + TimeOfDay.Minute + TimeOfDay.Second).Next
         'Dim Seed As Integer = TimeOfDay.Hour + TimeOfDay.Minute + TimeOfDay.Second

         m_Random = New Random(Seed)
      Catch ex As Exception
         Throw (ex)
      End Try
   End Sub

   Private Sub GetImageDimension(ByRef H As Integer, ByRef W As Integer)
      Try
         Dim Rect As RectangleF = m_Img.GetBounds(GraphicsUnit.Pixel)
         H = CInt(Rect.Height)
         W = CInt(Rect.Width)
      Catch ex As Exception
         Throw (ex)
      End Try
   End Sub

   Private Sub PaintSolidRectangle()
      Try
         Dim H As Integer
         Dim W As Integer

         GetImageDimension(H, W)

         Dim ColorIndex As Integer = m_Random.Next(0, m_PaletteBack.Count)
         Dim MyColor As Color = m_PaletteBack(ColorIndex)

         For i As Integer = 0 To W - 1 Step 1
            For j As Integer = 0 To H - 1 Step 1
               m_Img.SetPixel(i, j, MyColor)
            Next j
         Next i
      Catch ex As Exception
         Throw (ex)
      End Try
   End Sub
End Class
